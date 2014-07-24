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

public class MenuNinjaController : MonoBehaviour
{
	public NinjaSettings baseSettings;
	public NinjaSettings airSettings;
	public NinjaSettings fireSettings;
	public NinjaSettings waterSettings;

	private NinjaType ninjaType;
	private Renderer[] ninjaRenderers;
	private GameObject ninjaEffects;

	enum CharacterGround
	{
		Grass,
		Wood,
		Water
	}

	private bool attacking = false;
	private InputDevice inputDevice;

#if PLAY_TESTING
	public int numAttacks = 0;
	public int numKills = 0;
	public float totalAirTime = 0.0f;
	public float totalFireTime = 0.0f;
	public float totalWaterTime = 0.0f;
	public float totalAirCharging = 0.0f;
	public float totalFireCharging = 0.0f;
	public float totalWaterCharging = 0.0f;
	private float sumHealth = 0.0f;
	private float numHealthPoints = 0.0f;
	public float avgHealth(){ return (sumHealth / numHealthPoints); }
#endif

    private bool isControllable = true;

    void Awake()
    {
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
		// Set the base ninja
		setBaseNinja ();
    }

	void ApplyAttacking()
	{
		// Prevent attacking too fast after each other
		if (attacking)
			return;
		
		if (!IsAttacking() && inputDevice.RightTrigger.WasPressed)
		{
			DidAttack();
		}
	}

	public void DidAttack()
	{
#if PLAY_TESTING
		numAttacks++;
#endif
		attacking = true;
		StartCoroutine(Attack());
	}
	
	IEnumerator Attack()
	{
		yield return true;
		attacking = false;
	}

    void FixedUpdate()
	{
		//InputManager.Update();
		// Use last device which provided input.
		inputDevice = InputManager.ActiveDevice;
#if PLAY_TESTING
		sumHealth += zenEnergy;
		numHealthPoints += 1.0f;
#endif
		//updateEnergyValues ();
		handleNinjaChange ();

		/*
        if (!isControllable)
        {
            // kill all inputs if not controllable.
            Input.ResetInputAxes();
        }
		*/

		// Apply jumping logic
		ApplyAttacking ();
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
				// Set the air ninja
				setAirNinja();
			}
		}
		// Fire Ninja
		else if (inputDevice.Action2.WasPressed)//Input.GetKeyDown ("2"))
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
				// Set the fire ninja
				setFireNinja();
			}
		}
		// Water Ninja
		else if (inputDevice.Action3.WasPressed)//Input.GetKeyDown ("3"))
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
				// Set the water ninja
				setWaterNinja();
			}
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

	public bool IsAttacking()
	{
		return attacking;
	}

    public bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }

    public void Reset()
    {
        gameObject.tag = "Player";
    }
}