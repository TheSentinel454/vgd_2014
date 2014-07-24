using UnityEngine;
using System.Collections;

public class PulseText : MonoBehaviour
{
	//public GameObject[] menuItems;

	public float pulseTime = 0.25f;
	public int minPulseSize = 160;
	public int maxPulseSize = 200;
	public bool increasing = true;
	public bool selected = false;

	private float timePassed;
	private TextMesh textMesh;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		timePassed = 0.0f;
		increasing = false;
		// Get the Text Mesh associated with the game object
		textMesh = gameObject.GetComponentInChildren<TextMesh>();
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		// Make sure we are selected
		if (selected)
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
			// Make sure we have one to work with
			if (textMesh != null)
			{
				// Set the font size of the text mesh based interpolation
				textMesh.fontSize = (int)Mathf.Lerp((float)minPulseSize, (float)maxPulseSize, timePassed / pulseTime);
			}
		}
		// Not selected
		else
		{
			timePassed -= Time.deltaTime;
			if (timePassed <= 0.0f)
				timePassed = 0.0f;
			// Make sure we have one to work with
			if (textMesh != null)
			{
				// Set the font size of the text mesh based interpolation
				textMesh.fontSize = (int)Mathf.Lerp((float)minPulseSize, (float)maxPulseSize, timePassed / pulseTime);
			}
		}
	}
}
