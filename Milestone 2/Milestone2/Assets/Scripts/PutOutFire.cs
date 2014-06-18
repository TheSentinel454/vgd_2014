/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/

using UnityEngine;
using System.Collections;

public class PutOutFire : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		Flammable flammable = collider.gameObject.GetComponent ("Flammable") as Flammable;
		if (flammable != null && flammable.onFire) {
			flammable.onFire = false;
		}
	}
}
