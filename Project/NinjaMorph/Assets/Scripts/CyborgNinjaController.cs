using UnityEngine;
using System.Collections;

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
		animationController = GetComponent<CyborgNinjaAnimationController> ();
		ninjaController = GetComponent<NinjaController> ();
	}
	
	// Determine what the ninja should be doing.
	void FixedUpdate ()
	{
		// ANIMATION TOGGLES
		animationController.attacking = animationController.attacking || Input.GetKey (attackKeyCode);
		animationController.weapon ^= Input.GetKeyDown (toggleWeaponKeyCode);
		animationController.dead = ninjaController.getZen() <= 0;
	}
}
