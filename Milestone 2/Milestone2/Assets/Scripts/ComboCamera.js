// The target we are following
var target : Transform;
// The distance in the x-z plane to the target
var distance = 10.0;

/** Mouse Orbit Variables **/
var xSpeed = 250.0;
var ySpeed = 120.0;
var yMinLimit = -20;
var yMaxLimit = 80;
private var x = 0.0;
private var y = 0.0;
/** END **/

/** Smooth Follow Variables **/
// The height we want the camera to be above the target
var height = 5.0;
// How much we damp
var heightDamping = 2.0;
var rotationDamping = 3.0;
/** END **/

// Place the script in the Camera-Control group in the component menu
@script AddComponentMenu("Camera-Control/Combo Camera")


function LateUpdate ()
{
	// Make sure we have a target
	if (!target)
		return;
		
	// Mouse Orbit
	if (Input.GetMouseButton(1))
	{
		// Make the rigid body does not change rotation
	   	if (rigidbody && !rigidbody.freezeRotation)
			rigidbody.freezeRotation = true;
		if (x == 0.0 && y == 0.0)
		{
			var angles = transform.eulerAngles;
		    x = angles.y;
		    y = angles.x;
		}
	    if (target)
	    {
	        x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
	        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
	 		
	 		y = ClampAngle(y, yMinLimit, yMaxLimit);
	 		       
	        var rotation = Quaternion.Euler(y, x, 0);
	        var position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
	        
	        transform.rotation = rotation;
	        transform.position = position;
	    }
	}
	// Smooth Follow
	else
	{
    	// Make the rigid body can change rotation
	   	if (rigidbody && rigidbody.freezeRotation)
			rigidbody.freezeRotation = false;
		x = 0.0;
		y = 0.0;
		// Calculate the current rotation angles
		var wantedRotationAngle = target.eulerAngles.y;
		var wantedHeight = target.position.y + height;
			
		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		transform.position.y = currentHeight;
		
		// Always look at the target
		transform.LookAt (target);
	}
}

static function ClampAngle (angle : float, min : float, max : float)
{
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}