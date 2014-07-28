/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;
using InControl;

public class PauseMenuHandler : MonoBehaviour
{
	private InputDevice inputDevice;
	public static Selected selected;
	
	// Variables for calculating delta time
	private float timeLastFrame;
	private float timeCurrentFrame;
	private float deltaTime;
	private float movementTrigger;

	public enum Selected
	{
		Restart,
		MainMenu
	}

	void Awake()
	{
		// Set the default to restart
		selected = Selected.Restart;
		// Setup the input manager
		InputManager.Setup ();
		timeLastFrame = 0.0f;
		timeCurrentFrame = 0.0f;
		deltaTime = 0.0f;
		movementTrigger = Time.time;
	}
	void Update()
	{
		// Create our own delta time based on realtime since startup
		timeCurrentFrame = Time.realtimeSinceStartup;
		deltaTime = timeCurrentFrame - timeLastFrame;
		timeLastFrame = timeCurrentFrame;

		// Use last device which provided input.
		inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Action1.WasPressed)
		{
			switch(selected)
			{
			case Selected.MainMenu:
				// Load the ninja morph scene
				CameraFade.StartAlphaFade( Color.black, false, 0.5f, 0.0f, () =>
				{
					Destroy (GameController.instance);
					Application.LoadLevel("NinjaMorph");
				} );
				break;
			case Selected.Restart:
			default:
				// Load the current level
				CameraFade.StartAlphaFade( Color.black, false, 0.5f, 0.0f, () =>
				{
					Application.LoadLevel(Application.loadedLevelName);
				} );
				break;
			}
			Time.timeScale = 1.0f;
		}

		switch(selected)
		{
		case Selected.MainMenu:
			gameObject.GetComponent<PulseText>().selected = this.name == "MainMenuButton";
			break;
		case Selected.Restart:
		default:
			gameObject.GetComponent<PulseText>().selected = this.name == "RestartButton";
			break;
		}
		movementTrigger += deltaTime;
		// Handle transitioning between the selected menu items
		if (movementTrigger < 0.25f)
			return;
		if (inputDevice.DPadDown.WasPressed ||
		    inputDevice.LeftStick.Down.WasPressed)
		{
			movementTrigger = 0.0f;
			if (selected == Selected.Restart)
				selected = Selected.MainMenu;
		}
		if (inputDevice.DPadUp.WasPressed ||
		    inputDevice.LeftStick.Up.WasPressed)
		{
			movementTrigger = 0.0f;
			if (selected == Selected.MainMenu)
				selected = Selected.Restart;
		}
	}
}
