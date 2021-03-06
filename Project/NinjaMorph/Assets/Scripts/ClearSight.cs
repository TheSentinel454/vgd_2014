﻿/*
* http://answers.unity3d.com/questions/44815/make-object-transparent-when-between-camera-and-pl.html
*/
using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent (typeof (ComboCamera))]

public class ClearSight : MonoBehaviour
{
	public LayerMask mask = -1;
	private ComboCamera comboCamera;

	void Awake()
	{
		comboCamera = gameObject.GetComponent<ComboCamera> ();
	}

	void Update()
	{
		RaycastHit[] hits;
		// Raycast, but filter the layers based on the mask
		hits = Physics.RaycastAll(transform.position, transform.forward, comboCamera.getDistanceToPlayer(), mask);
		foreach(RaycastHit hit in hits)
		{
			Renderer R = hit.collider.renderer;
			if (R == null)
				continue; // no renderer attached? go to next hit
			
			AutoTransparent AT = R.GetComponent<AutoTransparent>();
			if (AT == null) // if no script is attached, attach one
			{
				AT = R.gameObject.AddComponent<AutoTransparent>();
			}
			AT.BeTransparent(); // get called every frame to reset the falloff
		}
	}
}