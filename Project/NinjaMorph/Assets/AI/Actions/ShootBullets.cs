using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class ShootBullets : RAINAction
{
	private GameObject backOfGun_;
	private GameObject bulletModel_;
	private GameObject bullet_;
	private float timeOfLastShot = 0.0f;

    public ShootBullets() {
        actionName = "ShootBullets";
    }

	// Start the AI and find the back of the gun and the bullet model
    public override void Start(AI ai) {
		backOfGun_ = GameObject.Find("Back Of Gun");
		bulletModel_ = GameObject.Find("Bullet");
        base.Start(ai);
    }

	// Create a new bullet and fire!
    public override ActionResult Execute(AI ai) {
		// Limit the number of shots per second
		if (Time.time - timeOfLastShot >= 0.35) {
			// Create the new bullet instance
			bullet_ = MonoBehaviour.Instantiate (bulletModel_) as GameObject;

			// Make the bullet destroyable
			bullet_.GetComponent<Bullet>().destroyable = true;
			bullet_.renderer.enabled = true;

			// Position the bullet within the barrel
			bullet_.transform.position = bulletModel_.transform.position;

			// FIRE!
			bullet_.GetComponent<Bullet>().direction = (bulletModel_.transform.position - backOfGun_.transform.position).normalized;

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