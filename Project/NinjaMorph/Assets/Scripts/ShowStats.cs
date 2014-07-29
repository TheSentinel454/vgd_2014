using UnityEngine;
using System.Collections;

public class ShowStats : MonoBehaviour
{
	public string nextLvl;
	private StatsInfo levelStats;
	private StatsInfo totalStats;
	// STYLES
	GUIStyle style = new GUIStyle ();
	
	// TEXTURES
	Texture2D texture;

	void Start()
	{
		texture = new Texture2D(1, 1);
		texture.SetPixel (0, 0, new Color(0.0f, 0.0f, 0.0f, 1.0f));
		texture.Apply ();
		style.normal.background = texture;
	}

	/// <summary>
	/// Sets the stats.
	/// </summary>
	/// <param name="nextLevel">Next Level.</param>
	/// <param name="info">Info.</param>
	public void setStats(string nextLevel, StatsInfo levelInfo, StatsInfo totalInfo)
	{
		// Store a copy
		levelStats = levelInfo;
		totalStats = totalInfo;
		nextLvl = nextLevel;
	}

	/// <summary>
	/// Raises the GUI event.
	/// </summary>
	void OnGUI()
	{
		if (levelStats == null || totalStats == null || nextLvl == null)
			return;
		// Define some constant for all the bars
		float top = Screen.height / 2.0f - 145;
		float left = Screen.width / 2.0f - 125;
		float space = 15;
		float textLeft = left + 10;
		float textHeight = 30.0f;
		float containerWidth = 250.0f;
		float containerHeight = 350.0f;

		GUI.Box(new Rect(left, top, containerWidth, containerHeight), "Room Statistics");
		top += 20;
		float ttc = 0.0f;
		if (nextLvl.Contains("Fire"))
			ttc = totalStats.endAirTime - totalStats.startAirTime;
		else if (nextLvl.Contains("Water"))
			ttc = totalStats.endFireTime - totalStats.startFireTime;
		else if (nextLvl.Contains("NinjaMorph"))
			ttc = totalStats.endWaterTime - totalStats.startWaterTime;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Time to complete: " + ttc + " seconds");
		top += space;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Average Health: " + levelStats.getAvgHealth());
		top += space;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Air Energy Spent: " + levelStats.totalAirTime);
		top += space;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Air Energy Gathered: " + levelStats.totalAirCharging);
		top += space;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Fire Energy Spent: " + levelStats.totalFireTime);
		top += space;
		GUI.Label (new Rect (textLeft, top , containerWidth, textHeight), "Fire Energy Gathered: " + levelStats.totalFireCharging);
		top += space;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Water Energy Spent: " + levelStats.totalWaterTime);
		top += space;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Water Energy Gathered: " + levelStats.totalWaterCharging);
		top += space;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Number of attacks: " + levelStats.numberAttacks);
		top += space;
		GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Troops killed: " + levelStats.numberTroopsKilled);
		top += space;
		if (nextLvl.Contains("Water"))
		{
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Failed fire puzzles: " + levelStats.failedFirePuzzles);
			top += space;
		}
		else if (nextLvl.Contains("Combo"))
		{
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Failed water puzzles: " + levelStats.failedWaterPuzzles);
			top += space;
		}
		else if (nextLvl.Contains("NinjaMorph"))
		{
			// Display total
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Time to complete: " + (totalStats.endTime - totalStats.startTime) + " seconds");
			top += space;
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Average Health: " + totalStats.getAvgHealth());
			top += space;
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Air Energy Spent: " + totalStats.totalAirTime);
			top += space;
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Air Energy Gathered: " + totalStats.totalAirCharging);
			top += space;
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Fire Energy Spent: " + totalStats.totalFireTime);
			top += space;
			GUI.Label (new Rect (textLeft, top , containerWidth, textHeight), "Total Fire Energy Gathered: " + totalStats.totalFireCharging);
			top += space;
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Water Energy Spent: " + totalStats.totalWaterTime);
			top += space;
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Water Energy Gathered: " + totalStats.totalWaterCharging);
			top += space;
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Number of attacks: " + totalStats.numberAttacks);
			top += space;
			GUI.Label (new Rect (textLeft, top, containerWidth, textHeight), "Total Troops killed: " + totalStats.numberTroopsKilled);
			top += space;
		}
	}
}
