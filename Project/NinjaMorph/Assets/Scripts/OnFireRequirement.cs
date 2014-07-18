/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class OnFireRequirement : Requirement {
	public override bool complete { get {
		return GetComponent<InteractiveObject>().getObjectType() == ObjectType.Fire;
	}}
}

