using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	private float creationTime_;
	public float lifeSpan = 20.0f;
	public bool destroyable = true;
	public float speed = 0.0f;
	public Vector3 direction = Vector3.zero;

	// Save the bullet's creation time
	void Start () {
		creationTime_ = Time.time;
	}

	// Update the bullet's position
	void Update () {
		transform.position += direction * speed;
		if (destroyable && Time.time - creationTime_ > lifeSpan) {
			Destroy (gameObject);
		}
	}

	// Destroy bullets when they run into something
	void OnTriggerEnter (Collider collider) {
		// Damage the player when they get hit
		if (collider.tag == "Player") {
			collider.GetComponent<NinjaController>().Damage(5.0f);
		}

		// Destroy the bullet
		if (destroyable) {
			Destroy (gameObject);
		}
	}
}
