/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class FillableObject : InteractiveObject
{
	public bool filled = false;

	private Vector3 originalPosition = new Vector3(0,0,0);
	private float fillAmount = 0.0f;
	private bool didPlay; //did the note play already for the given puzzle attempt
	private WaterSounds waterSounds;

	void Start() {
		originalPosition = transform.parent.FindChild("bucket_water").transform.position;
		waterSounds = GameObject.Find ("Water Sounds").GetComponent<WaterSounds> ();
		didPlay = false;
	}

	void Update() {

	}

	public override void PlayerCollisionStay(InteractiveCollision col) {

		if (col.GetNinjaType() == NinjaType.Water && fillAmount <= 1.0) {
			transform.parent.FindChild("bucket_water").Translate(0,0.03f,0);
			fillAmount += 0.03f;
		}
		if (fillAmount >= 1.0 && !didPlay) {
			filled = true;
			waterSounds.playNextWaterSound();
			didPlay = true;
		}
	}

	public void clearBucket() {
		transform.parent.FindChild("bucket_water").transform.position = originalPosition;
		fillAmount = 0.0f;
		didPlay = false;
		waterSounds.getQueue().Clear();
		waterSounds.fillQueue ();
	}

	public void setDidPlay(bool didPlay) {
		this.didPlay = didPlay;
	}

	public bool getDidPlay() {
		return didPlay;
	}

}
