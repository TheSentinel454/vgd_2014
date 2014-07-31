/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class SelectMenu : MonoBehaviour
{
	public GameObject[] menuItems;

	public float pulseTime;
	public int minPulseSize;
	public int maxPulseSize;
	public bool increasing;

	public int currentItem;
	private float timePassed;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		currentItem = 0;
		timePassed = 0.0f;
		increasing = false;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		if (increasing)
		{
			timePassed += Time.deltaTime;
			if (timePassed >= pulseTime)
				increasing = false;
		}
		else
		{
			timePassed -= Time.deltaTime;
			if (timePassed <= 0.0f)
				increasing = true;
		}
		// Get teh current game object
		GameObject go = menuItems [currentItem];
		// Get the Text Mesh associated with that game object
		TextMesh tm = go.GetComponentInChildren<TextMesh>();
		// Make sure we have one to work with
		if (tm != null)
		{
			// Set the font size of the text mesh based interpolation
			tm.fontSize = (int)Mathf.Lerp((float)minPulseSize, (float)maxPulseSize, timePassed / pulseTime);
		}
	}
}
