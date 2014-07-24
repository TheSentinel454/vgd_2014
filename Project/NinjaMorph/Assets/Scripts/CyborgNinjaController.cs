/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent (typeof(CyborgNinjaAnimationController))]
public class CyborgNinjaController : MonoBehaviour {

	// ANIMATIONS
	CyborgNinjaAnimationController animationController;
	NinjaController ninjaController;

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
		// ANIMATION TOGGLES
		animationController.attacking = animationController.attacking || 
			InputManager.ActiveDevice.RightTrigger.WasPressed || 
			InputManager.ActiveDevice.RightTrigger.IsPressed ||
			InputManager.ActiveDevice.RightTrigger.WasReleased;
		animationController.weapon ^= InputManager.ActiveDevice.RightBumper.WasPressed;
		animationController.dead = (ninjaController != null ? ninjaController.getZen() <= 0 : false);
	}
}
