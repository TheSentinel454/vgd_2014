using UnityEngine;
using System.Collections;
using RAIN.Core;

public class CyborgTroopDeath : MonoBehaviour {

	void ObjectDeath () {
		transform.Find ("AI").GetComponent<AIRig> ().AI.WorkingMemory.SetItem<bool> ("Dead", true);
	}
}
