/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour
{
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
			GameController.controller.triggerEndLevel(gameObject.name);
		}
	}
}
