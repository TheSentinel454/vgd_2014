using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour
{
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
		// Don't bother if this isn't the player
		if (other.tag != "Player")
			return;
		// We have collided with the player
		else
		{
			Debug.Log ("Trap Triggered!");
			// Find the Trigger collider
			foreach (Collider col in GetComponents<BoxCollider>())
			{
				// Check for the trigger collider
				if (col.isTrigger)
				{
					// Destroy the trigger
					Destroy(col);
				}
			}
			HingeJoint joint = gameObject.AddComponent<HingeJoint>();
			joint.anchor = new Vector3(0.4f, -0.1f, 0.0f);
			joint.useLimits = true;
			JointLimits limits = new JointLimits();
			limits.min = 0;
			limits.max = 60;
			limits.minBounce = 0.25f;
			limits.maxBounce = 0.75f;
			joint.limits = limits;
			// Remove the kinematic
			gameObject.rigidbody.isKinematic = false;
			/*
			//collider.enabled = false;

			// Find the Bridges
			foreach (Transform trans in transform.parent.gameObject.GetComponentsInChildren<Transform>())
			{
				if (trans.name == "Bridge1")
				{
					HingeJoint joint = trans.gameObject.AddComponent<HingeJoint>();
					joint.anchor = new Vector3(0.0f, -11.9f, 0.0f);
					joint.useLimits = true;
					JointLimits limits = new JointLimits();//joint.limits;
					limits.min = -15;
					limits.max = 0;
					joint.limits = limits;
					// Remove the collider
					Destroy (trans.gameObject.collider);
					trans.gameObject.rigidbody.isKinematic = false;
				}
				else if (trans.name == "Bridge2")
				{
					HingeJoint joint = trans.gameObject.AddComponent<HingeJoint>();
					joint.anchor = new Vector3(0.0f, 11.9f, 0.0f);
				}
			}*/
		}
	}
}
