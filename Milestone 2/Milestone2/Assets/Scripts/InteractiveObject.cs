using UnityEngine;
using System.Collections;

public class InteractiveObject : MonoBehaviour
{
	public enum ObjectType
	{
		Undefined,
		Wood,
		Water,
		Metal,
		Concrete,
		Dirt,
		Fiber,
		Paper,
		Glass,
		Plastic,
		Rubber,
		Plant,
		Ice
	}

	public ObjectType type;

	void PlayerCollision(ControllerColliderHit hit)
	{
		Debug.Log ("PlayerCollision: " + name);
	}
}