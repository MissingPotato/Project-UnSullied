using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour 
{

	//  --------Variables-------------------------------------------------------------------------------


	

		

	[Header("Building Proprieties")]
	public int health = 100;

	public BuildingType buildingType;

	[Space]
	[Space]

	[Header("Building Assets")]

	public Material PlacementMaterialT;
	public Material PlacementMaterialF;
	public Material OriginalMaterial;

	[Space]

	public List<GameObject> pivotList;

	public float distanceOffset = 2f;

	public PlayerBuildingController playerBuildingController;
	
	public List<Transform> SnapPointsList;

	[SerializeField]
	private List<Transform> detectedSnapPointsList;

	[SerializeField]
	Transform closest;

	public Transform player;

	GameObject Model;

	[SerializeField]
	List<GameObject> scannedPivotList;

	[SerializeField]
	BuildingScript closestPlatform;

	[SerializeField]
	Collider[] nearbyColliders;

	BoxCollider localCollider;
	Vector3 localColliderOriginalPosition;

	public float distanceToPlace = 2f;

	private bool placeable = true;

	public bool placing = false;

	//  --------Active Functions-------------------------------------------------------------------------------

	void Awake()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).tag == "SnapPoint")
			{
				pivotList.Add(transform.GetChild(i).gameObject);
			}

			if (transform.GetChild(i).tag == "Model")
			{
				Model = transform.GetChild(i).gameObject;
			}
		}

		localCollider = GetComponent<BoxCollider>();

		localColliderOriginalPosition = localCollider.size;

	}

	private void Update()
	{

		if (placing)
		{
			CheckForSnapping();
		}

		closest = null;
		scannedPivotList = null;
		closestPlatform = null;
	}

	#region collision checking

	private void OnTriggerStay(Collider other)
	{
		if (other.tag != "Ground" && other.tag != "Hand")
		{
			placeable = false;
		}

		
		CheckMode();
	}

	private void OnTriggerExit(Collider other)
	{

		if (other.tag != "Ground" && other.tag != "Hand")
		{
			placeable = true;
		}

		CheckMode();
	}

	#endregion

	//  --------Sleeper Functions-------------------------------------------------------------------------------


	/// <summary>
	/// Use this to place the building, as this will also do the math for the pivots
	/// </summary>
	/// <param name="a">The transform to place @</param>
	public void Place(Transform a)
	{
		if (a != null)
		{
			SetPosition(a);

			if ( buildingType == BuildingType.Floor )
			{
				closest.GetComponent<PivotScript>().platSocket = true;
				pivotList[2].GetComponent<PivotScript>().platSocket = true;
			}

			else if ( buildingType == BuildingType.Wall )
			{
				closest.GetComponent<PivotScript>().wallSocket = true;
				pivotList[2].GetComponent<PivotScript>().wallSocket = true;
			}

			
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
		transform.position = a.position + a.forward * 2;
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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
			transform.position = a.position + a.forward * a.GetComponentInParent<BuildingScript>().distanceToPlace;
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
			placing = false;
		}

		else

		{
			transform.parent = null;
			placing = false;
		}

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
			transform.position = player.position + player.forward * distanceOffset;
		}
	}

	/// <summary>
	/// Switches the collider from trigger to normal
	/// </summary>
	/// <param name="a">0 = True</param>
	public void ChangeCollider(int a)
	{
		if ( a == 0)
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
			if (CalculateDistance(point.transform.position, player.position + player.forward * 3) < distance && point.GetComponent<PivotScript>().platSocket == false)
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
	/// Calculates the distance between 2 points
	/// </summary>
	/// <param name="a">First point</param>
	/// <param name="b">Second point</param>
	/// <returns></returns>
	public float CalculateDistance(Vector3 a, Vector3 b)
	{
		return (Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2)));
	}

	/// <summary>
	/// Checks to change the material of the building
	/// </summary>
	public void CheckMode()
	{

		if (placing)
		{
			if (placeable)
				Model.GetComponent<Renderer>().material = PlacementMaterialT;

			else if (!placeable)
				Model.GetComponent<Renderer>().material = PlacementMaterialF;
		}

		else if (!placing)
			Model.GetComponent<Renderer>().material = OriginalMaterial;

	}

	/*

	/// <summary>
	/// Place the building
	/// </summary>
	public void Place()
	{
		PlacementMode = false;

		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).tag == "Model")
			{
				transform.GetChild(i).GetComponent<Renderer>().material = OriginalMaterial;
				
			}
		}
		

		GetComponent<BoxCollider>().isTrigger = false;
	}
	
	

	/// <summary>
	/// Follows the target
	/// </summary>
	/// <param name="_target">The target to follow</param>
	public void FollowObject(Transform _target)
	{

		if ( !PlacementMode )
			return;

		transform.position = new Vector3(Mathf.RoundToInt(_target.position.x), 0, Mathf.RoundToInt(_target.position.z)) + _target.transform.forward * distanceOffset;
		transform.rotation = _target.rotation;

		closest = null;

	}

	/// <summary>
	/// This function destroys the gameobject, should play an animation
	/// </summary>
	public void Deconstruct()
	{
	
		// Play animation
		// Reward Player half of the resources

		Destroy(gameObject);
	}

	/// <summary>
	/// Calculates the distance between 2 transforms
	/// </summary>
	/// <param name="a">The first transform</param>
	/// <param name="b">The second transform</param>
	/// <returns></returns>
	public float CalculateDistance(Transform a, Transform b)
	{
		return Mathf.Pow ( b.position.x, 2 ) - Mathf.Pow ( a.position.x, 2 ) + Mathf.Pow ( b.position.y, 2 ) - Mathf.Pow ( a.position.y, 2 ) + Mathf.Pow ( b.position.z, 2 ) - Mathf.Pow ( a.position.z, 2);
	}

	// Old scripts

	*/

}

public enum BuildingType
{	Floor,
	Wall,
	Celling,
	Door,
	DoorFrame,
}