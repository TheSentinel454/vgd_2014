using UnityEngine;
using System.Collections;

public class FillableObject : InteractiveObject
{
	public bool filled = false;

	/// <summary>
	/// Handles the water ninja collision.
	/// </summary>
	protected override void HandleWaterNinjaCollision()
	{
		// Check the filled flag
		if (!filled)
		{
			// Fill the object

		}
	}
}
