// Comment the line below if not play testing
#define PLAY_TESTING

using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour
{
	public GameObject player;
	public GameObject HUD;

	private NinjaController ninjaController;
#if PLAY_TESTING
	private PlayTestInfo testInfo;
	private PlayTesting playTest;
#endif

	private GUIText zenText;
	private GUIText airEnergyText;
	private GUIText fireEnergyText;
	private GUIText waterEnergyText;

	private bool airLevelComplete = true;	// Temporarily set to true
	private GameObject airPuzzle;

	private bool fireLevelComplete = true; // Temporarily set to true
	private GameObject firePuzzle;
	private ArrayList torchOrder = new ArrayList(4);

	private bool waterLevelComplete = true;
	private GameObject waterPuzzle;

	private bool gameActive = true;

	// Use this for initialization
	void Start ()
	{
#if PLAY_TESTING
		playTest = new PlayTesting ();
		testInfo = new PlayTestInfo ();
		testInfo.startTime = Time.time;
#endif
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
#if PLAY_TESTING
				if (testInfo.startFireTime < 0.0f)
					testInfo.startFireTime = Time.time;
#endif
				InteractiveObject[] objects = firePuzzle.GetComponentsInChildren<InteractiveObject>();
				int numberLit = 0;
				foreach(InteractiveObject io in objects)
				{
					if (io.getObjectType() == ObjectType.Fire)
					{
						numberLit++;
						if (!torchOrder.Contains(io.gameObject.name))
							torchOrder.Add(io.gameObject.name);
					}
				}
				if (numberLit == objects.Length)
				{
					fireLevelComplete = true;
					for(int i = 0; i < torchOrder.Count; i++)
					{
						string name = (string)torchOrder[i];
						if (!name.Contains(i.ToString()))
						{
							ninjaController.createMessage("Incorrect order!", 3.0f);
							fireLevelComplete = false;
							break;
						}
					}
					if (fireLevelComplete)
					{


						// Find all of the crates and allow them to be moved.
						Rigidbody[] crates = GameObject.FindGameObjectWithTag("Crates").GetComponentsInChildren<Rigidbody>();
						foreach (Rigidbody crate in crates) {
							crate.isKinematic = false;
						}

						// Make the cannon ball fire
						Bullet cannonBall = GameObject.FindGameObjectWithTag("Cannon Ball").GetComponent<Bullet>();
						cannonBall.speed = 0.3f;

						ninjaController.createMessage("Fire Room complete!", 5.0f);
#if PLAY_TESTING
						// Track the end fire time
						testInfo.endFireTime = Time.time;
#endif
					}
					else
					{
						// Clear fire from torches
						foreach(InteractiveObject io in objects)
							io.removeFire();
						// Clear the order
						torchOrder.Clear();
#if PLAY_TESTING
						// Increment the failure count
						testInfo.failedFirePuzzles++;
#endif
					}
				}
			}
			else if (!waterLevelComplete)
			{

				int numberFilled = 0;
				ArrayList bucketWaters = new ArrayList(4);
				ArrayList triggers = new ArrayList(4);

				WaterPuzzleTimer wptimer = waterPuzzle.GetComponent<WaterPuzzleTimer>();

				//fill up arrays with waters and fill triggers, and keep track of which buckets are filled
				for(int i = 0; i < bucketWaters.Capacity; i++) {
					bucketWaters.Add(GameObject.Find("Fillable Bucket " + i).transform.FindChild("bucket_water").GetComponent<InteractiveObject>());
					triggers.Add(((InteractiveObject)bucketWaters[i]).transform.parent.Find("bottom_trigger" + i).GetComponent<FillableObject>());
					if (((FillableObject)triggers[i]).filled) {
						numberFilled++;
					}
				}

				print ("Timer: " + wptimer.getTimer());

				if (wptimer.getTimer() > 20.0f) {
					//clear the buckets
					foreach(FillableObject fo in triggers) {
						fo.clearBucket();
						fo.filled = false;
					}
					//restart timer
					wptimer.setTimer(0.0f);
					wptimer.setStarted(false);
					//tell user he/she ran out of time
					ninjaController.createMessage("You ran out of time!", 3.0f);
				}
				else if (numberFilled == bucketWaters.Capacity)
				{
					//tell them they've completed the room
					ninjaController.createMessage("Water Room complete!", 5.0f);
					waterLevelComplete = true;
				}

			}
			else
			{
#if PLAY_TESTING
				// Set the test data
				testInfo.endTime = Time.time;
				// Save the play test data
				playTest.Save(testInfo);
#endif
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
