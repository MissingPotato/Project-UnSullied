using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{

//  --------Variables-------------------------------------------------------------------------------

	#region Variables
	
	public float playerBaseSpeed = 2.5f;
	public PlayerStats playerStats;
	private Rigidbody playerRb;
	private Vector3 mousePos;
	Ray camRay;
	RaycastHit floorHit;
	Vector3 playerToMouse;
	Quaternion newRotation;

	[Space]
	[Space]
	[Space]
	[Space]

	public List<string> randomPrefix = new List<string>(); // A list of random prefixes
	public List<string> randomName; // A list of random names

	#endregion

//  --------Active Functions-------------------------------------------------------------------------------

	void Awake ()
	{
		playerStats.playerName = GenerateUsername(); // Generate a name for the user.
		playerRb = GetComponent<Rigidbody>(); // Sets the player's rigidbody to playerRb
	}
	
	void FixedUpdate () 
	{
		PlayerMotor();
		LookAtMouse();
	}

//  --------Sleeper Functions-------------------------------------------------------------------------------

	/// <summary>
	/// Generates a username out of the prefixes and the randomNames
	/// </summary>
	/// <returns>Returns the username</returns>
	public string GenerateUsername()
	{
		return randomPrefix[Random.Range(0, randomPrefix.Count)] + " " + randomName[Random.Range(0, randomName.Count)];
	}

	/// <summary>
	/// Decides the player's movement
	/// </summary>
	public void PlayerMotor()
	{
		if ( Input.GetButton ("Sprint") )
		{
			playerRb.velocity = new Vector3(Input.GetAxis("Horizontal") * playerBaseSpeed * 1.5f, playerRb.velocity.y , Input.GetAxis("Vertical") * playerBaseSpeed * 1.5f );
		}
		else
		{
			playerRb.velocity = new Vector3(Input.GetAxis("Horizontal") * playerBaseSpeed, playerRb.velocity.y , Input.GetAxis("Vertical") * playerBaseSpeed );	
		}
	}

	/// <summary>
	/// Decides the player's look rotation, follows the mouse.
	/// </summary>
	public void LookAtMouse()
	{
		camRay = Camera.main.ScreenPointToRay ( Input.mousePosition ) ;

		if ( Physics.Raycast ( camRay, out floorHit ))
		{

			if (floorHit.collider.gameObject.name != "Plane")
				return;

			playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0;

			newRotation = Quaternion.LookRotation(playerToMouse);
			playerRb.MoveRotation ( newRotation ) ;

		}

		else
		{
			newRotation = Quaternion.Euler(Vector3.zero);
			transform.rotation = transform.rotation;

		}

	}

}
