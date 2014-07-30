/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
//#define PLAY_TESTING

using UnityEngine;
using System.Collections;
using InControl;

// Require a character controller to be attached to the same game object
[RequireComponent (typeof (CharacterController))]

[System.Serializable]
public class NinjaSettings
{
	public float walkSpeed;
	public float runSpeed;
	public float jumpHeight;
	
	public float walkMaxAnimationSpeed;
	public float runMaxAnimationSpeed;
	public float jumpAnimationSpeed;
	public float landAnimationSpeed;

	public float gravity;
	public float speedSmoothing;
	public float jumpRepeat;

	public AudioSource transitionIn;
	public AudioSource transitionOut;

	public GameObject[] effects;
	public Vector3[] effectLocations;
}

[System.Serializable]
public enum NinjaType
{
	Base,
	Air,
	Water,
	Fire
}

public class NinjaController : MonoBehaviour
{
	public float attackDistance = 2.5f;

	public NinjaSettings baseSettings;
	public NinjaSettings airSettings;
	public NinjaSettings fireSettings;
	public NinjaSettings waterSettings;

	public AudioSource waterWalkingAudio;
	public AudioSource metalWalkingAudio;
	public AudioSource attackAudio;
	public AudioSource attackHitAudio;
	private AudioSource walkingAudio;

	public MessageManager msgManager;

	private NinjaType ninjaType;
	private Renderer[] ninjaRenderers;
	private GameObject ninjaEffects;

	enum CharacterGround
	{
		Grass,
		Wood,
		Water
	}
	
	// The speed when walking
	private float walkSpeed;
    // when pressing "Fire3" button (cmd) we start running
	private float runSpeed;
	// How high do we jump when pressing jump and letting go immediately
	private float jumpHeight;
	// The gravity for the character
	private float gravity;
	// The gravity in controlled descent mode
	private float speedSmoothing;
	
	private float pushPower = 2.0f;

    private float jumpRepeatTime = 0.25f;
    private float groundedTimeout = 0.25f;
	private float jumpTimeout = 0.25f;

    // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
    private float lockCameraTimer = 0.0f;

    // The current move direction in x-z
    private Vector3 moveDirection = Vector3.zero;
    // The current vertical speed
    private float verticalSpeed = 0.0f;
    // The current x-z move speed
    private float moveSpeed = 0.0f;

    // The last collision flags returned from controller.Move
    private CollisionFlags collisionFlags;

    // Are we jumping? (Initiated with jump button and not grounded yet)
    private bool jumping = false;
    private bool jumpingReachedApex = false;

    // Are we moving backwards (This locks the camera to not do a 180 degree spin)
    private bool movingBack = false;
    // Is the user pressing any keys?
    private bool isMoving = false;
    // Last time the jump button was clicked down
    private float lastJumpButtonTime = -10.0f;
    // Last time we performed a jump
    private float lastJumpTime = -1.0f;

	private bool attacking = false;
	
    private Vector3 inAirVelocity = Vector3.zero;

    private float lastGroundedTime = 0.0f;

	private InputDevice inputDevice;

	// Energy levels
	private float zenEnergy = 100.0f;
	public float getZen(){return zenEnergy;}
	private float airEnergy = 100.0f;
	public float getAirEnergy(){return airEnergy;}
	private float fireEnergy = 100.0f;
	public float getFireEnergy(){return fireEnergy;}
	private float waterEnergy = 100.0f;
	public float getWaterEnergy(){return waterEnergy;}
	private float MAX_BAR_VALUE = 100.0f;

	public StatsInfo currentStats;
	public StatsInfo totalStats;

	private bool isControllable = true;

	/// <summary>
	/// Sets the control.
	/// </summary>
	/// <param name="controllable">If set to <c>true</c> controllable.</param>
	public void setControl(bool controllable)
	{
		isControllable = controllable;
	}

    void Awake()
    {
		currentStats = new StatsInfo ();
		// Setup the input manager
		InputManager.Setup ();
		// Get the ninja renderer
		ninjaRenderers = GetComponentsInChildren<Renderer> ();
		// Search for the Effects holder
		foreach(Transform tf in GetComponentsInChildren<Transform> ())
		{
			// Is this the effects game object?
			if (tf.name == "Effects")
			{
				// Save the game object
				ninjaEffects = tf.gameObject;
				// We are done here
				break;
			}
		}
		// Initialize GUI
		initializeGUI ();
		// Set the base ninja
		setBaseNinja ();
		// Set the base walking audio
		walkingAudio = metalWalkingAudio;
		// Initialize the move direction
        moveDirection = transform.TransformDirection(Vector3.forward);
    }

    void UpdateSmoothedMovementDirection()
    {
        Transform cameraTransform = Camera.main.transform;

        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

		float v = inputDevice.LeftStickY.Value;
		float h = inputDevice.LeftStickX.Value;

        // Are we moving backwards or looking backwards
        if (v < -0.2f)
            movingBack = true;
        else
            movingBack = false;

        bool wasMoving = isMoving;
        isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        // Target direction relative to the camera
        Vector3 targetDirection = h * right + v * forward;

        // Lock camera for short period when transitioning moving & standing still
        lockCameraTimer += Time.deltaTime;
        if (isMoving != wasMoving)
            lockCameraTimer = 0.0f;

        // We store speed and direction seperately,
        // so that when the character stands still we still have a valid forward direction
        // moveDirection is always normalized, and we only update it if there is user input.
        if (targetDirection != Vector3.zero)
        {
			moveDirection = targetDirection.normalized;
        }

        // Smooth the speed based on the current target direction
        float curSmooth = speedSmoothing * Time.deltaTime;

        // Choose target speed
        // We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
        float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

        // Pick speed modifier
		if (inputDevice.LeftTrigger.IsPressed)
        {
            targetSpeed *= runSpeed;
        }
        else
        {
            targetSpeed *= walkSpeed;
        }

        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
    }

	void ApplyAttacking()
	{
		// Prevent attacking too fast after each other
		if (attacking || !isControllable)
			return;
		
		if (!IsAttacking() && inputDevice.RightTrigger.WasPressed)
		{
			DidAttack();
		}
	}

    void ApplyJumping()
    {
        // Prevent jumping too fast after each other
        if (lastJumpTime + jumpRepeatTime > Time.time)
            return;

		if (IsGrounded())
		{
            // Jump
            // - Only when pressing the button down	
			if (Time.time < lastJumpButtonTime + jumpTimeout)
            {
                verticalSpeed = CalculateJumpVerticalSpeed(jumpHeight);
				DidJump();
            }
        }
    }

    void ApplyGravity()
    {
        if (isControllable)	// don't move player at all if not controllable.
        {
            // When we reach the apex of the jump we send out a message
            if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
            {
                jumpingReachedApex = true;
            }

            if (IsGrounded())
                verticalSpeed = 0.0f;
            else
                verticalSpeed -= gravity * Time.deltaTime;
        }
    }

    public float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt (2 * targetJumpHeight * gravity);
    }

	public void playFootstepSound(string data) {
	
		if(string.Equals(data[0], 'r')) {
			walkingAudio.volume = 0.2f;
		} else if (string.Equals(data[0], 'n')) {
			walkingAudio.volume = 0.1f;
		}

		if(string.Equals(data[1], 'l')){
			walkingAudio.pitch = Random.Range(1.0f, 1.1f);
		} else if (string.Equals(data[1], 'r')) {
			walkingAudio.pitch = Random.Range(1.2f, 1.3f);
		}

		walkingAudio.Play();

	}

	public void DidAttack()
	{
		currentStats.numberAttacks++;
		attacking = true;
		StartCoroutine(Attack());
	}
	
	IEnumerator Attack()
	{
		foreach(DamageObject obj in FindObjectsOfType<DamageObject> ())
		{
			// Make sure the object is alive
			if (!obj.alive) continue;

			// See how close we are to the object
			float dist = Vector3.Distance(transform.position, obj.transform.position);

			// Are we close enough?
			if (dist <= attackDistance) {
				// Let's check the direction now
				Vector3 dir = (obj.transform.position - transform.position).normalized;
				float direction = Vector3.Dot(dir, transform.forward);
				if (direction > 0.65f)
				{
					// Deal damage to the object
					obj.DealDamage(15.0f);
					attackHitAudio.Play();

					// Check for death
					if (!obj.alive)
					{
						currentStats.numberTroopsKilled++;
					}
				}
				else
				{
					attackAudio.Play();
				}
			} else {
				attackAudio.Play();
			}
		}
		yield return true;
		attacking = false;
	}
	
	public void DidJump()
	{
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		lastJumpButtonTime = -10;
	}

    void FixedUpdate()
	{
		InputManager.Update();
		// Use last device which provided input.
		inputDevice = InputManager.ActiveDevice;
		if (!isControllable)
		{
			return;
		}

		currentStats.addHealthPoint (zenEnergy);

		updateEnergyValues ();
		handleNinjaChange ();

		if (inputDevice.Action1.IsPressed)
        {
            lastJumpButtonTime = Time.time;
        }

        UpdateSmoothedMovementDirection();

        // Apply gravity
        // - extra power jump modifies gravity
        // - controlledDescent mode modifies gravity
        ApplyGravity();

        // Apply jumping logic
        ApplyJumping();

		// Apply jumping logic
		ApplyAttacking ();

        // Calculate actual motion
        Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0) + inAirVelocity;
        movement *= Time.deltaTime;

        // Move the controller
        CharacterController controller = GetComponent<CharacterController>();
        collisionFlags = controller.Move(movement);

        // Set rotation to the move direction
        if (IsGrounded())
        {
			transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            Vector3 xzMove = movement;
            xzMove.y = 0;
            if (xzMove.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(xzMove);
            }
        }

        // We are in jump mode but just became grounded
        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
            inAirVelocity = Vector3.zero;
            if (jumping)
            {
                jumping = false;
            }
        }
    }

	/// <summary>
	/// Handles the ninja change.
	/// </summary>
	void handleNinjaChange()
	{
		// Air Ninja
		if (InputManager.ActiveDevice.Action4.WasPressed)
		{
			// See if we are currently the air ninja
			if (ninjaType == NinjaType.Air)
			{
				// Set the base ninja
				setBaseNinja();
			}
			// Not the air ninja
			else
			{
				// Make sure we have enough air energy
				if (airEnergy > 0.0f)
				{
					// Set the air ninja
					setAirNinja();
				}
				// Not enough energy
				else
				{
					// Create a message
					createMessage("Cannot use Air Ninja. Not enough energy!");
				}
			}
		}
		// Fire Ninja
		else if (inputDevice.Action2.WasPressed)
		{
			// See if we are currently the fire ninja
			if (ninjaType == NinjaType.Fire)
			{
				// Set the base ninja
				setBaseNinja();
			}
			// Not the fire ninja
			else
			{
				// Make sure we have enough fire energy
				if (fireEnergy > 0.0f)
				{
					// Set the fire ninja
					setFireNinja();
				}
				// Not enough energy
				else
				{
					// Create a message
					createMessage("Cannot use Fire Ninja. Not enough energy!");
				}
			}
		}
		// Water Ninja
		else if (inputDevice.Action3.WasPressed)
		{
			// See if we are currently the water ninja
			if (ninjaType == NinjaType.Water)
			{
				// Set the base ninja
				setBaseNinja();
			}
			// Not the water ninja
			else
			{
				// Make sure we have enough water energy
				if (waterEnergy > 0.0f)
				{
					// Set the water ninja
					setWaterNinja();
				}
				// Not enough energy
				else
				{
					// Create a message
					createMessage("Cannot use Water Ninja. Not enough energy!");
				}
			}
		}
		// Check for expiring air/water/fire ninja
		if ((airEnergy <= 0.0f && ninjaType == NinjaType.Air) ||
		    (waterEnergy <= 0.0f && ninjaType == NinjaType.Water) ||
		    (fireEnergy <= 0.0f && ninjaType == NinjaType.Fire))
		{
			// Create a message
			createMessage("Ran out of energy!");
			// Go back to the base ninja
			setBaseNinja();
		}
	}

	/// <summary>
	/// Creates a message.
	/// </summary>
	/// <param name="msg">Message.</param>
	/// <param name="lifetime">Lifetime.</param>
	public void createMessage(string msg)
	{
		// Queue the message
		msgManager.queueMessage (msg);
	}

	/// <summary>
	/// Sets the message manager.
	/// </summary>
	/// <param name="manager">Manager.</param>
	public void setMessageManager(MessageManager manager)
	{
		msgManager = manager;
	}

	/// <summary>
	/// Updates the energy values.
	/// </summary>
	void updateEnergyValues()
	{
		// Check the air type
		if (ninjaType == NinjaType.Air)
		{
			currentStats.totalAirTime += 0.05f;
			// Decrement the energy level
			airEnergy -= 0.05f;
			// Make sure we don't fall below zero
			if (airEnergy < 0.0f)
				airEnergy = 0.0f;
		}
		// Check the fire type
		else if (ninjaType == NinjaType.Fire)
		{
			currentStats.totalFireTime += 0.05f;
			// Decrement the energy level
			fireEnergy -= 0.05f;
			// Make sure we don't fall below zero
			if (fireEnergy < 0.0f)
				fireEnergy = 0.0f;
		}
		// Check the water type
		else if (ninjaType == NinjaType.Water)
		{
			currentStats.totalWaterTime += 0.05f;
			// Decrement the energy level
			waterEnergy -= 0.05f;
			// Make sure we don't fall below zero
			if (waterEnergy < 0.0f)
				waterEnergy = 0.0f;
		}
	}

	/// <summary>
	/// Sets the base ninja.
	/// </summary>
	void setBaseNinja()
	{
		// Set the base ninja type
		ninjaType = NinjaType.Base;
		// Set Ninja Settings
		setNinjaSettings (baseSettings);

		// Iterate through each of the renderers
		foreach(Renderer ninjaRenderer in ninjaRenderers)
		{
			// Set the texture alpha
			ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			ninjaRenderer.material.shader = Shader.Find ("Reflective/Bumped Specular");
			ninjaRenderer.material.SetColor ("_ReflectColor", new Color(0.0f, 0.0f, 0.0f, 0.5f));
		}
	}
	/// <summary>
	/// Sets the air ninja.
	/// </summary>
	void setAirNinja()
	{
		// Set the air ninja type
		ninjaType = NinjaType.Air;
		// Set Ninja Settings
		setNinjaSettings (airSettings);
		
		// Iterate through each of the renderers
		foreach(Renderer ninjaRenderer in ninjaRenderers)
		{
			// Set the texture alpha
			ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
			ninjaRenderer.material.shader = Shader.Find ("Transparent/Bumped Specular");
			ninjaRenderer.material.SetColor ("_ReflectColor", new Color(1.0f, 1.0f, 1.0f, 0.5f));
		}
	}
	/// <summary>
	/// Sets the fire ninja.
	/// </summary>
	void setFireNinja()
	{
		// Set the fire ninja type
		ninjaType = NinjaType.Fire;
		// Set Ninja Settings
		setNinjaSettings (fireSettings);
		
		// Iterate through each of the renderers
		foreach(Renderer ninjaRenderer in ninjaRenderers)
		{
			// Set the texture alpha
			ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			ninjaRenderer.material.shader = Shader.Find ("Reflective/Bumped Specular");
			ninjaRenderer.material.SetColor ("_ReflectColor", new Color(1.0f, 0.0f, 0.0f, 0.5f));
		}
	}
	/// <summary>
	/// Sets the water ninja.
	/// </summary>
	void setWaterNinja()
	{
		// Set the water ninja type
		ninjaType = NinjaType.Water;
		// Set Ninja Settings
		setNinjaSettings (waterSettings);
		
		// Iterate through each of the renderers
		foreach(Renderer ninjaRenderer in ninjaRenderers)
		{
			// Set the texture alpha
			ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			ninjaRenderer.material.shader = Shader.Find ("Reflective/Bumped Specular");
			ninjaRenderer.material.SetColor ("_ReflectColor", new Color(0.0f, 0.0f, 1.0f, 0.5f));
		}
	}
	/// <summary>
	/// Sets the ninja settings.
	/// </summary>
	/// <param name="settings">Ninja Settings.</param>
	void setNinjaSettings(NinjaSettings settings)
	{
		// Clear the effects
		for (var i = ninjaEffects.transform.childCount - 1; i >= 0; i--)
		{
			// Get the effect
			Transform effect = ninjaEffects.transform.GetChild(i);
			// Clear the child/parent relationship
			effect.parent = null;
			// Destroy the effect
			Destroy(effect.gameObject);
		} 
		// Set the jump repeat time
		jumpRepeatTime = settings.jumpRepeat;
		// Set the walk/run/jump values
		walkSpeed = settings.walkSpeed;
		runSpeed = settings.runSpeed;
		jumpHeight = settings.jumpHeight;
		// Set gravity/speed smoothing/rotate speed
		gravity = settings.gravity;
		speedSmoothing = settings.speedSmoothing;
		// Add the effects
		for(int i = 0; i < settings.effects.Length; i++)
		{
			// Instantiate the effect
			GameObject newEffect = (GameObject)Instantiate(settings.effects[i],
			                                               // Set the location of the parent plus the effect location
			                                               ninjaEffects.transform.position + settings.effectLocations[i],
			                                               // Set the rotation to that of the parent
			                                               ninjaEffects.transform.rotation);
			// Add the effect to the children
			newEffect.transform.parent = ninjaEffects.transform;
		}
	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter(Collider collider)
	{
		// Check to see if the collided game object has an InteractiveObject
		InteractiveObject interactive = collider.gameObject.GetComponent<InteractiveObject> ();
		if (interactive != null)
		{
			// See if the object type was of water
			if (interactive.getObjectType() == ObjectType.Water)
			{
				if (walkingAudio != waterWalkingAudio)
				{
					walkingAudio.Stop ();
				}
				walkingAudio = waterWalkingAudio;
			}
			// Broadcast to the game object that there was a player collision
			interactive.PlayerCollision(new InteractiveCollision(collider, ninjaType));
		}
	}

	bool zenCharging = false;
	bool airCharging = false;
	bool fireCharging = false;
	bool waterCharging = false;

	/// <summary>
	/// Raises the trigger stay event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerStay(Collider collider)
	{
		// See if we are paused
		if (GameController.controller.isPaused())
			// Nothing to do
			return;
		// Check for the interactive object script
		InteractiveObject obj = (InteractiveObject)collider.gameObject.GetComponentInChildren<InteractiveObject> ();
		if (obj != null)
		{
			obj.PlayerCollisionStay(new InteractiveCollision(collider, ninjaType));
			if (ninjaType == NinjaType.Base || 
			    (ninjaType == NinjaType.Air && obj.getObjectType() == ObjectType.Air) ||
			    (ninjaType == NinjaType.Fire && obj.getObjectType() == ObjectType.Fire) ||
			    (ninjaType == NinjaType.Water && obj.getObjectType() == ObjectType.Water))
			{
				switch(obj.getObjectType())
				{
				case ObjectType.Air:
					currentStats.totalAirCharging += 0.2f;
					airCharging = true;
					airEnergy += 0.2f;
					if (airEnergy > 100.0f)
					{
						zenCharging = true;
						zenEnergy += 0.1f;
						if (zenEnergy > 100.0f)
							zenEnergy = 100.0f;
						airEnergy = 100.0f;
					}
					break;
				case ObjectType.Fire:
					currentStats.totalFireCharging += 0.2f;
					fireCharging = true;
					fireEnergy += 0.2f;
					if (fireEnergy > 100.0f)
					{
						zenCharging = true;
						zenEnergy += 0.1f;
						if (zenEnergy > 100.0f)
							zenEnergy = 100.0f;
						fireEnergy = 100.0f;
					}
					break;
				case ObjectType.Water:
					currentStats.totalWaterCharging += 0.2f;
					waterCharging = true;
					waterEnergy += 0.2f;
					if (waterEnergy > 100.0f)
					{
						zenCharging = true;
						zenEnergy += 0.1f;
						if (zenEnergy > 100.0f)
							zenEnergy = 100.0f;
						waterEnergy = 100.0f;
					}
					break;
				}
			}
		}
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerExit(Collider collider)
	{
		walkingAudio.Stop ();
		walkingAudio = metalWalkingAudio;
		// Check for the interactive object script
		InteractiveObject obj = (InteractiveObject)collider.gameObject.GetComponentInChildren<InteractiveObject> ();
		if (obj != null)
		{
			obj.PlayerCollisionStay(new InteractiveCollision(collider, ninjaType));
			if (ninjaType == NinjaType.Base || 
			    (ninjaType == NinjaType.Air && obj.getObjectType() == ObjectType.Air) ||
			    (ninjaType == NinjaType.Fire && obj.getObjectType() == ObjectType.Fire) ||
			    (ninjaType == NinjaType.Water && obj.getObjectType() == ObjectType.Water))
			{
				switch(obj.getObjectType())
				{
				case ObjectType.Air:
					airCharging = false;
					zenCharging = false;
					break;
				case ObjectType.Fire:
					fireCharging = false;
					zenCharging = false;
					break;
				case ObjectType.Water:
					waterCharging = false;
					zenCharging = false;
					break;
				}
			}
		}
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3 || !body || body.isKinematic) 
			return;
		
		// Calculate push direction from move direction, 
		// we only push objects to the sides never up and down
		Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);
		// If you know how fast your character is trying to move,
		// then you can also multiply the push velocity by that.

		// open the door faster if they're running
		if (inputDevice.LeftTrigger.IsPressed)
		{
			pushPower = 4.0f;
		}
		else
		{
			pushPower = 2.0f;
		}
		// Apply the push
		body.velocity = pushDir * pushPower;
    }

	// STYLES
	GUIStyle zenStyle = new GUIStyle ();
	GUIStyle airStyle = new GUIStyle ();
	GUIStyle fireStyle = new GUIStyle ();
	GUIStyle waterStyle = new GUIStyle ();
	GUIStyle pulseStyle = new GUIStyle ();
	
	// TEXTURES
	Texture2D zenTexture;
	Texture2D airTexture;
	Texture2D fireTexture;
	Texture2D waterTexture;
	Texture2D pulseTexture;

	void initializeGUI()
	{
		// Zen bar
		zenTexture = new Texture2D(1, 1);
		zenTexture.SetPixel (0, 0, new Color(1.0f, 0.92f, 0.016f, 0.2f));
		zenTexture.Apply ();
		zenStyle.normal.background = zenTexture;
		// Air bar
		airTexture = new Texture2D(1, 1);
		airTexture.SetPixel (0, 0, new Color(1.0f, 1.0f, 1.0f, 0.2f));
		airTexture.Apply ();
		airStyle.normal.background = airTexture;
		// Fire bar
		fireTexture = new Texture2D(1, 1);
		fireTexture.SetPixel (0, 0, new Color(1.0f, 0.0f, 0.0f, 0.2f));
		fireTexture.Apply ();
		fireStyle.normal.background = fireTexture;
		// Water bar
		waterTexture = new Texture2D(1, 1);
		waterTexture.SetPixel (0, 0, new Color(0.0f, 0.0f, 1.0f, 0.2f));
		waterTexture.Apply ();
		waterStyle.normal.background = waterTexture;
		// Pulse Texture
		pulseTexture = new Texture2D (1, 1);
		pulseTexture.SetPixel (0, 0, new Color(0.0f, 1.0f, 0.0f, 0.2f));
		pulseTexture.Apply ();
		pulseStyle.normal.background = pulseTexture;
	}
	float timePassed = 0.0f;
	bool increasing = true;
	/// <summary>
	/// Raises the GUI event.
	/// </summary>
	void OnGUI()
	{
		// Define some constant for all the bars
		float top = GUI.skin.box.padding.top;
		float left = GUI.skin.box.padding.left;
		float containerWidth = 80.0f;
		float containerHeight = 30.0f;
		float height = containerHeight - GUI.skin.box.padding.vertical;
		float energyContainerTop = GUI.skin.window.padding.top + height + GUI.skin.window.padding.bottom;

		// Zen Bar Container
		float zenContainerWidth = 250.0f;
		float zenContainerLeft = Screen.width - zenContainerWidth - GUI.skin.window.padding.right;
		
		// Zen Bar 
		float zenWidth = (zenEnergy / MAX_BAR_VALUE) * (zenContainerWidth - GUI.skin.box.padding.horizontal);
		
		// Zen GUI
		GUI.BeginGroup (new Rect (zenContainerLeft, GUI.skin.window.padding.top, zenContainerWidth, containerHeight));
		GUI.Box(new Rect(0, 0, zenContainerWidth, containerHeight), ((int)zenEnergy).ToString());
		if (zenCharging)
		{
			if (increasing)
			{
				timePassed += Time.deltaTime;
				if (timePassed > 1.0f)
				{
					increasing = false;
					timePassed = 1.0f;
				}
			}
			else
			{
				timePassed -= Time.deltaTime;
				if (timePassed < 0.0f)
				{
					increasing = true;
					timePassed = 0.0f;
				}
			}
			pulseTexture.SetPixel (0, 0, new Color(0.0f, 1.0f, 0.0f, timePassed * 0.2f));
			pulseTexture.Apply();
			GUI.Box(new Rect(0, 0, zenContainerWidth, containerHeight), "", pulseStyle);
		}
		GUI.Box(new Rect(left, top, zenWidth, height), "", zenStyle);
		GUI.EndGroup ();

		// Fire Bar Container
		float fireContainerLeft = Screen.width - containerWidth - GUI.skin.window.padding.right;
		
		// Fire Bar 
		float fireWidth = (fireEnergy / MAX_BAR_VALUE) * (containerWidth - GUI.skin.box.padding.horizontal);

		// Fire GUI
		GUI.BeginGroup (new Rect (fireContainerLeft, energyContainerTop, containerWidth, containerHeight));
		GUI.Box(new Rect(0, 0, containerWidth, containerHeight), ((int)fireEnergy).ToString());
		if (fireCharging)
		{
			if (increasing)
			{
				timePassed += Time.deltaTime;
				if (timePassed > 1.0f)
				{
					increasing = false;
					timePassed = 1.0f;
				}
			}
			else
			{
				timePassed -= Time.deltaTime;
				if (timePassed < 0.0f)
				{
					increasing = true;
					timePassed = 0.0f;
				}
			}
			pulseTexture.SetPixel (0, 0, new Color(0.0f, 1.0f, 0.0f, timePassed * 0.2f));
			pulseTexture.Apply();
			GUI.Box(new Rect(0, 0, containerWidth, containerHeight), "", pulseStyle);
		}
		GUI.Box(new Rect(left, top, fireWidth, height), "", fireStyle);
		GUI.EndGroup ();

		// Air Bar Container
		float airContainerLeft = fireContainerLeft - containerWidth - 5.0f;
		
		// Air Bar 
		float airWidth = (airEnergy / MAX_BAR_VALUE) * (containerWidth - GUI.skin.box.padding.horizontal);

		// Air GUI
		GUI.BeginGroup (new Rect (airContainerLeft, energyContainerTop, containerWidth, containerHeight), GUIStyle.none);
		GUI.Box(new Rect(0, 0, containerWidth, containerHeight), ((int)airEnergy).ToString());
		if (airCharging)
		{
			if (increasing)
			{
				timePassed += Time.deltaTime;
				if (timePassed > 1.0f)
				{
					increasing = false;
					timePassed = 1.0f;
				}
			}
			else
			{
				timePassed -= Time.deltaTime;
				if (timePassed < 0.0f)
				{
					increasing = true;
					timePassed = 0.0f;
				}
			}
			pulseTexture.SetPixel (0, 0, new Color(0.0f, 1.0f, 0.0f, timePassed * 0.2f));
			pulseTexture.Apply();
			GUI.Box(new Rect(0, 0, containerWidth, containerHeight), "", pulseStyle);
		}
		GUI.Box(new Rect(left, top, airWidth, height), "", airStyle);
		GUI.EndGroup ();
		
		// Water Bar Container
		float waterContainerLeft = airContainerLeft - containerWidth - 5.0f;
		
		// Water Bar 
		float waterWidth = (waterEnergy / MAX_BAR_VALUE) * (containerWidth - GUI.skin.box.padding.horizontal);
		
		// Water GUI
		GUI.BeginGroup (new Rect (waterContainerLeft, energyContainerTop, containerWidth, containerHeight));
		GUI.Box(new Rect(0, 0, containerWidth, containerHeight), ((int)waterEnergy).ToString());
		if (waterCharging)
		{
			if (increasing)
			{
				timePassed += Time.deltaTime;
				if (timePassed > 1.0f)
				{
					increasing = false;
					timePassed = 1.0f;
				}
			}
			else
			{
				timePassed -= Time.deltaTime;
				if (timePassed < 0.0f)
				{
					increasing = true;
					timePassed = 0.0f;
				}
			}
			pulseTexture.SetPixel (0, 0, new Color(0.0f, 1.0f, 0.0f, timePassed * 0.2f));
			pulseTexture.Apply();
			GUI.Box(new Rect(0, 0, containerWidth, containerHeight), "", pulseStyle);
		}
		GUI.Box(new Rect(left, top, waterWidth, height), "", waterStyle);
		GUI.EndGroup ();
	}

	public void Damage (float amount) {
		zenEnergy -= amount;
	}

	public Vector3 velocity {
		get { return moveSpeed * moveDirection; }
	}

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public bool IsJumping()
    {
        return jumping;
    }

    public bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

	public bool IsAttacking()
	{
		return attacking;
	}

    public Vector3 GetDirection()
    {
        return moveDirection;
    }

    public bool IsMovingBackwards()
    {
        return movingBack;
    }

    public float GetLockCameraTimer()
    {
        return lockCameraTimer;
    }

    public bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }

    public bool HasJumpReachedApex()
    {
        return jumpingReachedApex;
    }

    public bool IsGroundedWithTimeout()
    {
        return lastGroundedTime + groundedTimeout > Time.time;
    }

    public void Reset()
    {
        gameObject.tag = "Player";
    }
}