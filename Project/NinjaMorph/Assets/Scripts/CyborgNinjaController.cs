using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]
public class CyborgNinjaController : MonoBehaviour {
	// Cyborg Ninja Animator
	Animator animator;

	// Animator State Flags


	void Start () {
		animator = GetComponent<Animator> ();
	}


	void Update () {
		float move = Input.GetAxis ("Vertical");
		animator.SetFloat ("Speed", move);
	}
	
	// Animator Root Motion Callback
	void OnAnimatorMove () {
		// Get the user's current speed
		float speed = animator.GetFloat ("Speed");
		
		transform.position += transform.forward * (speed * Time.fixedDeltaTime);
	}
}
