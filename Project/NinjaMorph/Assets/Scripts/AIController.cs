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
	public float maxChaseRange = 12.0f;
	public float minChaseRange = 4.0f;
	public float weaponRange = 8.0f;

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
		// When we are chasing
		if (chasing) {
			// Get the distance from the player.
			float playerDistance = Vector3.Distance (transform.position, player_.transform.position);

			// If we are chasing and the player runs out of our max chase range, go back to patrolling 
			if (playerDistance > maxChaseRange) {
				chasing = false;
				patrolling = true;

			// Chase the player when we are not within weapon range.
			} else if (playerDistance >= weaponRange) {
				// Turn off attacking animation
				animationController_.attacking = false;

				// Chase the player.
				agent.SetDestination(player_.transform.position);

			// When we are within attacking distance
			} else if (playerDistance >= minChaseRange) {
				// Turn on attacking animation
				animationController_.attacking = true;
				
				// Chase the player.
				if (playerDistance >= minChaseRange + 1) {
					agent.SetDestination(player_.transform.position);
				
				// Otherwise, face towards the player.
				} else {
					// Ensure that we are always facing the player
					transform.rotation = Quaternion.Slerp(
						transform.rotation, 
						Quaternion.LookRotation((player_.transform.position - transform.position).normalized), 
						Time.deltaTime * agent.angularSpeed
					);
				}

			// When we are within the minimum chase range.
			} else {
				// Ensure that we are always facing the player
				transform.rotation = Quaternion.Slerp(
					transform.rotation, 
					Quaternion.LookRotation((player_.transform.position - transform.position).normalized), 
					Time.deltaTime * agent.angularSpeed
				);

				// Prevent the AI from getting too close to the player.
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
	void OnTriggerStay (Collider collider) {
		if (collider.name == player_.name) {
			chasing = true;
		}
	}
}
