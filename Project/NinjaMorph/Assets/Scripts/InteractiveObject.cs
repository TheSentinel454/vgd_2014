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
	private NinjaType type;
	public NinjaType GetNinjaType(){ return type; }

	public InteractiveCollision(ControllerColliderHit colHit, NinjaType ninjaType)
	{
		hit = colHit;
		type = ninjaType;
	}
}

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
		Ice,
		Fire
	}

	public ObjectType type;
	private ObjectType currentType;

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
	void PlayerCollision(InteractiveCollision collision)
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
			GameObject fire = (GameObject)Instantiate(fireGameObject, transform.position, transform.rotation);
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
			// Clear the fire prefab
			
			break;
		}
	}
}