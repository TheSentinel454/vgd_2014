using UnityEngine;
using System.Collections;

public class FillableObject : InteractiveObject
{
	public bool filled = false;
	private float fillAmount = 0.0f;
	private Vector3 originalPosition = new Vector3(0,0,0);
	private Vector3 originalScale = new Vector3(0,0,0);

	void Start() {
		originalPosition = transform.parent.FindChild("bucket_water").transform.position;
		originalScale = transform.parent.FindChild ("bucket_water").localScale;
	}

	public override void PlayerCollisionStay(InteractiveCollision col) {

		if (col.GetNinjaType() == NinjaType.Water && fillAmount <= 1.0) {
			transform.parent.FindChild("bucket_water").Translate(0,0.03f,0);
			transform.parent.FindChild("bucket_water").localScale += new Vector3(0.00007f, 0, 0.00007f);
			fillAmount += 0.03f;
		}
		if (fillAmount >= 1.0) {
			filled = true;
		}
	}

	public void clearBucket() {
		transform.parent.FindChild("bucket_water").transform.position = originalPosition;
		transform.parent.FindChild ("bucket_water").localScale = originalScale;
		fillAmount = 0.0f;
	}
}
