using UnityEngine;
using System.Collections;

public class CyborgNinjaAnimationController : MonoBehaviour {

	// STATE FLAGS
	public bool attacking = false;
	public bool grounded = false;
	public bool weapon = false;
	public bool dead = false;

	// ANIMATION FLAGS
	bool weaponOut = false;
	bool landingJump = false;
	bool previouslyGrounded = false;

	// ANIMATIONS
	Animation animation;

	// POSITIONS
	Vector3 previousPosition = Vector3.zero;
	Vector3 displacement = Vector3.zero;
	Vector3 direction = Vector3.zero;
	Vector3 velocity = Vector3.zero;

	// DISTANCES
	float distanceToGround = 0;

	// Initialize Animations and Positions
	void Start () {
		animation = GetComponent<Animation>();
		previousPosition = transform.position;
		distanceToGround = (collider.bounds.extents.y - collider.bounds.center.y) + 0.01f;
	}

	// Updating Animation States
	void FixedUpdate () {
		// Determine if we are currently grounded.
		previouslyGrounded = grounded;
		grounded = Physics.Raycast (new Ray (transform.position, Vector3.down), distanceToGround);

		// Update position information
		displacement = transform.position - previousPosition;
		direction = displacement.normalized;
		velocity = displacement / Time.deltaTime;
		previousPosition = transform.position;


		// DEATH ANIMATIONS
		// ----------------
		// deathBackwards
		// deathBackwardsSwordDrawn
		if (dead == true) {
			animation[weaponOut ? "deathBackwardsSwordDrawn" : "deathBackwards"].wrapMode = WrapMode.ClampForever;
			animation.CrossFade(weaponOut ? "deathBackwardsSwordDrawn" : "deathBackwards");
			return;
		}

		// IDLE ANIMATIONS
		// ---------------
		// idlestandbreathe
		// idleSword
		// getSword
		// putBackSword
		// 3HitComboSword
		if (grounded == true && landingJump == false && displacement.sqrMagnitude <= 0.0001f) {
			animation.CrossFade(weaponOut ? "idleSword" : "idlestandbreathe");

			if (weapon == true && weaponOut == false) {
				animation.CrossFade("getSword");
			}

			if (weapon == false && weaponOut == true) {
				animation.CrossFade("putBackSword");
			}

			if (attacking == true && weaponOut == true) {
				animation["3HitComboSword"].wrapMode = WrapMode.Once;
				animation.CrossFade("3HitComboSword");
			}

			if (attacking == true && weaponOut == false) {
				animation.CrossFade("getSword");
			}
		}

		// WALKING ANIMATIONS
		// ------------------
		// walkNormal
		if (grounded == true && 0.0001f < displacement.sqrMagnitude && velocity.sqrMagnitude <= 23) {
			animation.CrossFade("walkNormal");
			SetLandingJump(0);
		}

		// RUNNING ANIMATIONS
		// ------------------
		// runNoWeapon
		// runSword
		if (grounded == true && 23 < velocity.sqrMagnitude) {
			animation.CrossFade(weaponOut ? "runSword" : "runNoWeapon");
			SetLandingJump(0);
		}

		// JUMPING ANIMATIONS
		// ------------------
		// jumpNoWeapon_up
		// jumpNoWeapon_down
		// jumpNoWeapon_fall
		if (grounded == false && velocity.y > 0) {
			animation.CrossFade("jumpNoWeapon_up");
		}

		if (grounded == false && velocity.y < 0) {
			animation.CrossFade("jumpNoWeapon_down");
		}

		if (grounded == true && previouslyGrounded == false) {
			animation.CrossFade("jumpNoWeapon_fall");
		}
	}

	// Callback for when the user has completed an attack animation.
	void OnAttackComplete () {
		attacking = false;
	}

	// Sets the cyborg ninja's katana visibility
	void SetSwordVisibility (int visible) {
		Transform katana = transform.Find ("CYBORG_NINJA_/CYBORG_NINJA_ Pelvis/CYBORG_NINJA_ Spine/CYBORG_NINJA_ Spine1/CYBORG_NINJA_ Neck/CYBORG_NINJA_ R Clavicle/CYBORG_NINJA_ R UpperArm/CYBORG_NINJA_ R Forearm/CYBORG_NINJA_ R Hand/KATANA");
		Transform katanaSheathed = transform.Find ("CYBORG_NINJA_/CYBORG_NINJA_ Pelvis/CYBORG_NINJA_ Spine/CYBORG_NINJA_ Spine1/KATANA001");

		if (visible == 0) {
			katana.renderer.enabled = false;
			katanaSheathed.renderer.enabled = true;
			weaponOut = false;
		} else {
			katana.renderer.enabled = true;
			katanaSheathed.renderer.enabled = false;
			weaponOut = true;
		}
	}

	// Sets the landing jump flag
	void SetLandingJump (int landing) {
		landingJump = landing != 0;
	}
}
