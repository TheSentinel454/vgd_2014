﻿/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class WaterPuzzleTimer : MonoBehaviour {

	private float timer;
	private ArrayList buckets;
	private ArrayList triggers; 
	private bool started;

	// Use this for initialization
	void Start () {
		timer = 0.0f;
		started = false;
		buckets = new ArrayList(4);
		triggers = new ArrayList(4);

		for(int i = 0; i < buckets.Capacity; i++) {
			buckets.Add(transform.FindChild("Fillable Bucket " + i).FindChild("bucket_water").GetComponent<InteractiveObject>());
			triggers.Add(((InteractiveObject)buckets[i]).transform.parent.FindChild("bottom_trigger" + i).GetComponent<FillableObject>());
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
		foreach (FillableObject fo in triggers) {
			if (fo.filled) {
				started = true;
			}
		}
		if (started) {
			timer += Time.deltaTime;
		}
	}

	public void setTimer(float timer) {
		this.timer = timer;
	}

	public float getTimer() {
		return timer;
	}

	public void setStarted(bool started) {
		this.started = started;
	}

	public bool getStarted() {
		return started;
	}
}
