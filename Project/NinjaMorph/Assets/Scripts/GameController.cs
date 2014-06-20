﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject player;
	public GameObject HUD;

	private NinjaController ninjaController;

	private GUIText zenText;
	private GUIText airEnergyText;
	private GUIText fireEnergyText;
	private GUIText waterEnergyText;

	// Use this for initialization
	void Start ()
	{
		// Find the GUI Text
		foreach(GUIText gt in HUD.GetComponentsInChildren<GUIText>())
		{
			if (gt.name.ToLower().Contains("zen"))
				zenText = gt;
			else if (gt.name.ToLower().Contains("air"))
				airEnergyText = gt;
			else if (gt.name.ToLower().Contains("fire"))
				fireEnergyText = gt;
			else if (gt.name.ToLower().Contains("water"))
				waterEnergyText = gt;
		}
		// Get the Ninja Controller
		ninjaController = player.GetComponent<NinjaController> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		zenText.text = "Zen: " + ninjaController.getZen ().ToString ();
		airEnergyText.text = "Air: " + ninjaController.getAirEnergy ().ToString ();
		fireEnergyText.text = "Fire: " + ninjaController.getFireEnergy ().ToString ();
		waterEnergyText.text = "Water: " + ninjaController.getWaterEnergy ().ToString ();
	}
}
