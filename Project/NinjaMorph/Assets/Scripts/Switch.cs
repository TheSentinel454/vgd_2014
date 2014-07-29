/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	private float timer;
	private bool timerStarted;
	private bool disabled;

	// Use this for initialization
	void Start () {
		timer = 0.0f;
		timerStarted = false;
		disabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(timerStarted && !disabled) {
			timer += Time.deltaTime;
			if(timer <= 0.5f) {
				transform.Translate(new Vector3(0,-0.005f,0));
				GameObject.Find ("Platform 5").transform.Translate(new Vector3(0.188f,0,0));
			} else {
				disabled = true;
			}
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if(!timerStarted) {
				audio.Play ();
				GameObject.Find ("Brick Sound").audio.Play ();
			}
			timerStarted = true;
		}
	}
}
