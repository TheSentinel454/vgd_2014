using UnityEngine;
using System.Collections;

public class OnCollision : MonoBehaviour
{
	void PlayerCollision(ControllerColliderHit hit)
	{
		Debug.Log ("PlayerCollision: " + name);
	}
}