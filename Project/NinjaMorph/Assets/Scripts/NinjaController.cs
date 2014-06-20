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
}

public class NinjaController : MonoBehaviour
{
	private Animation _animation;
    public AnimationClip idleAnimation;
    public AnimationClip walkAnimation;
    public AnimationClip runAnimation;
    public AnimationClip jumpPoseAnimation;

	public NinjaSettings baseSettings;
	public NinjaSettings airSettings;
	public NinjaSettings fireSettings;
	public NinjaSettings waterSettings;

	public AudioSource baseWalkingAudio;
	public AudioSource woodWalkingAudio;
	public AudioSource waterWalkingAudio;
	public AudioSource grassWalkingAudio;

	private AudioSource walkingAudio;
	private Type ninjaType;
	private Renderer ninjaRenderer;

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

	public enum Type
	{
		Base,
		Air,
		Water,
		Fire
	}

	private CharacterState _characterState;

	// Animation speeds
	public float walkMaxAnimationSpeed;
	public float runMaxAnimationSpeed;
	public float jumpAnimationSpeed;
	public float landAnimationSpeed;
	
	// The speed when walking
    public float walkSpeed = 2.0f;
    // when pressing "Fire3" button (cmd) we start running
	public float runSpeed = 6.0f;
	// How high do we jump when pressing jump and letting go immediately
	public float jumpHeight = 0.5f;
	// The gravity for the character
	public float gravity = 20.0f;
	// The gravity in controlled descent mode
	public float speedSmoothing = 10.0f;
	public float rotateSpeed = 500.0f;
	
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
		ninjaRenderer = GetComponentInChildren<Renderer> ();
		setBaseNinja ();
		walkingAudio = grassWalkingAudio;
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
            Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
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
                SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
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
                SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
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

    public void DidJump()
    {
        jumping = true;
        jumpingReachedApex = false;
        lastJumpTime = Time.time;
        lastJumpButtonTime = -10;

		_characterState = CharacterState.Jumping;
		Debug.Log("Did Jump!");
    }

	public void DidJumpReachApex()
	{
		Debug.Log("Did Reach Apex!");
	}

	public void DidLand()
	{
		Debug.Log("Did Land!");
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

	void handleNinjaChange()
	{
		// Air Ninja
		if (Input.GetKeyDown ("1"))
		{
			// See if we are currently the air ninja
			if (ninjaType == Type.Air)
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
					// Show some kind of message?

				}
			}
		}
		// Fire Ninja
		else if (Input.GetKeyDown ("2"))
		{
			// See if we are currently the fire ninja
			if (ninjaType == Type.Fire)
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
					// Show some kind of message?
					
				}
			}
		}
		// Water Ninja
		else if (Input.GetKeyDown ("3"))
		{
			// See if we are currently the water ninja
			if (ninjaType == Type.Water)
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
					// Show some kind of message?
					
				}
			}
		}
		// Check for expiring air/water/fire ninja
		if ((airEnergy <= 0.0f && ninjaType == Type.Air) ||
		    (waterEnergy <= 0.0f && ninjaType == Type.Water) ||
		    (fireEnergy <= 0.0f && ninjaType == Type.Fire))
		{
			// Go back to the base ninja
			setBaseNinja();
		}
	}

	/// <summary>
	/// Updates the energy values.
	/// </summary>
	void updateEnergyValues()
	{
		zenEnergy = 100.0f;

		if (ninjaType == Type.Air)
		{
			airEnergy -= 0.1f;
			if (airEnergy < 0.0f)
				airEnergy = 0.0f;
		}
		else if (ninjaType == Type.Fire)
		{
			fireEnergy -= 0.1f;
			if (fireEnergy < 0.0f)
				fireEnergy = 0.0f;
		}
		else if (ninjaType == Type.Water)
		{
			waterEnergy -= 0.1f;
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
		ninjaType = Type.Base;
		// Set the texture
		ninjaRenderer.material.mainTexture = baseSettings.texture;
		ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		// Set the animation speeds
		walkMaxAnimationSpeed = baseSettings.walkMaxAnimationSpeed;
		runMaxAnimationSpeed = baseSettings.runMaxAnimationSpeed;
		jumpAnimationSpeed = baseSettings.jumpAnimationSpeed;
		landAnimationSpeed = baseSettings.landAnimationSpeed;
		// Set the walk/run/jump values
		walkSpeed = baseSettings.walkSpeed;
		runSpeed = baseSettings.runSpeed;
		jumpHeight = baseSettings.jumpHeight;
		// Set gravity/speed smoothing/rotate speed
		gravity = baseSettings.gravity;
		speedSmoothing = baseSettings.speedSmoothing;
		rotateSpeed = baseSettings.rotateSpeed;

	}
	/// <summary>
	/// Sets the air ninja.
	/// </summary>
	void setAirNinja()
	{
		// Set the air ninja type
		ninjaType = Type.Air;
		// Set the texture
		ninjaRenderer.material.mainTexture = airSettings.texture;
		ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
		// Set the animation speeds
		walkMaxAnimationSpeed = airSettings.walkMaxAnimationSpeed;
		runMaxAnimationSpeed = airSettings.runMaxAnimationSpeed;
		jumpAnimationSpeed = airSettings.jumpAnimationSpeed;
		landAnimationSpeed = airSettings.landAnimationSpeed;
		// Set the walk/run/jump values
		walkSpeed = airSettings.walkSpeed;
		runSpeed = airSettings.runSpeed;
		jumpHeight = airSettings.jumpHeight;
		// Set gravity/speed smoothing/rotate speed
		gravity = airSettings.gravity;
		speedSmoothing = airSettings.speedSmoothing;
		rotateSpeed = airSettings.rotateSpeed;

	}
	/// <summary>
	/// Sets the fire ninja.
	/// </summary>
	void setFireNinja()
	{
		// Set the fire ninja type
		ninjaType = Type.Fire;
		// Set the texture
		ninjaRenderer.material.mainTexture = fireSettings.texture;
		ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		// Set the animation speeds
		walkMaxAnimationSpeed = fireSettings.walkMaxAnimationSpeed;
		runMaxAnimationSpeed = fireSettings.runMaxAnimationSpeed;
		jumpAnimationSpeed = fireSettings.jumpAnimationSpeed;
		landAnimationSpeed = fireSettings.landAnimationSpeed;
		// Set the walk/run/jump values
		walkSpeed = fireSettings.walkSpeed;
		runSpeed = fireSettings.runSpeed;
		jumpHeight = fireSettings.jumpHeight;
		// Set gravity/speed smoothing/rotate speed
		gravity = fireSettings.gravity;
		speedSmoothing = fireSettings.speedSmoothing;
		rotateSpeed = fireSettings.rotateSpeed;

	}
	/// <summary>
	/// Sets the water ninja.
	/// </summary>
	void setWaterNinja()
	{
		// Set the water ninja type
		ninjaType = Type.Water;
		// Set the texture
		ninjaRenderer.material.mainTexture = waterSettings.texture;
		ninjaRenderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		// Set the animation speeds
		walkMaxAnimationSpeed = waterSettings.walkMaxAnimationSpeed;
		runMaxAnimationSpeed = waterSettings.runMaxAnimationSpeed;
		jumpAnimationSpeed = waterSettings.jumpAnimationSpeed;
		landAnimationSpeed = waterSettings.landAnimationSpeed;
		// Set the walk/run/jump values
		walkSpeed = waterSettings.walkSpeed;
		runSpeed = waterSettings.runSpeed;
		jumpHeight = waterSettings.jumpHeight;
		// Set gravity/speed smoothing/rotate speed
		gravity = waterSettings.gravity;
		speedSmoothing = waterSettings.speedSmoothing;
		rotateSpeed = waterSettings.rotateSpeed;
	}

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
	}

	void OnTriggerExit(Collider collider)
	{
		if (walkingAudio != grassWalkingAudio)
		{
			walkingAudio.Stop ();
		}
		walkingAudio = grassWalkingAudio;
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
		}

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3 || !body || body.isKinematic) 
			return;

		// Check to see if the collided game object has an InteractiveObject
		InteractiveObject interactive = hit.gameObject.GetComponent<InteractiveObject>();
		if (interactive != null)
		{
			// Broadcast to the game object that there was a player collision
			hit.gameObject.BroadcastMessage("PlayerCollision", hit);
		}
		
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