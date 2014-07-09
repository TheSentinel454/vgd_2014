using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class ShootBullets : RAINAction {

	// Player and Bullet Game Objects
	private GameObject player;
	private GameObject bulletModel;
	private GameObject bullet;

	// Player Velocity
	private Vector3 playerPreviousPosition;
	private Vector3 playerDisplacement = Vector3.zero;
	private Vector3 playerVelocity = Vector3.zero;
	private Vector3 aimingAt;

	// Timers
	private float timeOfLastShot = 0.0f;

	// Constructor
    public ShootBullets() {
        actionName = "ShootBullets";
    }

	// Start the AI and find the back of the gun and the bullet model
    public override void Start(AI ai) {
		bulletModel = ai.Body.GetComponentInChildren<Bullet> ().gameObject;
		player = ai.WorkingMemory.GetItem<GameObject> ("ninja").transform.Find ("Entity").gameObject;
		playerPreviousPosition = player.transform.position;
        base.Start(ai);
    }

	// Create a new bullet and fire!
    public override ActionResult Execute(AI ai) {
		// Limit the number of shots per second
		if (Time.time - timeOfLastShot >= 0.35) {
			// Create the new bullet instance
			bullet = MonoBehaviour.Instantiate (bulletModel) as GameObject;
			bullet.GetComponent<Bullet>().destroyable = true;
			bullet.renderer.enabled = true;

			// Determine where we are aiming
			playerVelocity = ai.WorkingMemory.GetItem<GameObject> ("ninja").GetComponent<NinjaController>().velocity;

			// Aim at the player
			float scalar = playerVelocity.magnitude * 0.5f;
			aimingAt = (player.transform.position + player.transform.forward * scalar) - bulletModel.transform.position;

			// Position the bullet within the barrel and shoot
			bullet.transform.up = aimingAt.normalized;
			bullet.transform.position = bulletModel.transform.position;
			bullet.GetComponent<Bullet>().direction = aimingAt.normalized;

			// Update our time of last shot
			timeOfLastShot = Time.time;
		}

        return ActionResult.SUCCESS;
    }

	// Stop the AI
    public override void Stop(AI ai) {
        base.Stop(ai);
    }
}