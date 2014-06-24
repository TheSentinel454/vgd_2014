using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

	// STATE FLAGS
	public bool idle = true;
	public bool moving = false;
	public bool rolling = false;
	public bool attacking = false;
	public bool dead = false;

	// ANIMATIONS
	private Animation animation_;
	public AnimationClip idleAnimation;
	public AnimationClip standingAttackAnimation;
	public AnimationClip rapidStandingAttackAnimation;
	public AnimationClip moveAnimation;
	public AnimationClip movingAttackAnimation;
	public AnimationClip rapidMovingAttackAnimation;
	public AnimationClip rollAnimation;
	public AnimationClip damageAnimation;
	public AnimationClip deathAnimation;

	// ANIMATION SPEEDS
	private float movementAnimationSpeed_ = 1.0f;

	// POSITIONS
	private Vector3 previousPosition_;
	private Vector3 displacement_;
	private Vector3 velocity_;

	// Initialize Animations
	void Start () {
		animation_ = gameObject.GetComponent<Animation>();
		previousPosition_ = gameObject.transform.position;
	}

	// Updating Animation States
	void Update () {
		// Calculate the object's displacement.
		displacement_ = gameObject.transform.position - previousPosition_;
		previousPosition_ = gameObject.transform.position;

		// Determine the character movement speed
		velocity_ = displacement_ / Time.deltaTime;
		movementAnimationSpeed_ = velocity_.magnitude / 3.0f;

		// Determine if the character is moving.
		moving = displacement_ != Vector3.zero;
		idle = moving == false;

		// DEAD
		if (dead) {
			animation_[deathAnimation.name].wrapMode = WrapMode.ClampForever;
			animation_.CrossFade(deathAnimation.name);

		// IDLE
		} else if (idle) { 
			animation_[idleAnimation.name].wrapMode = WrapMode.ClampForever;
			animation_.CrossFade(idleAnimation.name);

			// STANDING ATTACKING
			if (attacking) {
				animation_[standingAttackAnimation.name].speed = 1.0f;
				animation_[standingAttackAnimation.name].wrapMode = WrapMode.Loop;
				animation_.CrossFade(standingAttackAnimation.name);
			}

		// MOVING
		} else if (moving) {
			animation_[moveAnimation.name].speed = movementAnimationSpeed_;
			animation_[moveAnimation.name].wrapMode = WrapMode.Loop;
			animation_.CrossFade(moveAnimation.name);

			// MOVE ATTACKING
			if (attacking) {
				animation_[movingAttackAnimation.name].speed = movementAnimationSpeed_;
				animation_[movingAttackAnimation.name].wrapMode = WrapMode.Loop;
				animation_.CrossFade(movingAttackAnimation.name);
			}
		}
	}
}
