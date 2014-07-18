﻿/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class DamageObject : MonoBehaviour
{
	public float maxHealth = 100.0f;
	public float minHealth = 0.0f;

	public float health = 100.0f;
	public bool alive = true;
	public bool allowHealWhenDead = false;

	/// <summary>
	/// Deals the damage.
	/// </summary>
	/// <param name="damage">Damage.</param>
	public void DealDamage(float damage)
	{
		// Decrement the health
		health -= damage;
		// Check for death
		if (health <= minHealth)
		{
			// Dead
			alive = false;
			// Set the health
			health = minHealth;
			// Send the message (Reuqire a receiver)
			SendMessage("ObjectDeath", SendMessageOptions.RequireReceiver);
		}
	}

	/// <summary>
	/// Heal the specified heal amount
	/// </summary>
	/// <param name="heal">Heal.</param>
	public void Heal(float heal)
	{
		// Check for alive or bypass
		if (alive || allowHealWhenDead)
		{
			// Heal for the amount or max out
			health = Mathf.Min (health + heal, maxHealth);
		}
	}
}
