using UnityEngine;
using System.Collections;
using System.Linq;

public class Projectile : MonoBehaviour {

	public float damage = 5.0f;
	public string[] destroyOnImpactAgainst;

	void OnCollisionEnter (Collision collision) {
		if (destroyOnImpactAgainst.Contains (collision.transform.root.gameObject.tag) || 
		    destroyOnImpactAgainst.Contains (collision.gameObject.tag)) {
			Destroy (gameObject);
		}

		// Damage player
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<NinjaController>().Damage(damage);
		}
	}
}
