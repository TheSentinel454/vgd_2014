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

		switch(selected)
		{
		case Selected.Back:
			if (this.name == "BackButton")
			{
				// Transition to gray
				iTween.ColorTo (gameObject, Color.gray, colorTransitionSpeed);
			}
			else
			{
				// Transition to white
				iTween.ColorTo (gameObject, Color.white, colorTransitionSpeed);
			}
			break;
		case Selected.Help:
			if (this.name == "HelpButton")
			{
				// Transition to gray
				iTween.ColorTo (gameObject, Color.gray, colorTransitionSpeed);
			}
			else
			{
				// Transition to white
				iTween.ColorTo (gameObject, Color.white, colorTransitionSpeed);
			}
			break;
		case Selected.Play:
		default:
			if (this.name == "PlayButton")
			{
				// Transition to gray
				iTween.ColorTo (gameObject, Color.gray, colorTransitionSpeed);
			}
			else
			{
				// Transition to white
				iTween.ColorTo (gameObject, Color.white, colorTransitionSpeed);
			}
			break;
		}
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
		if (state == State.Main)
		{
			if (inputDevice.DPadDown.WasReleased ||
			    inputDevice.DPadUp.WasReleased)
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
