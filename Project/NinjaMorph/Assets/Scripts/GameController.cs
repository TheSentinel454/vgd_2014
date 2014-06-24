using UnityEngine;
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
		zenText.text = "Zen: " + (int)ninjaController.getZen () + "%";
		airEnergyText.text = "Air: " + (int)ninjaController.getAirEnergy () + "%";
		fireEnergyText.text = "Fire: " + (int)ninjaController.getFireEnergy () + "%";
		waterEnergyText.text = "Water: " + (int)ninjaController.getWaterEnergy () + "%";

		// Check for ninja zen being less than 0
		if (ninjaController.getZen() < 0.0f)
		{
			// Reset the scene
			Application.LoadLevel(0);
		}
	}
}
