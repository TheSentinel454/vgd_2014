using UnityEngine;
using System.Collections;

public class Sprinkler : MonoBehaviour
{
	public float fireRateReduction;
	public float sparksExtraReduction;
	
	
	private float heightAboveFloor;
	private ParticleSystem.CollisionEvent[][] collisionEvents;
	private GameObject barrel;
	private ParticleSystem[] sprinklers;
	private GameObject floor;
	
	
	void Awake ()
	{
		sprinklers = GetComponentsInChildren<ParticleSystem>();
	}
	
	
	void Start ()
	{
		heightAboveFloor = transform.position.y;
		collisionEvents = new ParticleSystem.CollisionEvent[sprinklers.Length][];
	}
	
	
	void OnParticleCollision(GameObject other)
	{

	}
	
	
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
