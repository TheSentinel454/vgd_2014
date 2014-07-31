/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;

public class Sprinkler : MonoBehaviour
{
	public float fireRateReduction;
	public float sparksExtraReduction;

	private float heightAboveFloor;
	private GameObject barrel;
	private ParticleSystem[] sprinklers;
	private GameObject floor;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		sprinklers = GetComponentsInChildren<ParticleSystem>();
	}
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		heightAboveFloor = transform.position.y;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits;
			
		hits = Physics.RaycastAll(ray);
			
		foreach(RaycastHit h in hits)
		{
			if(h.collider.name == "ground")
				transform.position = h.point +  new Vector3(0f, heightAboveFloor, 0f);
		}
				
		if(!sprinklers[0].isPlaying)
		{
			for(int i = 0; i < sprinklers.Length; i++)
			{
				sprinklers[i].Play();
			}
		}


	}
}
