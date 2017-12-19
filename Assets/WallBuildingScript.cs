using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuildingScript : MonoBehaviour 
{

	//  --------Variables-------------------------------------------------------------------------------

	public Transform player;

	GameObject Model;

	[SerializeField]
	Transform closest;

	[SerializeField]
	Collider[] nearbyColliders;

	[SerializeField]
	List<GameObject> scannedPivotList;

	public PlayerBuildingController playerBuildingController;

	[SerializeField]
	BuildingScript closestPlatform;

	BoxCollider localCollider;
	Vector3 localColliderOriginalPosition;

	public bool placing = false;

	//  --------Active Functions-------------------------------------------------------------------------------

	void Awake () 
	{
		for (int i = 0; i < transform.childCount; i++)
		{

			if (transform.GetChild(i).tag == "Model")
			{
				Model = transform.GetChild(i).gameObject;
			}
		}

		localCollider = GetComponent<BoxCollider>();

		localColliderOriginalPosition = localCollider.size;
	}
	

	void Update () 
	{
		if (placing)
		{
			CheckForSnapping();
		}

		closest = null;
		scannedPivotList = null;
		closestPlatform = null;
	}

	//  --------Sleeper Functions-------------------------------------------------------------------------------

	/// <summary>
	/// Checks for the closest point to the transform
	/// </summary>
	public Transform CheckClosest()
	{

		closest = null;

		nearbyColliders = Physics.OverlapSphere(player.position, 3.5f);

		Debug.DrawLine(player.position, transform.position);

		List<BuildingScript> platformlist = new List<BuildingScript>();

		// Scan for a nearby building
		foreach (Collider hit in nearbyColliders)
		{
			if (hit.GetComponentInParent<BuildingScript>() && hit.GetComponentInParent<BuildingScript>() != this)
			{
				platformlist.Add(hit.GetComponentInParent<BuildingScript>());
			}
		}

		float distance = 500;

		foreach (BuildingScript ps in platformlist)
		{

			if (CalculateDistance(ps.transform.position, player.position + player.forward * 3) < distance)
			{
				closestPlatform = ps;
				distance = CalculateDistance(ps.transform.position, player.position + player.forward * 3);
			}

		}

		if (closestPlatform == null)
			return null;

		scannedPivotList = closestPlatform.pivotList;

		if (scannedPivotList.Count == 0)
			return null;

		// Calculate which of the points is the closest

		distance = 500;

		foreach (GameObject point in scannedPivotList)
		{
			if (CalculateDistance(point.transform.position, player.position + player.forward * 3) < distance && point.GetComponent<PivotScript>().wallSocket == false)
			{
				closest = point.transform;
				distance = CalculateDistance(point.transform.position, player.position + player.forward * 3);
			}
		}

		if (closest == null)
			return null;


		return closest;

	}


	/// <summary>
	/// Checks for snapping, and does so if it can snap, if not it will just stay in the player's hand
	/// </summary>
	void CheckForSnapping()
	{
		if (CheckClosest() != null)
		{
			UpdatePosition(CheckClosest());
		}
		else
		{
			transform.rotation = player.rotation;
			transform.position = player.position;
		}
	}


	/// <summary>
	/// Use this to place the building, as this will also do the math for the pivots
	/// </summary>
	/// <param name="a">The transform to place @</param>
	public void Place(Transform a)
	{
		if (a != null)
		{
			SetPosition(a);

			closest.GetComponent<PivotScript>().platSocket = true;

			placing = false;
			ChangeCollider(1);
		}
		else

		{
			SetPosition(transform);
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	}

	/// <summary>
	/// This will only update the position to the player's hand
	/// </summary>
	/// <param name="a"></param>
	public void UpdatePosition(Transform a)
	{
		if (a == null)
			return;

		transform.rotation = a.rotation;
		transform.position = a.position;
	}

	/// <summary>
	/// Effectively places the building
	/// </summary>
	/// <param name="a">The transform to place it @</param>
	public void SetPosition(Transform a)
	{
		if (a == null)
			return;

		if (a != transform)
		{
			transform.parent = null;
			transform.rotation = a.rotation;
			transform.position = a.position;
			placing = false;
		}

		else

		{
			transform.parent = null;
			placing = false;
		}

	}

	/// <summary>
	/// Switches the collider from trigger to normal
	/// </summary>
	/// <param name="a">0 = True</param>
	public void ChangeCollider(int a)
	{
		if (a == 0)
		{
			localCollider.isTrigger = true;
			localCollider.size *= 0.8f;
		}
		else
		{
			localCollider.isTrigger = false;
			localCollider.size = localColliderOriginalPosition;
		}
	}

	/// <summary>
	/// Calculates the distance between 2 points
	/// </summary>
	/// <param name="a">First point</param>
	/// <param name="b">Second point</param>
	/// <returns></returns>
	public float CalculateDistance(Vector3 a, Vector3 b)
	{
		return (Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2)));
	}

}
