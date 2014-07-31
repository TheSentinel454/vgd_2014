/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/

using UnityEngine;
using System.Collections;

public class WaterSounds : MonoBehaviour {
	
	public AudioSource[] waterSounds; 

	private Queue waterQueue;
	private bool[] playedSounds;
	private AudioSource mainAudio;

	// Use this for initialization
	void Start () {
		playedSounds = new bool[] {false, false, false, false};
		mainAudio = waterSounds[0];
		waterQueue = new Queue ();
		fillQueue ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Queue getQueue() {
		return waterQueue;
	}

	public AudioSource getAudioSource() {
		return mainAudio;
	}

	public void fillQueue() {
		waterQueue.Enqueue (waterSounds [0]);
		waterQueue.Enqueue (waterSounds [1]);
		waterQueue.Enqueue (waterSounds [2]);
		waterQueue.Enqueue (waterSounds [3]);
	}

	public void playNextWaterSound() {
		mainAudio = (AudioSource)waterQueue.Dequeue ();
		mainAudio.Play();
	}
}
