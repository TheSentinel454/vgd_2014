/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/

using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent (typeof (CharacterController))]

[System.Serializable]
public class NinjaSettings
{
	public Texture texture;

	public float walkSpeed;
	public float runSpeed;
	public float jumpHeight;
	
	public float walkMaxAnimationSpeed;
	public float runMaxAnimationSpeed;
	public float jumpAnimationSpeed;
	public float landAnimationSpeed;

	public float gravity;
	public float speedSmoothing;
	public float rotateSpeed;

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
	private Animation _animation;
    public AnimationClip idleAnimation;
    public AnimationClip walkAnimation;
    public AnimationClip runAnimation;
    public AnimationClip jumpPoseAnimation;
	public AnimationClip moveAttackAnimation;
	public AnimationClip idleAttackAnimation;

	public BoxCollider attackCollider;
	public CapsuleCollider damageCollider;

	public GameObject warningMsg;

	public NinjaSettings baseSettings;
	public NinjaSettings airSettings;
	public NinjaSettings fireSettings;
	public NinjaSettings waterSettings;

	public AudioSource baseWalkingAudio;
	public AudioSource woodWalkingAudio;
	public AudioSource waterWalkingAudio;
	public AudioSource grassWalkingAudio;

	private AudioSource walkingAudio;
	private NinjaType ninjaType;
	private Renderer ninjaRenderer;
	private GameObject ninjaEffects;

	enum CharacterGround
	{
		Grass,
		Wood,
		Water
	}

    enum CharacterState
    {
        Idle = 0,
        Walking = 1,
        Running = 2,
        Jumping = 3,
    }

	private CharacterState _characterState;

	// Animation speeds
	private float walkMaxAnimationSpeed;
	private float runMaxAnimationSpeed;
	private float jumpAnimationSpeed;
	private float landAnimationSpeed;
	
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
	private float rotateSpeed;
	
	private float pushPower = 2.0f;

    private float inAirControlAcceleration = 3.0f;

    private float jumpRepeatTime = 0.05f;
    private float jumpTimeout = 0.15f;
    private float groundedTimeout = 0.25f;

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

	// Energy levels
	private float zenEnergy = 100.0f;
	public float getZen(){return zenEnergy;}
	private float airEnergy = 100.0f;
	public float getAirEnergy(){return airEnergy;}
	private float fireEnergy = 100.0f;
	public float getFireEnergy(){return fireEnergy;}
	private float waterEnergy = 100.0f;
	public float getWaterEnergy(){return waterEnergy;}

    private bool isControllable = true;

    void Awake()
    {
		// Get the ninja renderer
		ninjaRenderer = GetComponentInChildren<Renderer> ();
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
		// Set the base ninja
		setBaseNinja ();
		// Set the base walking audio
		walkingAudio = baseWalkingAudio;
		// Initialize the move direction
        moveDirection = transform.TransformDirection(Vector3.forward);

        _animation = GetComponent<Animation>();
        if (!_animation)
            Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");

        if (!idleAnimation)
        {
            _animation = null;
            Debug.Log("No idle animation found. Turning off animations.");
        }
        if (!walkAnimation)
        {
            _animation = null;
            Debug.Log("No walk animation found. Turning off animations.");
        }
        if (!runAnimation)
        {
            _animation = null;
            Debug.Log("No run animation found. Turning off animations.");
        }
        if (!jumpPoseAnimation)
        {
            _animation = null;
            Debug.Log("No jump animation found. Turning off animations.");
        }
		if (!moveAttackAnimation)
		{
			_animation = null;
			Debug.Log("No move attack animation found. Turning off animations.");
		}
		if (!idleAttackAnimation)
		{
			_animation = null;
			Debug.Log("No idle attack animation found. Turning off animations.");
		}
    }

    void UpdateSmoothedMovementDirection()
    {
        Transform cameraTransform = Camera.main.transform;
        bool grounded = IsGrounded();

        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // Are we moving backwards or looking backwards
        if (v < -0.2f)
            movingBack = true;
        else
            movingBack = false;

        bool wasMoving = isMoving;
        isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        // Target direction relative to the camera
        Vector3 targetDirection = h * right + v * forward;

        // Grounded controls
        if (grounded)
        {
            // Lock camera for short period when transitioning moving & standing still
            lockCameraTimer += Time.deltaTime;
            if (isMoving != wasMoving)
                lockCameraTimer = 0.0f;

            // We store speed and direction seperately,
            // so that when the character stands still we still have a valid forward direction
            // moveDirection is always normalized, and we only update it if there is user input.
            if (targetDirection != Vector3.zero)
            {
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                moveDirection = moveDirection.normalized;
            }

            // Smooth the speed based on the current target direction
            float curSmooth = speedSmoothing * Time.deltaTime;

            // Choose target speed
            //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            _characterState = CharacterState.Idle;

            // Pick speed modifier
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                targetSpeed *= runSpeed;
                _characterState = CharacterState.Running;
            }
            else
            {
                targetSpeed *= walkSpeed;
                _characterState = CharacterState.Walking;
            }

            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
        }
        // In air controls
        else
        {
            // Lock camera while in air
            if (jumping)
                lockCameraTimer = 0.0f;

            if (isMoving)
                inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
        }
    }

	void ApplyAttacking()
	{
		// Prevent attacking too fast after each other
		if (attacking)
			return;
		
		if (!IsAttacking() && Input.GetButtonDown("Fire1"))
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
            // - With a timeout so you can press the button slightly before landing		
            if (Time.time < lastJumpButtonTime + jumpTimeout)
            {
                verticalSpeed = CalculateJumpVerticalSpeed(jumpHeight);
				DidJump();
                //SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
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
				DidJumpReachApex();
                //SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
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

	public void DidAttack()
	{
		attacking = true;

		StartCoroutine(BlockAttack());
		Debug.Log ("Did Attack!");
	}
	
	IEnumerator BlockAttack()
	{
		float length;
		if (_characterState == CharacterState.Running || _characterState == CharacterState.Walking)
			length = _animation[moveAttackAnimation.name].length;
		else
			length = _animation[idleAttackAnimation.name].length;
		yield return new WaitForSeconds(length);
		attacking = false;
		print("Attack Complete!");
	}
	
	public void DidJump()
	{
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		lastJumpButtonTime = -10;
		
		_characterState = CharacterState.Jumping;
		//Debug.Log("Did Jump!");
	}

	public void DidJumpReachApex()
	{
		//Debug.Log("Did Reach Apex!");
	}

	public void DidLand()
	{
		//Debug.Log("Did Land!");
	}

    void Update()
	{
		updateEnergyValues ();
		handleNinjaChange ();

        if (!isControllable)
        {
            // kill all inputs if not controllable.
            Input.ResetInputAxes();
        }

        if (Input.GetButtonDown("Jump"))
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

		if (movement != Vector3.zero && !IsJumping())// && IsGrounded())
		{
			if (walkingAudio != null)
			{
				if (!walkingAudio.isPlaying)
					walkingAudio.Play();
				else
				{
					walkingAudio.volume = moveSpeed / runSpeed;
					walkingAudio.pitch = 1.5f + (2.0f * (moveSpeed / runSpeed));
				}
			}
		}
		else
		{
			if (walkingAudio != null && walkingAudio.isPlaying)
			{
				walkingAudio.Stop();
			}
		}

        // Move the controller
        CharacterController controller = GetComponent<CharacterController>();
        collisionFlags = controller.Move(movement);

        // ANIMATION sector
        if (_animation)
        {
            if (_characterState == CharacterState.Jumping)
            {
                if (!jumpingReachedApex)
                {
                    _animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
                    _animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
                    _animation.CrossFade(jumpPoseAnimation.name);
                }
                else
                {
                    _animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
                    _animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
                    _animation.CrossFade(jumpPoseAnimation.name);
                }
            }
            else
            {
                if (controller.velocity.sqrMagnitude < 0.1f)
                {
					_animation.CrossFade(idleAnimation.name);
					
					if (attacking)
					{
						_animation[idleAttackAnimation.name].speed = 1.0f;
						_animation[idleAttackAnimation.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(idleAttackAnimation.name);
					}
                }
                else
                {
                    if (_characterState == CharacterState.Running)
                    {
                        _animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
                        _animation.CrossFade(runAnimation.name);
                    }
                    else if (_characterState == CharacterState.Walking)
                    {
                        _animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
                        _animation.CrossFade(walkAnimation.name);
					}
					if (attacking)
					{
						_animation[moveAttackAnimation.name].speed = 1.0f;
						_animation[moveAttackAnimation.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(moveAttackAnimation.name);
					}
                }
            }
        }
        // ANIMATION sector

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
                SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

	/// <summary>
	/// Handles the ninja change.
	/// </summary>
	void handleNinjaChange()
	{
		// Air Ninja
		if (Input.GetKeyDown ("1"))
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
					createMessage(5.0f, "Cannot use Air Ninja. Not enough energy!");
				}
			}
		}
		// Fire Ninja
		else if (Input.GetKeyDown ("2"))
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
					createMessage(5.0f, "Cannot use Fire Ninja. Not enough energy!");
				}
			}
		}
		// Water Ninja
		else if (Input.GetKeyDown ("3"))
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
					createMessage(5.0f, "Cannot use Water Ninja. Not enough energy!");
				}
			}
		}
		// Check for expiring air/water/fire ninja
		if ((airEnergy <= 0.0f && ninjaType == NinjaType.Air) ||
		    (waterEnergy <= 0.0f && ninjaType == NinjaType.Water) ||
		    (fireEnergy <= 0.0f && ninjaType == NinjaType.Fire))
		{
			// Create a message
			createMessage(5.0f, "Ran out of energy!");
			// Go back to the base ninja
			setBaseNinja();
		}
	}

	/// <summary>
	/// Creates a message.
	/// </summary>
	/// <param name="lifetime">Lifetime.</param>
	/// <param name="msg">Message.</param>
	void createMessage(float lifetime, string msg)
	{
		GameObject obj = (GameObject)Instantiate(warningMsg);
		DestroyGuiTextByTime dest = (DestroyGuiTextByTime)obj.GetComponent<DestroyGuiTextByTime>();
		dest.lifetime = lifetime;
		dest.message = msg;
	}

	/// <summary>
	/// Updates the energy values.
	/// </summary>
	void updateEnergyValues()
	{
		// Check the air type
		if (ninjaType == NinjaType.Air)
		{
			// Decrement the energy level
			airEnergy -= 0.1f;
			// Make sure we don't fall below zero
			if (airEnergy < 0.0f)
				airEnergy = 0.0f;
		}
		// Check the fire type
		else if (ninjaType == NinjaType.Fire)
		{
			// Decrement the energy level
			fireEnergy -= 0.1f;
			// Make sure we don't fall below zero
			if (fireEnergy < 0.0f)
				fireEnergy = 0.0f;
		}
		// Check the water type
		else if (ninjaType == NinjaType.Water)
		{
			// Decrement the energy level
			waterEnergy -= 0.1f;
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
		// Set the texture alpha
		ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);

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
		// Set the texture alpha
		ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 0.5f);

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
		// Set the texture alpha
		ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);

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
		// Set the texture alpha
		ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
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
		// Set the texture
		ninjaRenderer.material.mainTexture = settings.texture;
		// Set the animation speeds
		walkMaxAnimationSpeed = settings.walkMaxAnimationSpeed;
		runMaxAnimationSpeed = settings.runMaxAnimationSpeed;
		jumpAnimationSpeed = settings.jumpAnimationSpeed;
		landAnimationSpeed = settings.landAnimationSpeed;
		// Set the walk/run/jump values
		walkSpeed = settings.walkSpeed;
		runSpeed = settings.runSpeed;
		jumpHeight = settings.jumpHeight;
		// Set gravity/speed smoothing/rotate speed
		gravity = settings.gravity;
		speedSmoothing = settings.speedSmoothing;
		rotateSpeed = settings.rotateSpeed;
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
		if (collider.gameObject.tag == "Water")
		{
			if (walkingAudio != waterWalkingAudio)
			{
				walkingAudio.Stop ();
			}
			walkingAudio = waterWalkingAudio;
		}
		// Check to see if the collided game object has an InteractiveObject
		InteractiveObject interactive = collider.gameObject.GetComponent<InteractiveObject>();
		if (interactive != null)
		{
			// Broadcast to the game object that there was a player collision
			interactive.PlayerCollision(new InteractiveCollision(collider, ninjaType));
			print ("OnTriggerEnter: " + collider.gameObject.name);
		}
	}

	/// <summary>
	/// Raises the trigger stay event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerStay(Collider collider)
	{
		// Check for the interactive object script
		InteractiveObject obj = (InteractiveObject)collider.gameObject.GetComponentInChildren<InteractiveObject> ();
		if (obj != null && ninjaType == NinjaType.Base)
		{
			switch(obj.getObjectType())
			{
			case ObjectType.Air:
				//print ("Air increase due to: " + collider.gameObject.name);
				airEnergy += 0.1f;
				if (airEnergy > 100.0f)
					airEnergy = 100.0f;
				break;
			case ObjectType.Fire:
				//print ("Fire increase due to: " + collider.gameObject.name);
				fireEnergy += 0.1f;
				if (fireEnergy > 100.0f)
					fireEnergy = 100.0f;
				break;
			case ObjectType.Water:
				//print ("Water increase due to: " + collider.gameObject.name);
				waterEnergy += 0.1f;
				if (waterEnergy > 100.0f)
					waterEnergy = 100.0f;
				break;
			}
		}
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerExit(Collider collider)
	{
		if (walkingAudio != grassWalkingAudio)
		{
			walkingAudio.Stop ();
		}
		walkingAudio = baseWalkingAudio;
		
		// Check to see if the collided game object has an InteractiveObject
		InteractiveObject interactive = collider.gameObject.GetComponent<InteractiveObject>();
		if (interactive != null)
		{
			print ("OnTriggerExit: " + collider.gameObject.name);
		}
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;

		if (walkingAudio != waterWalkingAudio)
		{
			if (hit.gameObject.tag == "Wood" && walkingAudio != woodWalkingAudio)
			{
				walkingAudio.Stop ();
				walkingAudio = woodWalkingAudio;
			}
			else if (hit.gameObject.tag == "Ground" && walkingAudio != grassWalkingAudio)
			{
				walkingAudio.Stop ();
				walkingAudio = grassWalkingAudio;
			}
			else if (walkingAudio != baseWalkingAudio)
			{
				walkingAudio.Stop();
				walkingAudio = baseWalkingAudio;
			}
		}

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3 || !body || body.isKinematic) 
			return;
		
		// Calculate push direction from move direction, 
		// we only push objects to the sides never up and down
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		//var pushDir : Vector3 = Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
		// If you know how fast your character is trying to move,
		// then you can also multiply the push velocity by that.

		// open the door faster if they're running
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			pushPower = 4.0f;
		} else {
			pushPower = 2.0f;
		}
		// Apply the push
		body.velocity = pushDir * pushPower;
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