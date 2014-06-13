using UnityEngine;
using System.Collections;

public class Waterfall : MonoBehaviour {

	private float resetTime = 0f;

	void Start () {

	}

	void Update () {
		if (resetTime >= 0.5 || resetTime == 0) {
			audio.PlayOneShot (audio.clip);
			resetTime = 0;
		}
		resetTime += Time.deltaTime;
	}
}
