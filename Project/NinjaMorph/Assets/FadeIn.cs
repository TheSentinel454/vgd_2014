using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour
{	
	void Start()
	{
		// Load the next level
		CameraFade.StartAlphaFade (Color.black, true, 2.5f, 0.0f);
	}
}
