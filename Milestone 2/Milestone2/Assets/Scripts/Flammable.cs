/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/

using UnityEngine;
using System.Collections;

public class Flammable : MonoBehaviour {

	public GameObject fire;
	private GameObject Fire;
	public bool onFire = false;
	public bool permanent = false;

	// Check if we are currently on fire.
	void Update () {
		if (Fire == null && onFire && fire != null) {
			Fire = Instantiate(fire, transform.position, transform.rotation) as GameObject;
			Fire.transform.parent = transform;
		} 

		if (!permanent && Fire != null) {
//			Fire.gameObject.transform.Find("InnerCore").gameObject.particleEmitter.maxEnergy -= 0.001f;
//			Fire.gameObject.transform.Find("OuterCore").gameObject.particleEmitter.maxEnergy -= 0.001f;
//			Fire.gameObject.transform.Find("Smoke").gameObject.particleEmitter.maxEnergy -= 0.01f;
//			Fire.gameObject.transform.Find("Lightsource").gameObject.light.range -= 0.01f;
		}

		// Check if we need to remove the fire.
		if (Fire != null && !onFire && !permanent) {
			Destroy(Fire);
		}
	}

	// When we have collided with an object, check if it is on fire and catch on fire if it is.
	void OnTriggerEnter (Collider collider) {
		Flammable flammable = collider.gameObject.GetComponent ("Flammable") as Flammable;
		if (flammable != null && flammable.onFire) {
			onFire = true;
			if (Fire != null) {
//				Fire.gameObject.transform.Find("InnerCore").gameObject.particleEmitter.maxEnergy = 0.5f;
//				Fire.gameObject.transform.Find("OuterCore").gameObject.particleEmitter.maxEnergy = 1;
//				Fire.gameObject.transform.Find("Smoke").gameObject.particleEmitter.maxEnergy = 6;
//				Fire.gameObject.transform.Find("Lightsource").gameObject.light.range = 6;
			}
		}
	}
}
