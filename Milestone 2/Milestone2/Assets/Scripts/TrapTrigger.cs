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
			// Disable this collider
			collider.enabled = false;

			// Find the Bridges
			foreach (Transform trans in transform.parent.gameObject.GetComponentsInChildren<Transform>())
			{
				if (trans.name == "Bridge1")
				{
					HingeJoint joint = trans.gameObject.AddComponent<HingeJoint>();
					joint.anchor = new Vector3(0.0f, -11.9f, 0.0f);
					joint.useLimits = true;
					JointLimits limits = joint.limits;
					limits.min = -15;
					limits.max = 0;
					// Remove the collider
					Destroy (trans.gameObject.collider);
					trans.gameObject.rigidbody.isKinematic = false;
				}
				else if (trans.name == "Bridge2")
				{
					HingeJoint joint = trans.gameObject.AddComponent<HingeJoint>();
					joint.anchor = new Vector3(0.0f, 11.9f, 0.0f);
				}
			}
		}
	}
}
