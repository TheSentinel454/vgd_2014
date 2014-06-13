using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Transform player;
	private Vector3 lastPos;
	private Vector3 playerVel;
	private float timer;
	private bool hit = false; //has door been pushed

	// Use this for initialization
	void Start () {
		lastPos = player.position; // initialize lastPos
	}
	
	// Update is called once per frame
	void Update () {
		// calculate displacement since last Update:
		Vector3 moved = player.position - lastPos;
		// update lastPos:
		lastPos = player.position;
		// calculate the velocity:
		playerVel = moved / Time.deltaTime;


	}

	void OnTriggerEnter(Collider other) {

		if(!hit) timer = Time.time;
		//once the door has been opened, the player can't apply a force anymore
		if (other.tag == "Player" && Time.time - timer < 1) {
			print ("force");
			float forceAmount = 100f;
			Rigidbody body = this.rigidbody;
			print (forceAmount * playerVel.magnitude);
			body.AddForce(body.transform.right * forceAmount * playerVel.magnitude, ForceMode.Acceleration);
			body.useGravity = true;
			hit = true;
		}	
	}
}
