using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float damage = 5.0f;

	void OnCollisionEnter (Collision collision) {
//		Destroy (gameObject);
	}
}
