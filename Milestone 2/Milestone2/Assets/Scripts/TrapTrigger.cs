using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour
{
	public bool destroyTrigger;
	public Vector3 hingeAnchor;
	public float hingeMinLimit;
	public float hingeMaxLimit;
	public float hingeMinBounce;
	public float hingeMaxBounce;

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
			if (destroyTrigger)
			{
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
			}
			if (!destroyTrigger)
			{
				// Make sure we don't add multiple hinges
				if (gameObject.GetComponent<HingeJoint>() != null)
					return;
			}
			HingeJoint joint = gameObject.AddComponent<HingeJoint>();
			joint.anchor = hingeAnchor;
			joint.useLimits = true;
			JointLimits limits = new JointLimits();
			limits.min = hingeMinLimit;
			limits.max = hingeMaxLimit;
			limits.minBounce = hingeMinBounce;
			limits.maxBounce = hingeMaxBounce;
			joint.limits = limits;
			// Remove the kinematic
			gameObject.rigidbody.isKinematic = false;
		}
	}
}
