using UnityEngine;
using System.Collections;

public class FillableObject : InteractiveObject
{
	public bool filled = false;
	private float fillAmount = 0.0f;

	void Start() {
	
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
		fillAmount = 0.0f;
	}
}
