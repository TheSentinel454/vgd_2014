/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class CyborgAnimationController : MonoBehaviour {

	public Animator animator;

	private Vector3 previousPosition;
	private Vector3 velocity = Vector3.zero;
	private Vector3 hVelocity = Vector3.zero;
	private Vector3 vVelocity = Vector3.zero;

	void Start () {
		previousPosition = transform.position;
	}

	void FixedUpdate () {
		velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
		previousPosition = transform.position;

//		animator.SetFloat("speed", Mathf.Min(new Vector2(velocity.x, velocity.z).magnitude, 0.51f));
	}
}
