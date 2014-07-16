using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class PredictiveAim : RAINAction {

	GameObject target;
	GameObject bullet;
	Vector3 targetOffset;
	Vector3 targetPosition;
	float muzzleVelocity;

	public PredictiveAim() {
		actionName = "PredictiveAim";
	}
	
    public override void Start(AI ai) {
		if (ai.WorkingMemory.GetItem("CyborgNinja").GetValue<GameObject>() != null) {
			target = ai.WorkingMemory.GetItem ("CyborgNinja").GetValue<GameObject>();
		} else {
			target = ai.WorkingMemory.GetItem ("CyborgNinjaNear").GetValue<GameObject>();
		}
		bullet = ai.WorkingMemory.GetItem ("Bullet").GetValue<GameObject>();
		muzzleVelocity = ai.Body.gameObject.GetComponent<CyborgTroopAttackController>().muzzleVelocity;
		base.Start(ai);
    }

    public override ActionResult Execute(AI ai) {
		// Calculate bullet offset to point torwards the target
		targetOffset = bullet.transform.position - ai.Body.transform.position;
		targetPosition = target.transform.position - targetOffset;

		// Now we should predictively calculate the target's position
		Vector3 targetVelocity = target.GetComponent<NinjaController> ().velocity;
		targetPosition = FirstOrderIntercept (bullet.transform.position, bullet.rigidbody.velocity, muzzleVelocity, targetPosition, targetVelocity);

		// Update the target location
		ai.WorkingMemory.SetItem<Vector3> ("CyborgNinjaFuture", targetPosition);

        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai) {
        base.Stop(ai);
    }

	// First-order intercept using absolute target position
	public static Vector3 FirstOrderIntercept
		(
			Vector3 shooterPosition,
			Vector3 shooterVelocity,
			float shotSpeed,
			Vector3 targetPosition,
			Vector3 targetVelocity
		)  {
		Vector3 targetRelativePosition = targetPosition - shooterPosition;
		Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
		float t = FirstOrderInterceptTime
			(
				shotSpeed,
				targetRelativePosition,
				targetRelativeVelocity
				);
		return targetPosition + t*(targetRelativeVelocity);
	}

	// First-order intercept using relative target position
	public static float FirstOrderInterceptTime
		(
			float shotSpeed,
			Vector3 targetRelativePosition,
			Vector3 targetRelativeVelocity
		) {
		float velocitySquared = targetRelativeVelocity.sqrMagnitude;
		if(velocitySquared < 0.001f)
			return 0f;
		
		float a = velocitySquared - shotSpeed*shotSpeed;
		
		//handle similar velocities
		if (Mathf.Abs(a) < 0.001f)
		{
			float t = -targetRelativePosition.sqrMagnitude/
				(
					2f*Vector3.Dot
					(
					targetRelativeVelocity,
					targetRelativePosition
					)
					);
			return Mathf.Max(t, 0f); //don't shoot back in time
		}
		
		float b = 2f*Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
		float c = targetRelativePosition.sqrMagnitude;
		float determinant = b*b - 4f*a*c;
		
		if (determinant > 0f) { //determinant > 0; two intercept paths (most common)
			float	t1 = (-b + Mathf.Sqrt(determinant))/(2f*a),
			t2 = (-b - Mathf.Sqrt(determinant))/(2f*a);
			if (t1 > 0f) {
				if (t2 > 0f)
					return Mathf.Min(t1, t2); //both are positive
				else
					return t1; //only t1 is positive
			} else
				return Mathf.Max(t2, 0f); //don't shoot back in time
		} else if (determinant < 0f) //determinant < 0; no intercept path
			return 0f;
		else //determinant = 0; one intercept path, pretty much never happens
			return Mathf.Max(-b/(2f*a), 0f); //don't shoot back in time
	}
}