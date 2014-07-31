/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;
using InControl;

public class PauseGame : MonoBehaviour
{
	public string homeSceneName;
	public bool allowed;
	private bool paused;
	private float alterTime;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		Time.timeScale = 1.0f;
		paused = false;
		AudioListener.volume = 1.0f;
		alterTime = Time.time;
		allowed = true;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		// See if we are paused
		if (paused)
		{
			// Update the input manager in this case
			InputManager.Update();
			// See if the Start button was released
			if (InputManager.ActiveDevice.GetControl(InputControlType.Start).WasReleased)
			{
				alterTime = Time.time;
				// Unpause the game
				paused = false;
				Time.timeScale = 1.0f;
				AudioListener.volume = 1.0f;
			}
		}
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate()
	{
		if (!allowed)
			return;
		// See if the game is not paused
		if (!paused)
		{
			// Make sure that we don't double trigger an unpause
			// so let's wait at least a quarter of a second before
			// we allow activation again
			if (Time.time - alterTime < 0.25f)
				return;
			// Check for the menu press
			if (InputManager.ActiveDevice.GetControl(InputControlType.Start).WasReleased)
			{
				alterTime = Time.time;
				// Pause the game
				paused = true;
				Time.timeScale = 0.0f;
				AudioListener.volume = 0.0f;
			}
		}
	}
}