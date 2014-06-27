using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CyborgNinjaAnimationController))]
public class CyborgNinjaController : MonoBehaviour {

	// CONTROLS
	public KeyCode attackKey;
	public float attackKeyPressDelay = 0.15f;

	// CONTROL TIMERS
	float attackKeyPressedTimer;

	// ANIMATIONS
	CyborgNinjaAnimationController animationController;

	// Get the animation controller and initialize the attack key timer
	void Start () {
		animationController = GetComponent<CyborgNinjaAnimationController> ();
		attackKeyPressedTimer = attackKeyPressDelay;
	}
	
	// Determine what the ninja should be doing.
	void FixedUpdate () {

		// When the user has pressed the attack key, determine if he is quickly pressing the button
		// to draw his sword or actually attempting to attack an enemy.
		if (Input.GetKey (attackKey)) {
			if (attackKeyPressedTimer >= 0) {
				attackKeyPressedTimer -= Time.deltaTime;
			} else {
				animationController.attacking = true;
			}

		// If he releases the attack key before the before the attack delay is completed, we are just
		// going to toggle his sword
		} else if (attackKeyPressedTimer < attackKeyPressDelay) {
			if (attackKeyPressedTimer > 0) {
				animationController.weapon = !animationController.weapon;
			}
			attackKeyPressedTimer = attackKeyPressDelay;
		}
	}
}
