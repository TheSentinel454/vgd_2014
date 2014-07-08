using UnityEngine;
using System.Collections;

public class FillableObject : InteractiveObject
{
	public bool filled = false;
	private Vector3 originalPosition = new Vector3(0,0,0);
	private float fillAmount = 0.0f;

	void Start() {
		originalPosition = transform.parent.FindChild("bucket_water").transform.position;
	}

	public override void PlayerCollisionStay(InteractiveCollision col) {

		if (col.GetNinjaType() == NinjaType.Water && fillAmount <= 1.0) {
			transform.parent.FindChild("bucket_water").Translate(0,0.03f,0);
			fillAmount += 0.03f;
		}
		if (fillAmount >= 1.0) {
			filled = true;
		}
	}

	public void clearBucket() {
		transform.parent.FindChild("bucket_water").transform.position = originalPosition;
		fillAmount = 0.0f;
	}
}
