/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;
using InControl;

public class MainMenuHandler : MonoBehaviour
{
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

		if (inputDevice.Action1.WasPressed)
		{
			switch(selected)
			{
			case Selected.Back:
				selected = Selected.Help;
				state = State.Main;
				// Rotate back towards the main menu
				iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 0, 0), rotateTransitionSpeed);
				break;
			case Selected.Help:
				selected = Selected.Back;
				state = State.Help;
				// Rotate towards the help page
				iTween.RotateTo(Camera.main.gameObject, new Vector3(0, 90, 0), rotateTransitionSpeed);
				break;
			case Selected.Play:
			default:
				// Load the air level
				CameraFade.StartAlphaFade( Color.black, false, 1.5f, 0.0f, () => { Application.LoadLevel("AirRoom"); } );
				break;
			}
		}
		switch(selected)
		{
		case Selected.Back:
			gameObject.GetComponent<PulseText>().selected = this.name == "BackButton";
			break;
		case Selected.Help:
			gameObject.GetComponent<PulseText>().selected = this.name == "HelpButton";
			break;
		case Selected.Play:
		default:
			gameObject.GetComponent<PulseText>().selected = this.name == "PlayButton";
			break;
		}
		if (state == State.Main)
		{
			if (inputDevice.DPadDown.WasReleased ||
			    inputDevice.DPadUp.WasReleased ||
			    inputDevice.LeftStick.Up.WasReleased ||
			    inputDevice.LeftStick.Down.WasReleased)
			{
				if (selected == Selected.Play)
					selected = Selected.Help;
				else
					selected = Selected.Play;
			}
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
		if (this.name == "PlayButton")
		{
			selected = Selected.Play;
		}
		else if (this.name == "HelpButton")
		{
			selected = Selected.Help;
		}
		else if (this.name == "BackButton")
		{
			selected = Selected.Back;
		}
	}

	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit()
	{
		// Transition back to white
		gameObject.GetComponent<PulseText>().selected = false;
	}
}
