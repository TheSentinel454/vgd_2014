using UnityEngine;
using System.Collections;

public class FireDampen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Ninja-Water") {
			particleEmitter.minEmission -= 1;
			particleEmitter.maxEmission -= 1;
		}
	}
}
