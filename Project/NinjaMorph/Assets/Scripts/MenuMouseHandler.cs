using UnityEngine;
using System.Collections;

public class MenuMouseHandler : MonoBehaviour
{
	public float colorTransitionSpeed = 0.15f;
	public float rotateTransitionSpeed = 1.0f;
	
	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	void OnMouseDown()
	{
		print ("MouseTrigger: " + this.name);
		if (this.name == "PlayButton")
		{
			// Load the air level
			Application.LoadLevel("AirRoom");
		}
		else if (this.name == "HelpButton")
		{
			// Rotate towards the help page
			iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 90, 0), rotateTransitionSpeed);
		}
		else if (this.name == "BackButton")
		{
			// Rotate back towards the main menu
			iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 0, 0), rotateTransitionSpeed);
		}
	}
	
	/// <summary>
	/// Raises the mouse enter event.
	/// </summary>
	void OnMouseEnter()
	{
		// Transition to gray
		iTween.ColorTo (gameObject, Color.gray, colorTransitionSpeed);
	}

	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit()
	{
		// Transition back to white
		iTween.ColorTo (gameObject, Color.white, colorTransitionSpeed);
	}
}
