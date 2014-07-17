using UnityEngine;
using System.Collections;
using InControl;

public class MenuMouseHandler : MonoBehaviour
{
	public float colorTransitionSpeed = 0.15f;
	public float rotateTransitionSpeed = 1.0f;
	private InputDevice inputDevice;
	public static State state;
	public static Selected selected;

	public enum State
	{
		Main,
		Help
	}

	public enum Selected
	{
		Play,
		Help,
		Back
	}

	void Awake()
	{
		selected = Selected.Play;
		state = State.Main;
		// Setup the input manager
		InputManager.Setup ();
	}
	void FixedUpdate()
	{
		InputManager.Update();
		// Use last device which provided input.
		inputDevice = InputManager.ActiveDevice;
		switch(state)
		{
			case State.Help:
				if (inputDevice.DPadLeft.WasPressed ||
			    	inputDevice.LeftTrigger.WasPressed ||
			    	inputDevice.LeftBumper.WasPressed)
				{
					state = State.Main;
					// Rotate back towards the main menu
					iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 0, 0), rotateTransitionSpeed);
				}
				break;
			case State.Main:
			default:
				if (inputDevice.DPadRight.WasPressed ||
			    	inputDevice.RightTrigger.WasPressed ||
			    	inputDevice.RightBumper.WasPressed)
				{
					state = State.Help;
					// Rotate towards the help page
					iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 90, 0), rotateTransitionSpeed);
				}
				break;
		}
	}
	
	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	void OnMouseDown()
	{
		//print ("MouseTrigger: " + this.name);
		if (this.name == "PlayButton")
		{
			// Load the air level
			CameraFade.StartAlphaFade( Color.black, false, 1.5f, 0.0f, () => { Application.LoadLevel("AirRoom"); } );
		}
		else if (this.name == "HelpButton")
		{
			state = State.Help;
			// Rotate towards the help page
			iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 90, 0), rotateTransitionSpeed);
		}
		else if (this.name == "BackButton")
		{
			state = State.Main;
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
