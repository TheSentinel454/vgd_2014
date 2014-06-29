/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;
using InControl;

public class ComboCamera : MonoBehaviour
{
	// The target we are following
	public Transform target;
	// The distance in the x-z plane to the target
	public float minDistance = 5.0f;
	public float maxDistance = 15.0f;
	public float zoomSpeed = 5.0f;
	private float distance = 10.0f;
	public float getDistanceToPlayer(){ return distance; }
	
	/** Mouse Orbit Variables **/
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20.0f;
	public float yMaxLimit = 80.0f;
	private float x = 0.0f;
	private float y = 0.0f;
	/** END **/
	
	/** Smooth Follow Variables **/
	// The height we want the camera to be above the target
	public float height = 5.0f;
	// How much we damp
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	/** END **/

	void Start()
	{
		//InputManager.Setup ();
	}

	void LateUpdate ()
	{
		// Make sure we have a target
		if (!target)
			return;
		
		var inputDevice = InputManager.ActiveDevice;
		/*
		// Check for mouse scroll
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		// Alter the distance if there was any scrolling
		distance = Mathf.Clamp(distance + scroll * (-zoomSpeed), minDistance, maxDistance);
		
		// Mouse Orbit
		if (Input.GetMouseButton(1))
		{*/
			// Make the rigid body does not change rotation
			if (rigidbody && !rigidbody.freezeRotation)
				rigidbody.freezeRotation = true;
			if (x == 0.0f && y == 0.0f)
			{
				Vector3 angles = transform.eulerAngles;
				x = angles.y;
				y = angles.x;
			}
			if (target)
			{
				x += inputDevice.RightStickX /*Input.GetAxis("mouse x")*/ * xSpeed * 0.02f;
				y -= inputDevice.RightStickY /*Input.GetAxis("mouse y")*/ * ySpeed * 0.02f;
				
				y = ClampAngle(y, yMinLimit, yMaxLimit);
				
				Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
				Vector3 position = rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.position;
				
				transform.rotation = rotation;
				transform.position = position;
			}
		/*
		}
		// Smooth Follow
		else
		{
			// Make the rigid body can change rotation
			if (rigidbody && rigidbody.freezeRotation)
				rigidbody.freezeRotation = false;
			x = 0.0f;
			y = 0.0f;
			// Calculate the current rotation angles
			float wantedRotationAngle = target.eulerAngles.y;
			float wantedHeight = target.position.y + height;
			
			float currentRotationAngle = transform.eulerAngles.y;
			float currentHeight = transform.position.y;
			
			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
			
			// Damp the height
			currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
			
			// Convert the angle into a rotation
			Quaternion currentRotation = Quaternion.Euler (0.0f, currentRotationAngle, 0.0f);
			
			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;
			
			// Set the height of the camera
			transform.position.Set(transform.position.x, currentHeight, transform.position.z);
			
			// Always look at the target
			transform.LookAt (target);
		}
		*/
	}
	
	float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360.0f)
			angle += 360.0f;
		if (angle > 360.0f)
			angle -= 360.0f;
		return Mathf.Clamp (angle, min, max);
	}
}
