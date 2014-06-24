using UnityEngine;
using System.Collections;

public class DamageObject : MonoBehaviour
{
	const float MAX_HEALTH = 100.0f;
	const float MIN_HEALTH = 0.0f;

	public float health = MAX_HEALTH;
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
		if (health <= MIN_HEALTH)
		{
			// Dead
			alive = false;
			// Set the health
			health = MIN_HEALTH;
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
			health = Mathf.Min (health + heal, MAX_HEALTH);
		}
	}
}
