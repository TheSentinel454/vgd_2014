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
	public float startTime = -1.0f;
	public float endTime = -1.0f;

	public float startFireTime = -1.0f;
	public float endFireTime = -1.0f;
	public int failedFirePuzzles = 0;

	public float startWaterTime = -1.0f;
	public float endWaterTime = -1.0f;
	public int failedWaterPuzzles = 0;

	/// <summary>
	/// Gets the data.
	/// </summary>
	/// <returns>The data.</returns>
	public string getData()
	{
		return (startTime + "," + endTime + "," + (endTime - startTime) + "," + 
		        startFireTime + "," + endFireTime + "," + (endFireTime - startFireTime) + "," + 
		        startWaterTime + "," + endWaterTime + "," + (endWaterTime - startWaterTime) + "\n"
		        );
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
		// Set the file path
		string filePath = Application.persistentDataPath + "/playTest.csv";

		// Check for the file not existing
		if (!File.Exists(filePath))
		{
			// Write the headers out
			File.AppendAllText(filePath, "Start Time,End Time,Total Time," +
			                   			 "Fire Start Time,Fire End Time,Fire Total Time," +
			                   			 "Water Start Time,Water End Time,Water Total Time," +
			                   			 "Failed Fire,Failed Water\n");
		}
		Debug.Log("Play Test Data saved: " + filePath);
		// Write all the data out
		File.AppendAllText(filePath, info.getData());
	}
}
