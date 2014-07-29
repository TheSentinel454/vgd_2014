/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;
using InControl;

public class StatsMenuHandler : MonoBehaviour
{
	private InputDevice inputDevice;
	public string nextLevel;
	public static Selected selected;

	public enum Selected
	{
		Done
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		// Set the default to done
		selected = Selected.Done;
		// Setup the input manager
		InputManager.Setup ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// Use last device which provided input.
		inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Action1.WasPressed)
		{
			switch(selected)
			{
			case Selected.Done:
			default:
				// Load the next level
				CameraFade.StartAlphaFade( Color.black, false, 1.5f, 0.0f, () =>
				{
					// Hide the stats UI
					GameController.controller.hideStats();
					// Enable control again
					GameController.controller.setControllable(true);
					if (nextLevel == "NinjaMorph")
						Destroy(GameController.instance);
					Application.LoadLevel(nextLevel);
				} );
				break;
			}
		}

		switch(selected)
		{
		case Selected.Done:
		default:
			gameObject.GetComponent<PulseText>().selected = this.name == "DoneButton";
			break;
		}
		/* Not necessary currently
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
		*/
	}
}
