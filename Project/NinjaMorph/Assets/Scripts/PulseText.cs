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

	// Variables for calculating delta time
	private float timeLastFrame = 0.0f;
	private float timeCurrentFrame = 0.0f;
	private float deltaTime = 0.0f;

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
		// Create our own delta time based on realtime since startup
		timeCurrentFrame = Time.realtimeSinceStartup;
		deltaTime = timeCurrentFrame - timeLastFrame;
		timeLastFrame = timeCurrentFrame;
		//print ("Current: " + timeCurrentFrame + ", Delta: " + deltaTime + ", Last: " + timeLastFrame);
		// Make sure we are selected
		if (selected)
		{
			if (increasing)
			{
				timePassed += deltaTime;
				if (timePassed >= pulseTime)
					increasing = false;
			}
			else
			{
				timePassed -= deltaTime;
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
			timePassed -= deltaTime;
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
