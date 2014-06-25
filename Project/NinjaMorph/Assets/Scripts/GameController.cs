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

	private bool airLevelComplete = true;	// Temporarily set to true
	private GameObject airPuzzle;

	private bool fireLevelComplete = false;
	private GameObject firePuzzle;

	private bool waterLevelComplete = false;
	private GameObject waterPuzzle;

	private bool gameActive = true;

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
		// Get the Puzzles
		GameObject[] puzzles = GameObject.FindGameObjectsWithTag("Puzzle");
		// Find the puzzles
		foreach(GameObject go in puzzles)
		{
			if (go.name.Equals("Air Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				airPuzzle = go;
			else if (go.name.Equals("Fire Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				firePuzzle = go;
			else if (go.name.Equals("Water Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				waterPuzzle = go;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Only update if active
		if (!gameActive)
			return;
		// Update the GUI
		UpdateGUI ();

		// Check for Game Over
		CheckGameOver ();
	}

	/// <summary>
	/// Updates the GUI.
	/// </summary>
	void UpdateGUI()
	{
		// Update energy levels
		zenText.text = "Zen: " + (int)ninjaController.getZen () + "%";
		airEnergyText.text = "Air: " + (int)ninjaController.getAirEnergy () + "%";
		fireEnergyText.text = "Fire: " + (int)ninjaController.getFireEnergy () + "%";
		waterEnergyText.text = "Water: " + (int)ninjaController.getWaterEnergy () + "%";
	}

	/// <summary>
	/// Checks for the game over.
	/// </summary>
	void CheckGameOver()
	{
		// Check for ninja zen being less than 0
		if (ninjaController.getZen() <= 0.0f)
		{
			gameActive = false;
			StartCoroutine(GameOver());
		}
		// Still alive
		else
		{
			// Check for level completion
			if (!airLevelComplete)
			{

			}
			else if (!fireLevelComplete)
			{
				InteractiveObject[] objects = firePuzzle.GetComponentsInChildren<InteractiveObject>();
				int numberLit = 0;
				foreach(InteractiveObject io in objects)
				{
					if (io.getObjectType() == ObjectType.Fire)
					{
						numberLit++;
						/*
						if (!firstTorch)
						{
							if (io.gameObject.name.Contains("1"))
							{
							}
						}
						if (io.gameObject.name.Contains("1") && (!firstTorch && !secondTorch && !thirdTorch && !fourthTorch))
						{
							firstTorch = true;
						}
						else if (io.gameObject.name.Contains("2") && (firstTorch && !secondTorch && !thirdTorch && !fourthTorch))
						{
							secondTorch = true;
						}
						else if (io.gameObject.name.Contains("3") && (firstTorch && secondTorch && !thirdTorch && !fourthTorch))
						{
							thirdTorch = true;
						}
						else if (io.gameObject.name.Contains("4") && (firstTorch && secondTorch && thirdTorch && !fourthTorch))
						{
							fourthTorch = true;
						}
						 */
					}
				}
				if (numberLit == objects.Length)
				{
					fireLevelComplete = true;
					ninjaController.createMessage("Fire Room complete!", 5.0f);
				}
			}
			else if (!waterLevelComplete)
			{
			}
			else
			{
				// Create a message
				ninjaController.createMessage("Level Complete!", 60.0f);
				gameActive = false;
			}
		}
	}

	/// <summary>
	/// Games the over.
	/// </summary>
	/// <returns>The over.</returns>
	IEnumerator GameOver()
	{
		// Create a message
		ninjaController.createMessage("Game Over!", 10.0f);
		// Hold out for 30 seconds
		yield return new WaitForSeconds(10.0f);
		// Reset the scene
		Application.LoadLevel("TutorialCompound");
	}
}
