using UnityEngine;
using System.Collections;

public class TitleAnimation : MonoBehaviour
{
	GameObject fireObject;
	GameObject airObject;
	GameObject waterObject;
	GameObject titleObject;

	public float baseHoldTime = 5.0f;
	public float baseTransitionTime = 3.0f;
	public float fireTransitionTime = 3.0f;
	public float airTransitionTime = 3.0f;
	public float waterTransitionTime = 3.0f;

	// Use this for initialization
	void Start ()
	{
		// Iterate through all of the objects to find the auras
		foreach(Transform tf in GetComponentsInChildren<Transform>())
		{
			// Look for the fire aura
			if (tf.gameObject.name == "Fire Aura")
			{
				fireObject = tf.gameObject;
			}
			// Look for the air aura
			else if (tf.gameObject.name == "Air Aura")
			{
				airObject = tf.gameObject;
			}
			// Look for the water aura
			else if (tf.gameObject.name == "Water Aura")
			{
				waterObject = tf.gameObject;
			}
			else if (tf.gameObject.name == "Title")
			{
				titleObject = tf.gameObject;
			}
		}
		StartCoroutine (HoldBase());
	}

	IEnumerator HoldBase()
	{
		fireObject.SetActive (false);
		airObject.SetActive (false);
		waterObject.SetActive (false);
		yield return new WaitForSeconds (baseHoldTime);
		StartCoroutine (ChangeToFire ());
	}

	IEnumerator ChangeToFire()
	{
		fireObject.SetActive (true);
		airObject.SetActive (false);
		waterObject.SetActive (false);
		// Transition to red
		iTween.ColorTo (titleObject, Color.red, fireTransitionTime);
		// Wait for it
		yield return new WaitForSeconds(fireTransitionTime);

		StartCoroutine (ChangeToAir());
	}
	
	IEnumerator ChangeToAir()
	{
		fireObject.SetActive (false);
		airObject.SetActive (true);
		waterObject.SetActive (false);
		// Transition to white
		iTween.ColorTo (titleObject, Color.white, airTransitionTime);
		// Wait for it
		yield return new WaitForSeconds(airTransitionTime);
		
		StartCoroutine (ChangeToWater());
	}
	
	IEnumerator ChangeToWater()
	{
		fireObject.SetActive (false);
		airObject.SetActive (false);
		waterObject.SetActive (true);
		// Transition to blue
		iTween.ColorTo (titleObject, Color.blue, waterTransitionTime);
		// Wait for it
		yield return new WaitForSeconds(waterTransitionTime);
		
		StartCoroutine (ChangeToBase());
	}
	
	IEnumerator ChangeToBase()
	{
		fireObject.SetActive (false);
		airObject.SetActive (false);
		waterObject.SetActive (false);
		// Transition to gray
		iTween.ColorTo (titleObject, Color.white, baseTransitionTime);
		// Wait for it
		yield return new WaitForSeconds(baseTransitionTime);
		
		StartCoroutine (HoldBase());
	}
}
