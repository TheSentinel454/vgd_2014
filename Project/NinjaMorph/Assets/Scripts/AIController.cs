using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {
	// AGENT
	NavMeshAgent agent;

	// ANIMATION CONTROLLER
	private AnimationController animationController_;

	// AI FLAGS
	public bool chasing = false;
	public bool patrolling = true;

	// RANGES
	public float maxChaseRange = 10.0f;
	public float weaponRange = 5.0f;

	// WAY POINTS
	public Transform[] wayPoints;
	private int activeWayPointIndex_ = 0;

	// PLAYER
	private GameObject player_;

	// Initialize Agent and Animation Controller
	void Start () {
		agent = gameObject.GetComponent<NavMeshAgent>();
		animationController_ = gameObject.GetComponent<AnimationController> ();
		player_ = GameObject.FindGameObjectWithTag ("Player");
	}

	// Move our agent to the currently active way point.
	void Update () {
		patrolling = chasing == false;

		// When we are chasing
		if (chasing) {
			// If we are chasing and the player runs out of our max chase range, go back to patrolling 
			if (Vector3.Distance (transform.position, player_.transform.position) > maxChaseRange) {
				chasing = false;

			// Chase the player when we are not within weapon range.
			} else if (Vector3.Distance (transform.position, player_.transform.position) > weaponRange) {
				animationController_.attacking = false;
				agent.SetDestination(player_.transform.position);

			// Otherwise, we are within attacking distance.
			} else {
				// Ensure that we are always facing the player
				transform.rotation = Quaternion.Slerp(
					transform.rotation, 
					Quaternion.LookRotation((player_.transform.position - transform.position).normalized), 
					Time.deltaTime * agent.angularSpeed
				);

				animationController_.attacking = true;
				if (agent.destination != transform.position) {
					agent.SetDestination(transform.position);
				}
			}

		// When we are patrolling
		} else if (patrolling) {
			animationController_.attacking = false;

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
	}

	// When a rigidbody has entered the trigger area.
	void OnTriggerEnter (Collider collider) {
		if (collider.name == player_.name) {
			chasing = true;
		}
	}
}
