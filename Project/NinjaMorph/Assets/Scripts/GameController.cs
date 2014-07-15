// Comment the line below if not play testing
//#define PLAY_TESTING

using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour
{
	private GameObject player;

	private NinjaController ninjaController;
#if PLAY_TESTING
	private PlayTestInfo testInfo;
	private PlayTesting playTest;
#endif

	private bool airLevelComplete = false;
	private GameObject airPuzzle;

	private bool fireLevelComplete = false;
	private GameObject firePuzzle;
	private ArrayList torchOrder = new ArrayList(4);

	private bool waterLevelComplete = false;
	private GameObject waterPuzzle;
	private bool firstWaterMessage = false;
	private int timerThreshhold = 5;

	private bool gameActive = true;

	private string currentLevel = "";

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(gameObject);
#if PLAY_TESTING
		playTest = new PlayTesting ();
		testInfo = new PlayTestInfo ();
		testInfo.startTime = Time.time;
#endif
		// Get the Player
		player = GameObject.FindGameObjectWithTag("Player");
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

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		// Only update if active
		if (!gameActive)
			return;
		
		// See if we need to find the ninja controller again
		if (player == null)
		{
			// Get the Player
			player = GameObject.FindGameObjectWithTag("Player");
		}
		// See if we need to find the ninja controller again
		if (ninjaController == null)
		{
			// Get the Ninja Controller
			ninjaController = player.GetComponent<NinjaController> ();
		}
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

		// Check for Game Over
		CheckGameOver ();
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
#if PLAY_TESTING
			// Set the test data
			testInfo.endTime = Time.time;
			testInfo.numberAttacks = ninjaController.numAttacks;
			testInfo.averageHealth = ninjaController.avgHealth();
			testInfo.totalAirTime = ninjaController.totalAirTime;
			testInfo.totalFireTime = ninjaController.totalFireTime;
			testInfo.totalWaterTime = ninjaController.totalWaterTime;
			testInfo.totalAirCharging = ninjaController.totalAirCharging;
			testInfo.totalFireCharging = ninjaController.totalFireCharging;
			testInfo.totalWaterCharging = ninjaController.totalWaterCharging;
			testInfo.numberTroopsKilled = ninjaController.numKills;
			testInfo.success = false;
			// Save the play test data
			playTest.Save(testInfo);
#endif
			StartCoroutine(GameOver());
		}
		// Still alive
		else
		{
			// Check for level completion
			if (!airLevelComplete)
			{
#if PLAY_TESTING
				if (testInfo.startAirTime < 0.0f)
					testInfo.startAirTime = Time.time;
#endif
				if (airLevelComplete)
				{
					ninjaController.createMessage("Air Room complete!");
#if PLAY_TESTING
					// Track the end air time
					testInfo.endAirTime = Time.time;
#endif
				}
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
							ninjaController.createMessage("Incorrect order!");
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

						ninjaController.createMessage("Fire Room complete!");
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
#if PLAY_TESTING
				if (testInfo.startWaterTime < 0.0f)
					testInfo.startWaterTime = Time.time;
#endif
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

				if(!firstWaterMessage && wptimer.getStarted()) {
					ninjaController.createMessage("Fill the buckets before time runs out! You have 30 seconds left!");
					firstWaterMessage = true;
				}

				if (wptimer.getTimer() >= 30.0f) {
					//clear the buckets
					foreach(FillableObject fo in triggers) {
						fo.clearBucket();
						fo.filled = false;
					}
					//restart timer
					wptimer.setTimer(0.0f);
					wptimer.setStarted(false);
					firstWaterMessage = false;
					timerThreshhold = 5;
					//tell user he/she ran out of time
					ninjaController.createMessage("You ran out of time!");
#if PLAY_TESTING
					testInfo.failedWaterPuzzles++;
#endif
				} else if(wptimer.getStarted() && wptimer.getTimer() > timerThreshhold) {
					ninjaController.createMessage("You have " + (30 - timerThreshhold) + " seconds left!");
					timerThreshhold += 5;
				} else if (numberFilled == bucketWaters.Capacity) {
					//tell them they've completed the room
					ninjaController.createMessage("Water Room complete!");
					waterLevelComplete = true;
#if PLAY_TESTING
					// Track the end water time
					testInfo.endWaterTime = Time.time;
#endif
				} 

			}
			else
			{
#if PLAY_TESTING
				// Set the test data
				testInfo.endTime = Time.time;
				testInfo.numberAttacks = ninjaController.numAttacks;
				testInfo.averageHealth = ninjaController.avgHealth();
				testInfo.totalAirTime = ninjaController.totalAirTime;
				testInfo.totalFireTime = ninjaController.totalFireTime;
				testInfo.totalWaterTime = ninjaController.totalWaterTime;
				testInfo.totalAirCharging = ninjaController.totalAirCharging;
				testInfo.totalFireCharging = ninjaController.totalFireCharging;
				testInfo.totalWaterCharging = ninjaController.totalWaterCharging;
				testInfo.numberTroopsKilled = ninjaController.numKills;
				testInfo.success = true;
				// Save the play test data
				playTest.Save(testInfo);
#endif
				// Create a message
				ninjaController.createMessage("Level Complete!");
				gameActive = false;
			}
		}
	}

	/// <summary>
	/// Triggers the end level.
	/// </summary>
	/// <param name="triggerName">Trigger name.</param>
	public void triggerEndLevel(string triggerName)
	{
		print ("Trigger End Level: " + triggerName);
		string nextLevel = "";
		if (triggerName.Contains("Air"))
		{
			airLevelComplete = true;
			nextLevel = "FireRoom";
		}
		else if (triggerName.Contains("Fire"))
		{
			fireLevelComplete = true;
			nextLevel = "WaterRoom";
		}
		else if (triggerName.Contains("Water") && waterLevelComplete)
		{
			nextLevel = "GameComplete";
		}
		currentLevel = nextLevel;
		// Load the next level
		CameraFade.StartAlphaFade( Color.black, false, 2.5f, 0.0f, () => { Application.LoadLevel(nextLevel); } );
	}

	/// <summary>
	/// Games the over.
	/// </summary>
	/// <returns>The over.</returns>
	IEnumerator GameOver()
	{
		// Create a message
		ninjaController.createMessage("Game Over!");
		// Hold out for 5 seconds
		yield return new WaitForSeconds(5.0f);
		// Reset the scene
		Application.LoadLevel(currentLevel);
	}
}
