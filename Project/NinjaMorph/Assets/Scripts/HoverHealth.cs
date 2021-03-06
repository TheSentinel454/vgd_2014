﻿/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (DamageObject))]
public class HoverHealth : MonoBehaviour
{
	public LayerMask rayMask;
	DamageObject dmgObject;
	float adjustment = 3.5f;

	private Vector3 worldPosition = new Vector3();
	private Vector3 screenPosition = new Vector3();
	private Camera mainCamera;
	private int healthBarLeft = 100;
	private int barTop = 5;

	private float textOffsetLeft = 38.0f;
	private float textOffsetTop = -2.0f;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		mainCamera = Camera.main;
		dmgObject = GetComponent<DamageObject> ();
	}

	void OnGUI()
	{
		worldPosition = new Vector3(transform.position.x, transform.position.y + adjustment, transform.position.z);
		screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

		// Let's check the direction now
		Vector3 dir = (mainCamera.transform.position - transform.position).normalized;
		float direction = Vector3.Dot(dir, mainCamera.transform.forward);
		if (direction < -0.65f)
		{
			GUI.color = Color.red;
			// Displays a healthbar
			GUI.HorizontalScrollbar(new Rect (screenPosition.x - healthBarLeft / 2, Screen.height - screenPosition.y - barTop, 100, 0), 0, dmgObject.health, 0, dmgObject.maxHealth);
			
			GUI.color = Color.white;
			GUI.contentColor = Color.white;
			// Displays health in text format
			GUI.Label(new Rect(screenPosition.x - healthBarLeft / 2 + textOffsetLeft, Screen.height - screenPosition.y - barTop + textOffsetTop, 100, 100), ((int)dmgObject.health).ToString());
		}
	}
}
