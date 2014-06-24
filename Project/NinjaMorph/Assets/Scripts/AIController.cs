using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {
	// AGENT
	NavMeshAgent agent;

	// ANIMATION CONTROLLER
	private AnimationController animationController_;

	// WAY POINTS
	public Transform[] wayPoints;
	private int activeWayPointIndex_ = 0;

	// SEARCH AREA
	public Collider searchArea;

	// Initialize Agent and Animation Controller
	void Start () {
		agent = gameObject.GetComponent<NavMeshAgent>();
		animationController_ = gameObject.GetComponent<AnimationController> ();
	}

	// Move our agent to the currently active way point.
	void Update () {
		// When we are away from our destination, continue moving torwards our destination.
		if (Vector3.Distance (transform.position, wayPoints[activeWayPointIndex_].position) > 1.0f) {
			if (agent.destination != wayPoints[activeWayPointIndex_].position) {
				agent.SetDestination(wayPoints[activeWayPointIndex_].position);
			}
		
		// Otherwise, we need to switch to the next way point.
		} else {
			activeWayPointIndex_ = (activeWayPointIndex_ + 1) % wayPoints.Length;
		}
	}

	// When a rigidbody has entered the trigger area.
	void OnTriggerStay (Collider collider) {
		// Check if the rigidbody is the player
		if (collider.tag == "Player") { 
			animationController_.attacking = true;
			agent.SetDestination(collider.transform.position);
		}
	}

	// When a rigidbody has exited the trigger area
	void OnTriggerExit(Collider collider) {
		// Check if the rigidbody is the player
		if (collider.tag == "Player") { 
			animationController_.attacking = false;
		}
	}
}
