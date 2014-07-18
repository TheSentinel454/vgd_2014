/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class CyborgTroopAttackController : MonoBehaviour {
	
	public GameObject gun;
	public GameObject bullet;
	public AudioSource gunshotSound;
	public float muzzleVelocity = 0.002f;
	public float destroyAfter = 4.0f;
	private float lastFiredTime = 0.0f;

	// Fires a bullet
	void FireBullet () {
		if (Time.time - lastFiredTime < 0.1) return;
		GameObject loadedBullet = Instantiate (bullet) as GameObject;
		Physics.IgnoreCollision (loadedBullet.collider, bullet.collider);
		loadedBullet.renderer.enabled = true;
		loadedBullet.transform.position = bullet.transform.position;
		loadedBullet.transform.up = gun.transform.forward;
		loadedBullet.rigidbody.isKinematic = false;
		loadedBullet.rigidbody.AddRelativeForce (new Vector3 (0, muzzleVelocity, 0), ForceMode.Impulse);
		lastFiredTime = Time.time;
		Destroy (loadedBullet, destroyAfter);
		gunshotSound.Play ();
	}
}
