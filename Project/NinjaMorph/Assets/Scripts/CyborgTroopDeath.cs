﻿/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;
using RAIN.Core;

public class CyborgTroopDeath : MonoBehaviour {

	void ObjectDeath () {
		transform.Find ("AI").GetComponent<AIRig> ().AI.WorkingMemory.SetItem<bool> ("Dead", true);
		StartCoroutine (FadeOutAndRemove ());
	}

	IEnumerator FadeOutAndRemove () {
		yield return new WaitForSeconds (2.0f);
		Renderer[] renderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer childRenderer in renderers) {
			childRenderer.material.shader = Shader.Find ("Transparent/Bumped Specular");
		}
		
		Hashtable fadeSettings = new Hashtable ();
		fadeSettings.Add ("alpha", 0);
		fadeSettings.Add ("time", 3);
		fadeSettings.Add ("onComplete", "DestroyCyborgTroop");
		iTween.FadeTo(gameObject, fadeSettings);
	}

	void DestroyCyborgTroop () {
		DestroyObject (gameObject);
	}
}
