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
	/*
	void Start()
	{
		// Load the next level
		CameraFade.StartAlphaFade (Color.black, true, 2.5f, 0.0f);
	}
	*/

	public void fadeIn(float time)
	{
		// Load the next level
		CameraFade.StartAlphaFade (Color.black, true, time, 0.0f);
	}
}
