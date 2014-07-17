using UnityEngine;
//using UnityEditor;
using System.Collections;

public class CyborgNinjaAnimationController : MonoBehaviour {

	// STATE FLAGS
	public bool attacking = false;
	public bool grounded = true;
	public bool weapon = false;
	public bool dead = false;

	// ANIMATION FLAGS
	bool weaponOut = false;
	bool landingJump = false;
	bool previouslyGrounded = false;

	// ANIMATIONS
	Animation cyborgAnimation;

	// POSITIONS
	Vector3 previousPosition = Vector3.zero;
	Vector3 displacement = Vector3.zero;
	Vector3 velocity = Vector3.zero;

	// DISTANCES
	float distanceToGround = 0;
	float colliderRadius = 0.5f;

	// Hit
	RaycastHit hit;

	// Initialize Animations and Positions
	void Start () {
		cyborgAnimation = GetComponent<Animation>();
		previousPosition = transform.position;
		distanceToGround = collider.bounds.center.y - collider.bounds.min.y;
		colliderRadius = GetComponent<CharacterController> ().radius;
	}

	// Updating Animation States
	void FixedUpdate () {
		// Determine if we are currently grounded.
		previouslyGrounded = grounded;
		grounded = Physics.CapsuleCast (collider.bounds.center, collider.bounds.center, colliderRadius, Vector3.down, out hit, distanceToGround + 0.1f);

		// Update position information
		displacement = transform.position - previousPosition;
		velocity = displacement / Time.deltaTime;
		previousPosition = transform.position;


		// DEATH ANIMATIONS
		// ----------------
		// deathBackwards
		// deathBackwardsSwordDrawn
		if (dead == true) {
			cyborgAnimation[weaponOut ? "deathBackwardsSwordDrawn" : "deathBackwards"].wrapMode = WrapMode.ClampForever;
			cyborgAnimation.CrossFade(weaponOut ? "deathBackwardsSwordDrawn" : "deathBackwards");
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
			cyborgAnimation.CrossFade(weaponOut ? "idleSword" : "idlestandbreathe");

			if (weapon == true && weaponOut == false) {
				cyborgAnimation.CrossFade("getSword");
			}

			if (attacking == false && weapon == false && weaponOut == true) {
				cyborgAnimation.CrossFade("putBackSword");
			}
		}

		// ATTACK ANIMATIONS
		// -----------------
		if (attacking == true && weaponOut == false) {
			cyborgAnimation["getSword"].layer = 1;
			cyborgAnimation["getSword"].weight = 1;
			cyborgAnimation.CrossFade("getSword");
		}

		if (attacking == true && weaponOut == true) {
			cyborgAnimation["3HitComboSword"].layer = 1;
			cyborgAnimation["3HitComboSword"].weight = 1;
			cyborgAnimation["3HitComboSword"].wrapMode = WrapMode.Once;
			cyborgAnimation.CrossFade("3HitComboSword");
		}

		// WALKING ANIMATIONS
		// ------------------
		// walkNormal
		if (grounded == true && 0.0001f < displacement.sqrMagnitude && velocity.sqrMagnitude <= 23) {
			cyborgAnimation.CrossFade("walkNormal");
			OnAttackComplete();
			SetLandingJump(0);
		}

		// RUNNING ANIMATIONS
		// ------------------
		// runNoWeapon
		// runSword
		if (grounded == true && 23 < velocity.sqrMagnitude) {
			cyborgAnimation.CrossFade(weaponOut ? "runSword" : "runNoWeapon");
			OnAttackComplete();
			SetLandingJump(0);
		}

		// JUMPING ANIMATIONS
		// ------------------
		// jumpNoWeapon_up
		// jumpNoWeapon_down
		// jumpNoWeapon_fall
		if (grounded == false && velocity.y > 0) {
			cyborgAnimation.CrossFade("jumpNoWeapon_up");
			OnAttackComplete();
		}

		if (grounded == false && velocity.y < 0) {
			cyborgAnimation.CrossFade("jumpNoWeapon_down");
		}

		if (grounded == true && previouslyGrounded == false) {
			cyborgAnimation.CrossFade("jumpNoWeapon_fall");
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
