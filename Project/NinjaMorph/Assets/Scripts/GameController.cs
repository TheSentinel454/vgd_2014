/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/

using UnityEngine;
using System.Collections;
using InControl;
using RAIN.Entities;
using RAIN.Core;

public class GameController : MonoBehaviour
{
	public static GameObject instance;
	public static GameController controller;

	private GameObject player;
	private GameObject pauseUI;
	private GameObject statsUI;

	private NinjaController ninjaController;
	private CyborgNinjaAnimationController ninjaAnimationController;
	private MessageManager msgManager;
	private PauseGame pauseScript;

	private StatsInfo currentStats;
	private StatsInfo totalStats;

	private bool airLevelComplete = true;

	private bool fireLevelComplete = true;
	private GameObject firePuzzle;
	private ArrayList torchOrder = new ArrayList(4);

	private bool waterLevelComplete = true;
	private GameObject waterPuzzle;
	private bool firstWaterMessage = false;
	private int timerThreshhold = 5;

	private bool comboLevelComplete = false;
	private GameObject comboPuzzle;
	private bool bucketsComplete = false;
	private bool torchesComplete = false;

	private bool gameActive = true;
	private bool controllable = true;
	private bool showStats = false;

	public void hideStats()
	{
		showStats = false;
	}
	public void enablePause()
	{
		pauseScript.allowed = true;
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
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
		// Initialize stats info
		currentStats = new StatsInfo();
		currentStats.startTime = Time.time;
		totalStats = new StatsInfo();
		totalStats.startTime = Time.time;
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
			else if (puzzle.name.Equals("Combo Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				comboPuzzle = puzzle;
		}
		// See if we need to find the pause UI
		if (pauseUI == null)
		{
			// Get the Pause UI
			pauseUI = GameObject.FindGameObjectWithTag("PauseUI");
		}
		// See if we need to find the stats UI
		if (statsUI == null)
		{
			// Get the Stats UI
			statsUI = GameObject.FindGameObjectWithTag("StatsUI");
			if (statsUI != null)
			{
				// Disable it for now
				statsUI.SetActive(false);
			}
		}
		// See if we need to find the ninja animation controller again
		if (ninjaAnimationController == null)
		{
			// Get the Ninja Animation Controller
			ninjaAnimationController = player.GetComponent<CyborgNinjaAnimationController> ();
		}
		// Get the pause script
		pauseScript = GetComponent<PauseGame> ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		if (pauseUI == null)
			pauseUI = GameObject.FindGameObjectWithTag("PauseUI");
		if (statsUI == null)
			statsUI = GameObject.FindGameObjectWithTag("StatsUI");
		if (statsUI != null)
		{
			statsUI.SetActive(showStats);
		}
		if (pauseUI != null)
		{
			pauseUI.SetActive(isPaused());
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
		
		if (!controllable)
			return;

		// See if we need to find the pause UI
		if (pauseUI == null)
		{
			// Get the Pause UI
			pauseUI = GameObject.FindGameObjectWithTag("PauseUI");
		}
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
		// See if we need to find the ninja animation controller again
		if (ninjaAnimationController == null)
		{
			// Get the Ninja Animation Controller
			ninjaAnimationController = player.GetComponent<CyborgNinjaAnimationController> ();
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
			else if (puzzle.name.Equals("Combo Puzzle", System.StringComparison.CurrentCultureIgnoreCase))
				comboPuzzle = puzzle;
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
	/// Sets the controllable.
	/// </summary>
	/// <param name="control">If set to <c>true</c> controllable.</param>
	public void setControllable(bool control)
	{
		if (ninjaController != null)
			ninjaController.setControl(control);
		if (ninjaAnimationController != null)
			ninjaAnimationController.controllable = control;
		GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");
		if (enemy != null)
			enemy.GetComponentInChildren<AIRig> ().enabled = control;
		controllable = control;
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
			// Set the stats data
			currentStats.endTime = Time.time;
			currentStats.transferLevelSpecificStats(ninjaController.currentStats);
			ninjaController.currentStats = new StatsInfo();
			StartCoroutine(GameOver());
		}
		// Still alive
		else
		{
			// Check for level completion
			if (!airLevelComplete)
			{
				if (totalStats.startAirTime < 0.0f)
					totalStats.startAirTime = Time.time;

				if (airLevelComplete)
				{
					ninjaController.createMessage("Air Room complete!");
				}
			}
			else if (!fireLevelComplete)
			{
				if (totalStats.startFireTime < 0.0f)
					totalStats.startFireTime = Time.time;

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
						}
						else
						{
							// Clear fire from torches
							foreach(InteractiveObject io in objects)
								io.removeFire();
							// Clear the order
							torchOrder.Clear();
							// Increment the failure count
							totalStats.failedFirePuzzles++;
						}
					}
				}
			}
			else if (!waterLevelComplete)
			{
				if (totalStats.startWaterTime < 0.0f)
					totalStats.startWaterTime = Time.time;

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
						totalStats.failedWaterPuzzles++;
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
						GameObject.Destroy(GameObject.Find ("Exit Door"));
					} 
				}
			}
			else if (!comboLevelComplete)
			{
				print ("Checking combo level!");
				if (totalStats.startComboTime < 0.0f)
					totalStats.startComboTime = Time.time;
				
				if (Application.loadedLevelName == "ComboRoom")
				{
					if (comboPuzzle == null)
						return;

					// Check the buckets if not complete already
					if (!bucketsComplete)
					{
						print("Checking buckets!");
						int filledBuckets = 0;
						// Find all fillable objects
						FillableObject[] objects = comboPuzzle.GetComponentsInChildren<FillableObject>();
						foreach(FillableObject fo in objects)
						{
							if (fo.filled)
							{
								filledBuckets++;
							}
						}
						print ("Filled buckets: " + filledBuckets);
						// Buckets complete
						if (filledBuckets == 2)
						{
							print ("Buckets complete!");
							bucketsComplete = true;
							// Find crane
							GameObject crane = null;
							foreach(Transform tran in comboPuzzle.GetComponentsInChildren<Transform>())
							{
								if (tran.gameObject.name == "Crane")
								{
									crane = tran.gameObject;
									break;
								}
							}
							// Transition the crane into position
							iTween.RotateTo(crane, new Vector3(270.0f, 160.0f, 0.0f), 5.0f);
							print ("Buckets done!");
						}
					}

					// Check the torches if not complete already
					if (!torchesComplete)
					{
						// Find elevator
						GameObject elevator = null;
						foreach(Transform tran in comboPuzzle.GetComponentsInChildren<Transform>())
						{
							if (tran.gameObject.name == "Elevator")
							{
								elevator = tran.gameObject;
								break;
							}
						}
						// Find all interactive objects
						InteractiveObject[] objects = comboPuzzle.GetComponentsInChildren<InteractiveObject>();
						int numberLit = 0;
						foreach(InteractiveObject io in objects)
						{
							if (io.getObjectType() == ObjectType.Fire)
							{
								numberLit++;
							}
						}
						// Torches complete
						if (numberLit == 2)
						{
							torchesComplete = true;
							// Transition the elevator into position (from -33,60,-22 to -33,35,-22)
							StartCoroutine(MoveElevator(elevator, new Vector3(-33.0f, 35.0f, -22.0f), 5.0f));
							print ("Torches complete!");
						}
					}

					// TODO: Create combo level puzzle
					//comboLevelComplete = true;
				}
			}
			else
			{
				// Create a message
				ninjaController.createMessage("Level Complete!");
				triggerEndLevel("Combo");
				// Reset the puzzles
				waterLevelComplete = false;
				airLevelComplete = false;
				fireLevelComplete = false;
				comboLevelComplete = false;
			}
		}
	}

	IEnumerator MoveElevator(GameObject elevator, Vector3 finalPosition, float time)
	{
		Vector3 start = elevator.transform.localPosition;
		float timePassed = 0.0f;
		while(!elevator.transform.localPosition.Equals(finalPosition))
		{
			print ("Elevator Position: " + elevator.transform.localPosition);
			elevator.transform.localPosition = Vector3.Lerp(start, finalPosition, timePassed / time);
			yield return new WaitForSeconds(0.01f);
			timePassed += 0.01f;
		}
	}

	/// <summary>
	/// Triggers the end level.
	/// </summary>
	/// <param name="triggerName">Trigger name.</param>
	public void triggerEndLevel(string triggerName)
	{
		string nextLevel = "";
		if (triggerName.Contains("Air"))
		{
			airLevelComplete = true;
			// Track the end air time
			totalStats.endAirTime = Time.time;
			nextLevel = "FireRoom";
		}
		else if (triggerName.Contains("Fire"))
		{
			fireLevelComplete = true;
			// Track the end fire time
			totalStats.endFireTime = Time.time;
			nextLevel = "WaterRoom";
		}
		else if (triggerName.Contains("Water"))
		{
			waterLevelComplete = true;
			// Track the end water time
			totalStats.endWaterTime = Time.time;
			nextLevel = "ComboRoom";
		}
		else if (triggerName.Contains("Combo"))
		{
			comboLevelComplete = true;
			// Track the end combo time
			totalStats.endComboTime = Time.time;
			// Track the end time
			totalStats.endTime = Time.time;
			nextLevel = "NinjaMorph";
		}
		// Disable control for the time being
		setControllable(false);
		// Enable the Stats UI
		statsUI.SetActive (true);
		// Set the next level
		statsUI.GetComponentInChildren<StatsMenuHandler> ().nextLevel = nextLevel;
		// Transfer relevant stats to the total stats
		totalStats.transferLevelSpecificStats (ninjaController.currentStats);
		// Set the stats
		statsUI.GetComponentInChildren<ShowStats> ().setStats (nextLevel, ninjaController.currentStats, totalStats);
		// Reset the ninja controller stats
		ninjaController.currentStats = new StatsInfo ();
		// Enable the Stats UI
		showStats = true;
		// Disable the ability to pause
		pauseScript.allowed = false;
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
		CameraFade.StartAlphaFade( Color.black, false, 5.0f, 0.0f, () =>
		{
			// Reset the puzzle state
			resetPuzzleState();
			// Reset the stats
			currentStats.resetLevelSpecificStats();
			// Reload the scene
			Application.LoadLevel(Application.loadedLevelName);
		} );
		// Hold out for 5 seconds
		yield return new WaitForSeconds(5.0f);
		// Set the game back to active
		gameActive = true;
	}

	/// <summary>
	/// Resets the state of the puzzles.
	/// </summary>
	private void resetPuzzleState()
	{
		// Reset the torch order queue
		torchOrder.Clear ();
	}
}