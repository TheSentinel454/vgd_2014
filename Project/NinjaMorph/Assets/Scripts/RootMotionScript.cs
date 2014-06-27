using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]
public class RootMotionScript : MonoBehaviour {
	void OnAnimatorMove () {
		Animator animator = GetComponent<Animator> ();

		Vector3 previousPosition = transform.position;
		Vector3 nextPosition = previousPosition;

		nextPosition.z += animator.GetFloat ("Speed") * Time.deltaTime;
		transform.position = nextPosition;
	}
}
