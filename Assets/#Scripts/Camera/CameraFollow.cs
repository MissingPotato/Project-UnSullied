using UnityEngine;

public class CameraFollow : MonoBehaviour {

	#region Variables

	[Header("Settings")]

	[Tooltip("This will enable/disable the camera's bump effect")]
	public bool cameraBounce = true;

	[Space]

	public int constrainZ = 17;

	public int constrainY = 15;

	[Space]

	[Range(0f, 10f)]
	public float smoothSpeed = 7f;
	
	[Space]

	[Tooltip("This wiil adjust the camera's position")]
	public Vector3 offSet;
	[Space]
	[Space]
	[Space]
	[Space]
	[Space]

	[Header("References")]

	[Tooltip("This is the target that the camera will follow")]
	public Transform target;

	
	#endregion

	void FixedUpdate()
	{

		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			if (offSet.y < constrainY && offSet.y > 3.5f)
			{
				offSet.y += Input.GetAxis("Mouse ScrollWheel") * -3;
				if (Input.GetKey(KeyCode.LeftShift))
				{
					offSet.y += Input.GetAxis("Mouse ScrollWheel") * -3;
				}
			}
				
			if(-offSet.z < constrainZ && -offSet.z > 1)
			{
				offSet.z += Input.GetAxis("Mouse ScrollWheel") * 3;
				if (Input.GetKey(KeyCode.LeftShift))
				{
					offSet.z += Input.GetAxis("Mouse ScrollWheel") * 3;
				}

			}
				

			if (offSet.y > constrainY)
				offSet.y = constrainY - 0.1f;

			if (offSet.y < 3.5f)
				offSet.y = 3.51f;


			if (-offSet.z > constrainZ)
				offSet.z = -constrainZ;

			if (-offSet.z < 2)
				offSet.z = -2f;
		} // Scrolling

		Vector3 desiredPosition = target.position + offSet;

		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);//Smoothing

		transform.position = smoothedPosition;

		if (cameraBounce)
			transform.LookAt(target);
	}

}
