/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour
{
	public float fadeInTime;

	void Start()
	{
		// Load the next level
		CameraFade.StartAlphaFade (Color.black, true, fadeInTime, 0.0f);
	}
}
