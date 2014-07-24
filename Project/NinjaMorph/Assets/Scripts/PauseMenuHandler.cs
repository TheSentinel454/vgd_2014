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

	public enum Selected
	{
		Restart,
		MainMenu
	}

	void Awake()
	{
		selected = Selected.Restart;
		// Setup the input manager
		InputManager.Setup ();
	}

	void Update()
	{
		//InputManager.Setup();
		//InputManager.Update();
		print ("Update: " + selected);

		// Use last device which provided input.
		inputDevice = InputManager.ActiveDevice;

		if (inputDevice.Action1.WasPressed)
		{
			switch(selected)
			{
			case Selected.MainMenu:
				// Load the ninja morph scene
				CameraFade.StartAlphaFade( Color.black, false, 0.5f, 0.0f, () => { Application.LoadLevel("NinjaMorph"); } );
				break;
			case Selected.Restart:
			default:
				// Load the current level
				CameraFade.StartAlphaFade( Color.black, false, 0.5f, 0.0f, () => { Application.LoadLevel(Application.loadedLevelName); } );
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
		// Handle transitioning between the selected menu items
		if (inputDevice.DPadDown.WasReleased ||
			inputDevice.DPadUp.WasReleased ||
			inputDevice.LeftStick.Up.WasReleased ||
			inputDevice.LeftStick.Down.WasReleased)
		{
			if (selected == Selected.Restart)
				selected = Selected.MainMenu;
			else
				selected = Selected.Restart;
		}
	}
	
	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	void OnMouseDown()
	{
		print ("Mouse Down!");
		if (this.name == "RestartButton")
		{
			// Load the current level
			CameraFade.StartAlphaFade( Color.black, false, 0.5f, 0.0f, () => { Application.LoadLevel(Application.loadedLevelName); } );
		}
		else if (this.name == "MainMenuButton")
		{
			// Load the ninja morph scene
			CameraFade.StartAlphaFade( Color.black, false, 0.5f, 0.0f, () => { Application.LoadLevel("NinjaMorph"); } );
		}
	}
	
	/// <summary>
	/// Raises the mouse enter event.
	/// </summary>
	void OnMouseEnter()
	{
		print ("Mouse Enter!");
		if (this.name == "RestartButton")
		{
			selected = Selected.Restart;
		}
		else if (this.name == "MainMenuButton")
		{
			selected = Selected.MainMenu;
		}
	}

	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit()
	{
		print ("Mouse Exit!");
		gameObject.GetComponent<PulseText>().selected = false;
	}
}
