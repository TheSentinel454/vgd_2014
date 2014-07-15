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
			Application.LoadLevel("airLevel");
		}
		else if (this.name == "HelpButton")
		{
			iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 90, 0), rotateTransitionSpeed);
		}
		else if (this.name == "BackButton")
		{
			iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 0, 0), rotateTransitionSpeed);
		}
	}
	
	/// <summary>
	/// Raises the mouse over event.
	/// </summary>
	void OnMouseEnter()
	{
		iTween.ColorTo (gameObject, Color.gray, colorTransitionSpeed);
	}
	
	void OnMouseExit()
	{
		iTween.ColorTo (gameObject, Color.white, colorTransitionSpeed);
	}
}
