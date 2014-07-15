using UnityEngine;
using System.Collections;

public class MenuMouseHandler : MonoBehaviour
{
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
			iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 90, 0), 1.0f);
		}
		else if (this.name == "BackButton")
		{
			iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 0, 0), 1.0f);
		}
	}
}
