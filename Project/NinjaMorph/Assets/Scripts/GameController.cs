/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
// Comment the line below if not play testing
//#define PLAY_TESTING

using UnityEngine;
using System.Collections;
using InControl;


public class GameController : MonoBehaviour
{
	public static GameObject instance;
	public static GameController controller;

	private GameObject player;

	private NinjaController ninjaController;
	private MessageManager msgManager;
#if PLAY_TESTING
	private PlayTestInfo testInfo;
	private PlayTesting playTest;
#endif

	private bool airLevelComplete = false;

	private bool fireLevelComplete = false;
	private GameObject firePuzzle;
	private ArrayList torchOrder = new ArrayList(4);

	private bool waterLevelComplete = false;
	private GameObject waterPuzzle;
	private bool firstWaterMessage = false;
	private int timerThreshhold = 5;

	private bool gameActive = true;

	void Awake()
	{
		// See if we don't have the singleton yet
		if (!instance)
		{
			// Set the singleton
			instance = gameObject;
			controller = this;
			// Don't destroy this object
			DontDestroyOnLoad(gameObject);
		}
		// We have the singleton already
		else
		{
			// Destroy the new object
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
#if PLAY_TESTING
		playTest = new PlayTesting ();
		testInfo = new PlayTestInfo ();
		testInfo.startTime = Time.time;
#endif
		// Get the Message Manager
		msgManager = GetComponent<MessageManager> ();
		// Get the Player
		player = GameObject.FindGameObjectWithTag("Player");
		// Get the Ninja Controller
		ninjaController = player.GetComponent<NinjaController> ();
		ninjaController.setMessageManager (msgManager);
		// Get the Puzzles
		GameObject puzzle = GameObject.FindGameObjectWithTag("Puzzle");
		
		if(puzzle != null)
		{
			// Find the puzzles
			if (puzzle.name.Equals("Fire Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				firePuzzle = puzzle;
			else if (puzzle.name.Equals("Water Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				waterPuzzle = puzzle;
		}
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void FixedUpdate ()
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
			// Set the message manager
			ninjaController.setMessageManager(msgManager);
		}
		// Get the Puzzles
		GameObject puzzle = GameObject.FindGameObjectWithTag("Puzzle");
		
		if(puzzle != null)
		{
			// Find the puzzles
			if (puzzle.name.Equals("Fire Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				firePuzzle = puzzle;
			else if (puzzle.name.Equals("Water Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				waterPuzzle = puzzle;
		}

		// Check for Game Over
		CheckGameOver ();
	}

	/// <summary>
	/// Is the game paused?
	/// </summary>
	/// <returns><c>true</c>, if paused, <c>false</c> otherwise.</returns>
	public bool isPaused()
	{
		return (Time.timeScale < 1.0f);
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
				if (Application.loadedLevelName == "FireRoom")
				{
					if (firePuzzle == null)
						return;
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
							foreach (Rigidbody crate in crates)
							{
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
			}
			else if (!waterLevelComplete)
			{
#if PLAY_TESTING
				if (testInfo.startWaterTime < 0.0f)
					testInfo.startWaterTime = Time.time;
#endif
				if (Application.loadedLevelName == "WaterRoom")
				{
					int numberFilled = 0;
					ArrayList bucketWaters = new ArrayList(4);
					ArrayList triggers = new ArrayList(4);
					
					if (waterPuzzle == null)
						return;
					WaterPuzzleTimer wptimer = waterPuzzle.GetComponent<WaterPuzzleTimer>();

					//fill up arrays with waters and fill triggers, and keep track of which buckets are filled
					for(int i = 0; i < bucketWaters.Capacity; i++)
					{
						bucketWaters.Add(GameObject.Find("Fillable Bucket " + i).transform.FindChild("bucket_water").GetComponent<InteractiveObject>());
						triggers.Add(((InteractiveObject)bucketWaters[i]).transform.parent.Find("bottom_trigger" + i).GetComponent<FillableObject>());
						if (((FillableObject)triggers[i]).filled)
						{
							numberFilled++;
						}
					}

					if(!firstWaterMessage && wptimer.getStarted())
					{
						ninjaController.createMessage("Fill the buckets before time runs out! You have 30 seconds left!");
						firstWaterMessage = true;
					}

					if (wptimer.getTimer() >= 30.0f)
					{
						//clear the buckets
						foreach(FillableObject fo in triggers)
						{
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
					}
					else if(wptimer.getStarted() && wptimer.getTimer() > timerThreshhold)
					{
						ninjaController.createMessage("You have " + (30 - timerThreshhold) + " seconds left!");
						timerThreshhold += 5;
					}
					else if (numberFilled == bucketWaters.Capacity)
					{
						//tell them they've completed the room
						ninjaController.createMessage("Water Room complete!");
						waterLevelComplete = true;
	#if PLAY_TESTING
						// Track the end water time
						testInfo.endWaterTime = Time.time;
	#endif
					} 
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
				//gameActive = false;
				triggerEndLevel("Water");
				// Reset the puzzles
				waterLevelComplete = false;
				airLevelComplete = false;
				fireLevelComplete = false;
			}
		}
	}

	/// <summary>
	/// Triggers the end level.
	/// </summary>
	/// <param name="triggerName">Trigger name.</param>
	public void triggerEndLevel(string triggerName)
	{
		string nextLevel = "";
		float timeDelay = 2.5f;
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
			nextLevel = "NinjaMorph";
			timeDelay = 5.0f;
		}
		// Load the next level
		CameraFade.StartAlphaFade( Color.black, false, timeDelay, 0.0f, () => { Application.LoadLevel(nextLevel); } );
	}

	/// <summary>
	/// Games the over.
	/// </summary>
	/// <returns>The over.</returns>
	IEnumerator GameOver()
	{
		// Create a message
		ninjaController.createMessage("Game Over!");
		// Reset the scene
		CameraFade.StartAlphaFade( Color.black, false, 5.0f, 0.0f, () => { Application.LoadLevel(Application.loadedLevelName); } );
		// Hold out for 5 seconds
		yield return new WaitForSeconds(5.0f);
		// Set the game back to active
		gameActive = true;
	}
}
