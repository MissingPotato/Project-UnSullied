using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	#region Variables
	
	public float playerBaseSpeed = 2.5f;
	public PlayerStats playerStats;
	private Rigidbody playerRb;


	[Space]
	[Space]
	[Space]
	[Space]

	public List<string> randomPrefix = new List<string>();
	public List<string> randomName;

	#endregion

	void Awake () 
	{
		playerStats.playerName = GenerateUsername(); // Generate a name for the user.
		for (int i = 0; i < 50; i++)
		{
			Debug.Log(GenerateUsername());
		}
		playerRb = GetComponent<Rigidbody>();
	}
	

	void FixedUpdate () 
	{
		PlayerMotor();
		

	}

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


}
