using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent (typeof (ComboCamera))]

public class ClearSight : MonoBehaviour
{
	private ComboCamera comboCamera;

	void Awake()
	{
		comboCamera = gameObject.GetComponent<ComboCamera> ();
	}

	void Update()
	{
		RaycastHit[] hits;
		// you can also use CapsuleCastAll()
		// TODO: setup your layermask it improve performance and filter your hits.
		hits = Physics.RaycastAll(transform.position, transform.forward, comboCamera.getDistanceToPlayer());
		foreach(RaycastHit hit in hits)
		{
			Renderer R = hit.collider.renderer;
			if (R == null)
				continue; // no renderer attached? go to next hit
			// TODO: maybe implement here a check for GOs that should not be affected like the player
			
			AutoTransparent AT = R.GetComponent<AutoTransparent>();
			if (AT == null) // if no script is attached, attach one
			{
				AT = R.gameObject.AddComponent<AutoTransparent>();
			}
			AT.BeTransparent(); // get called every frame to reset the falloff
		}
	}
}