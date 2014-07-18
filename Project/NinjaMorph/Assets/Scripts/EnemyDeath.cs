﻿/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class EnemyDeath : MonoBehaviour
{
	void ObjectDeath()
	{
		// Destroy for right now (Need to do death animation ultimately)
		Destroy (gameObject);
	}
}
