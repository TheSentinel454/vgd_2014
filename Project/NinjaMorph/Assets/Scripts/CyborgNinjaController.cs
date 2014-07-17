using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent (typeof(CyborgNinjaAnimationController))]
[RequireComponent (typeof(NinjaController))]
public class CyborgNinjaController : MonoBehaviour {

	// ANIMATIONS
	CyborgNinjaAnimationController animationController;
	NinjaController ninjaController;

	// CONTROLS
	public KeyCode attackKeyCode = KeyCode.Slash;
	public KeyCode toggleWeaponKeyCode = KeyCode.Period;

	// Get the animation controller
	void Start ()
	{
		InputManager.Setup ();
		animationController = GetComponent<CyborgNinjaAnimationController> ();
		ninjaController = GetComponent<NinjaController> ();
	}
	
	// Determine what the ninja should be doing.
	void FixedUpdate ()
	{
		//InputManager.Update ();
		// ANIMATION TOGGLES
		animationController.attacking = animationController.attacking || InputManager.ActiveDevice.RightTrigger.WasPressed;//Input.GetKey (attackKeyCode);
		animationController.weapon ^= Input.GetKeyDown (toggleWeaponKeyCode);
		animationController.dead = ninjaController.getZen() <= 0;
	}
}
