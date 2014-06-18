/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/

using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// Air Garden
		if (Input.GetKeyDown ("1"))
		{
			if (!"AirGarden".Equals(Application.loadedLevelName))
				Application.LoadLevel ("AirGarden");
		}
		// Fire Garden
		else if (Input.GetKeyDown ("2"))
		{
			if (!"FireGarden".Equals(Application.loadedLevelName))
				Application.LoadLevel("FireGarden");
		}
		// Water Garden
		else if (Input.GetKeyDown ("3"))
		{
			if (!"WaterGarden".Equals(Application.loadedLevelName))
				Application.LoadLevel("WaterGarden");
		}
	}
}
