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
		*/
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
			x += inputDevice.RightStickX * xSpeed * 0.02f;
			y -= inputDevice.RightStickY * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);
				
			Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
			Vector3 position = rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.position;
				
			transform.rotation = rotation;
			transform.position = position;
		}
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
