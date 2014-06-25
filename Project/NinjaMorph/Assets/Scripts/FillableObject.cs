using UnityEngine;
using System.Collections;

public class FillableObject : InteractiveObject
{
	public bool filled = false;
	private float fillAmount = 0.0f;

	/// <summary>
	/// Handles the water ninja collision.
	/// </summary>
	protected override void HandleWaterNinjaCollision()
	{
		print ("override handle water ninja");
		// Check the filled flag
		if (!filled)
		{


		}
	}

	public override void PlayerCollisionStay(InteractiveCollision col) {
		print ("override player collision stay");
		if (col.GetNinjaType() == NinjaType.Water && fillAmount <= 1.0) {
			transform.parent.FindChild("bucket_water").Translate(0,0.03f,0);
			transform.parent.FindChild("bucket_water").localScale += new Vector3(0.00007f, 0, 0.00007f);
			fillAmount += 0.03f;

		}
	}
}
