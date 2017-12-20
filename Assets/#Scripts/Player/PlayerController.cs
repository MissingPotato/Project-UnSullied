using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{

//  --------Variables-------------------------------------------------------------------------------

	#region Variables
	
	public PlayerStats playerStats;
	private Rigidbody playerRb;
	private Vector3 mousePos;
	[SerializeField]
	private bool isColiding;
	Ray camRay;
	RaycastHit floorHit;
	Vector3 playerToMouse;
	Quaternion newRotation;

	public Transform playerHand;

	private float nextJump = 0;

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

		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).tag == "Hand")
			{
				playerHand = transform.GetChild(i);
			}
		}
	}
	
	void Update () 
	{
		LookAtMouse();
	}

	private void FixedUpdate()
	{
		PlayerMotor();
	}

	private void OnCollisionEnter(Collision collision)
	{
		isColiding = true;
	}



	private void OnDrawGizmos()
	{
		if (playerHand != null)
		{
			Gizmos.DrawWireSphere(playerHand.position, 3f);
		}
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


		if (Input.GetButton("Sprint"))
		{
			playerRb.MovePosition(new Vector3(transform.position.x + Input.GetAxis("Horizontal")* playerStats.walkSpeed * 1.2f * Time.fixedDeltaTime, transform.position.y, transform.position.z + (Input.GetAxis("Vertical")) * playerStats.walkSpeed * 1.2f * Time.fixedDeltaTime));
		}
		else
		{
			playerRb.MovePosition(new Vector3(transform.position.x + Input.GetAxis("Horizontal") * playerStats.walkSpeed * Time.fixedDeltaTime, transform.position.y, transform.position.z + (Input.GetAxis("Vertical")) * playerStats.walkSpeed * Time.fixedDeltaTime));
		}


		if (Input.GetButtonDown("Jump") && isColiding && nextJump <= Time.timeSinceLevelLoad)
		{
			playerRb.velocity += new Vector3(playerRb.velocity.x * 0.3f, playerStats.jumpPower, playerRb.velocity.z * 0.3f);

			nextJump = Time.timeSinceLevelLoad + .5f;

			isColiding = false;
		}

	}

	/// <summary>
	/// Decides the player's look rotation, follows the mouse.
	/// </summary>
	public void LookAtMouse()
	{
		camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll(camRay);

		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.gameObject.tag == "Ground")
			{
				playerToMouse = hit.point - transform.position;
				playerToMouse.y = 0;
				newRotation = Quaternion.LookRotation(playerToMouse);
				playerRb.MoveRotation(newRotation);
			}
		}

	}



}



/* Old scripts
	public void PlayerMotor()
	{
		if ( Input.GetButton ("Sprint") )
		{
			playerRb.velocity = new Vector3(Input.GetAxis("Horizontal") * (playerStats.walkSpeed * 1.5f), playerRb.velocity.y , Input.GetAxis("Vertical") * (playerStats.walkSpeed * 1.5f) );
		}
		else
		{
			playerRb.velocity = new Vector3(Input.GetAxis("Horizontal") * playerStats.walkSpeed, playerRb.velocity.y , Input.GetAxis("Vertical") * playerStats.walkSpeed);	
		}

		if ( Input.GetButtonDown ("Jump") && isColiding && nextJump <= Time.timeSinceLevelLoad )
		{
			playerRb.velocity += new Vector3(playerRb.velocity.x * 0.3f, playerStats.jumpPower, playerRb.velocity.z * 0.3f);

			nextJump = Time.timeSinceLevelLoad + .5f;

			isColiding = false;
		}

	}

	public void PlayerMotor2()
	{

		
		if (Input.GetButton("Sprint"))
		{
			transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * playerStats.walkSpeed * 1.2f * Time.fixedDeltaTime, transform.position.y, transform.position.z + (Input.GetAxis("Vertical")) * playerStats.walkSpeed * 1.2f * Time.fixedDeltaTime);
		}
		else
		{
			transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * playerStats.walkSpeed * Time.fixedDeltaTime, transform.position.y, transform.position.z + (Input.GetAxis("Vertical")) * playerStats.walkSpeed * Time.fixedDeltaTime);
		}


		if (Input.GetButtonDown("Jump") && isColiding && nextJump <= Time.timeSinceLevelLoad)
		{
			playerRb.velocity += new Vector3(playerRb.velocity.x * 0.3f, playerStats.jumpPower, playerRb.velocity.z * 0.3f);

			nextJump = Time.timeSinceLevelLoad + .5f;

			isColiding = false;
		}

	}
*/