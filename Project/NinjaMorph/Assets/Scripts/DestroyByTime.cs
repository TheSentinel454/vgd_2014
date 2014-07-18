﻿/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour
{
	public float lifetime = float.MaxValue;

	private ParticleSystem particles;
	private float startTime;
	private float endTime;
	private float startRate;

	void Start()
	{
		// Check for a particle system
		particles = (ParticleSystem)GetComponent<ParticleSystem> ();
		if (particles != null)
		{
			// Set start emission rate
			startRate = particles.emissionRate;
		}
		// Set the times
		startTime = Time.time;
		endTime = startTime + lifetime;
	}

	void Update()
	{
		// Check for a particle system
		if (particles != null)
		{
			// Set the emission rate based on the time passed
			particles.emissionRate -= ((Time.deltaTime / lifetime) * startRate) * 1.5f;
			if (particles.emissionRate < 0.0f)
				particles.emissionRate = 0.0f;
		}
		// Check for end condition
		if (Time.time >= endTime)
		{
			// Destroy the object
			Destroy(gameObject);
		}
	}
}
