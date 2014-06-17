using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	void OnTriggerStay(Collider other)
	{
		Debug.Log ("OnTriggerStay: " + other.tag);
	}
	
	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("OnTriggerEnter: " + other.tag);
	}
}
