/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class InteractiveCollision
{
	private ControllerColliderHit hit;
	public ControllerColliderHit GetHit(){ return hit; }
	private Collider collider;
	public Collider GetCollider(){ return collider; }
	private NinjaType type;
	public NinjaType GetNinjaType(){ return type; }

	public InteractiveCollision(ControllerColliderHit colHit, NinjaType ninjaType)
	{
		hit = colHit;
		type = ninjaType;
	}

	public InteractiveCollision(Collider col, NinjaType ninjaType)
	{
		collider = col;
		type = ninjaType;
	}
}
[System.Serializable]
public enum ObjectType
{
	Undefined,
	Air,
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
	Ice,
	Fire
}

public class InteractiveObject : MonoBehaviour
{
	public ObjectType type = ObjectType.Undefined;
	public ObjectType getObjectType(){ return currentType; }
	private ObjectType currentType;
	private GameObject fire;
	private GameObject smoke;

	public GameObject fireGameObject;
	public GameObject smokeGameObject;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		// Set the current type
		currentType = type;
	}

	/// <summary>
	/// Handle a player collision
	/// </summary>
	/// <param name="hit">Hit.</param>
	public void PlayerCollision(InteractiveCollision collision)
	{
		switch(collision.GetNinjaType())
		{
		case NinjaType.Air:
			HandleAirNinjaCollision();
			break;
		case NinjaType.Fire:
			HandleFireNinjaCollision();
			break;
		case NinjaType.Water:
			HandleWaterNinjaCollision();
			break;
		case NinjaType.Base:
		default:
			HandleBaseNinjaCollision();
			break;
		}
	}

	/// <summary>
	/// Handles the base ninja collision.
	/// </summary>
	void HandleBaseNinjaCollision()
	{
	}

	/// <summary>
	/// Handles the air ninja collision.
	/// </summary>
	void HandleAirNinjaCollision()
	{
	}

	/// <summary>
	/// Handles the fire ninja collision.
	/// </summary>
	void HandleFireNinjaCollision()
	{
		switch(currentType)
		{
		case ObjectType.Wood:
			// Set the current type to fire
			currentType = ObjectType.Fire;
			// Instantiate the fire prefab
			fire = (GameObject)Instantiate(fireGameObject, transform.position, transform.rotation);
			fire.transform.parent = transform;
			break;
		}
	}

	/// <summary>
	/// Handles the water ninja collision.
	/// </summary>
	void HandleWaterNinjaCollision()
	{
		switch(currentType)
		{
		case ObjectType.Fire:
			// Set the current type to initial type
			currentType = type;
			// Destroy the fire prefab
			Destroy(fire);
			// Instantiate the smoke prefab
			smoke = (GameObject)Instantiate(smokeGameObject, transform.position, transform.rotation);
			smoke.transform.parent = transform;
			DestroyByTime dbtScript = smoke.AddComponent<DestroyByTime>();
			dbtScript.lifetime = 4.0f;
			break;
		}
	}
}