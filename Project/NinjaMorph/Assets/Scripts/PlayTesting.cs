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
public class StatsInfo
{
	public float startTime = -1.0f;			// Done
	public float endTime = -1.0f;			// Done
	
	public float startAirTime = -1.0f;		// Done
	public float endAirTime = -1.0f;		// Done

	public float startFireTime = -1.0f;		// Done
	public float endFireTime = -1.0f;		// Done
	public int failedFirePuzzles = 0;		// Done

	public float startWaterTime = -1.0f;	// Done
	public float endWaterTime = -1.0f;		// Done
	public int failedWaterPuzzles = 0;		// Done

	public float startComboTime = -1.0f;
	public float endComboTime = -1.0f;
	
	public float totalAirTime = 0.0f;		// Done
	public float totalWaterTime = 0.0f;		// Done
	public float totalFireTime = 0.0f;		// Done
	
	public float totalAirCharging = 0.0f;	// Done
	public float totalWaterCharging = 0.0f;	// Done
	public float totalFireCharging = 0.0f;	// Done

	public int numberAttacks = 0;			// Done
	public int numberTroopsKilled = 0;		// Done

	public bool success = false;

	private float sumHealth = 0.0f;
	private float numHealthPoints = 0.0f;

	public float getAvgHealth()
	{
		return (sumHealth / numHealthPoints);
	}

	/// <summary>
	/// Gets the data.
	/// </summary>
	/// <returns>The data.</returns>
	public string getData()
	{
		return (startTime + "," + endTime + "," + (endTime - startTime) + "," + 
		        startAirTime + "," + endAirTime + "," + (endAirTime - startAirTime) + "," + 
		        startFireTime + "," + endFireTime + "," + (endFireTime - startFireTime) + "," + 
		        startWaterTime + "," + endWaterTime + "," + (endWaterTime - startWaterTime) + "," +
		        startComboTime + "," + endComboTime + "," + (endComboTime - startComboTime) + "," +
		        totalAirTime + "," + totalWaterTime + "," + totalFireTime + "," +
		        totalAirCharging + "," + totalWaterCharging + "," + totalFireCharging + "," +
		        failedFirePuzzles + "," + failedWaterPuzzles + "," +
		        numberAttacks + "," + numberTroopsKilled + "," + getAvgHealth() + "," + success + 
		        "\n");
	}

	/// <summary>
	/// Adds the health point.
	/// </summary>
	/// <param name="health">Health.</param>
	public void addHealthPoint(float health)
	{
		sumHealth += health;
		numHealthPoints += 1.0f;
	}

	/// <summary>
	/// Sets the avg health.
	/// </summary>
	/// <param name="info">Info.</param>
	public void setAvgHealth(StatsInfo info)
	{
		this.sumHealth = info.sumHealth;
		this.numHealthPoints = info.numHealthPoints;
	}

	/// <summary>
	/// Transfers the level specific stats.
	/// Should only be passed the current stats object, and only
	/// be called from the total stats.
	/// </summary>
	/// <param name="info">Info.</param>
	public void transferLevelSpecificStats(StatsInfo info)
	{
		// Add the energy times
		totalAirTime += info.totalAirTime;
		totalFireTime += info.totalFireTime;
		totalWaterTime += info.totalWaterTime;

		// Add the charging times
		totalAirCharging += info.totalAirCharging;
		totalFireCharging += info.totalFireCharging;
		totalWaterCharging += info.totalWaterCharging;

		// Add the attack stats
		numberAttacks += info.numberAttacks;
		numberTroopsKilled += info.numberTroopsKilled;

		// Add the health stats
		sumHealth += info.sumHealth;
		numHealthPoints += info.numHealthPoints;
	}

	/// <summary>
	/// Resets the level specific stats.
	/// </summary>
	public void resetLevelSpecificStats()
	{
		// Clear energy times
		totalAirTime = 0.0f;
		totalFireTime = 0.0f;
		totalWaterTime = 0.0f;

		// Clear charging times
		totalAirCharging = 0.0f;
		totalFireCharging = 0.0f;
		totalWaterCharging = 0.0f;

		// Clear attack stats
		numberAttacks = 0;
		numberTroopsKilled = 0;

		// Clear health stats
		sumHealth = 0.0f;
		numHealthPoints = 0.0f;
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
	public void Save(StatsInfo info)
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
