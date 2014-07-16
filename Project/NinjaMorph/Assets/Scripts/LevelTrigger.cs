using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour
{
	GameController gameController;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		// Get the game controller
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
	}

	/// <summary>
	/// Raises the trigger stay event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter(Collider collider)
	{
		// Make sure it is the player
		if (collider.gameObject.tag == "Player")
		{
			// Let the game controller know we triggered
			gameController.triggerEndLevel(gameObject.name);
		}
	}
}
