/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
//#define PLAY_TESTING

using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Play test info.
/// </summary>
[System.Serializable]
public class PlayTestInfo
{
	public float startTime = -1.0f;			// Done
	public float endTime = -1.0f;			// Done

	public float startFireTime = -1.0f;		// Done
	public float endFireTime = -1.0f;		// Done
	public int failedFirePuzzles = 0;		// Done

	public float startWaterTime = -1.0f;	// Done
	public float endWaterTime = -1.0f;		// Done
	public int failedWaterPuzzles = 0;		// Done
	
	public float totalAirTime = 0.0f;		// Done
	public float totalWaterTime = 0.0f;		// Done
	public float totalFireTime = 0.0f;		// Done
	
	public float totalAirCharging = 0.0f;	// Done
	public float totalWaterCharging = 0.0f;	// Done
	public float totalFireCharging = 0.0f;	// Done

	public int numberAttacks = 0;			// Done
	public int numberTroopsKilled = 0;		// Done
	public float averageHealth = 100.0f;	// Done

	public bool success = false;

	/// <summary>
	/// Gets the data.
	/// </summary>
	/// <returns>The data.</returns>
	public string getData()
	{
		return (startTime + "," + endTime + "," + (endTime - startTime) + "," + 
		        startFireTime + "," + endFireTime + "," + (endFireTime - startFireTime) + "," + 
		        startWaterTime + "," + endWaterTime + "," + (endWaterTime - startWaterTime) + "," +
		        totalAirTime + "," + totalWaterTime + "," + totalFireTime + "," +
		        totalAirCharging + "," + totalWaterCharging + "," + totalFireCharging + "," +
		        failedFirePuzzles + "," + failedWaterPuzzles + "," +
		        numberAttacks + "," + numberTroopsKilled + "," + averageHealth + "," + success + 
		        "\n");
	}
}

/// <summary>
/// Play testing.
/// </summary>
public class PlayTesting
{
	/// <summary>
	/// Save the specified info.
	/// </summary>
	/// <param name="info">Info.</param>
	public void Save(PlayTestInfo info)
	{
#if PLAY_TESTING
		// Set the file path
		string filePath = Application.persistentDataPath + "/playTest.csv";

		// Check for the file not existing
		if (!File.Exists(filePath))
		{
			// Write the headers out
			File.AppendAllText(filePath, "Start Time,End Time,Total Time," +
			                   			 "Fire Start Time,Fire End Time,Fire Total Time," +
			                   			 "Water Start Time,Water End Time,Water Total Time," +
			                   			 "Air Time,Water Time,Fire Time," +
			                   			 "Air Charge Time,Water Charge Time,Fire Charge Time," +
			                   			 "Failed Fire,Failed Water," +
			                   			 "Number of Attacks,Troops Killed,Average Health,Success\n");
		}
		Debug.Log("Play Test Data saved: " + filePath);
		// Write all the data out
		File.AppendAllText(filePath, info.getData());
#endif
	}
}
