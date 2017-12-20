using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour 
{

	//  --------Variables-------------------------------------------------------------------------------

	float buildingDistance = 3f;

	[Header("Building Settings")]
	[Space]

	public BuildingType buildingType;


	[Space]
	[Space]

	[Header("Building Proprieties")]
	[Space]

	[Tooltip("The owner's transform reference.")]
	public Transform player;

	[Tooltip("The snappoints list")]
	public List<Transform> SnapPoints;
	[Tooltip("The buidling model")]
	public Transform Model;

	[Tooltip("If we are still placing the building")]
	public bool isPlacing = true;

	[Tooltip("The scanned nearby points")]
	public List<Transform> scannedPoints;

	[SerializeField]
	List<BuildingScript> nearbyBuildings;

	[SerializeField]
	Transform closestSnapPoint;

	[SerializeField]
	BuildingScript closestBuilding;

	RaycastHit hitInfo;

	Transform playerHand;

	//  --------Active Functions-------------------------------------------------------------------------------

	void Awake () 
	{
		Initialize();
	}
	
	void Update () 
	{
		
		if ( isPlacing )
		{
			UpdatePosition();
		}

	}
	
	//  --------Sleeper Functions-------------------------------------------------------------------------------
	
	/// <summary>
	/// Places the building
	/// </summary>
	public void Place()
	{

		Transform chekd = CheckForSnapping();

		if ( chekd != null)
		{

			PivotScript pivotScript = chekd.GetComponent<PivotScript>();

			if (buildingType == BuildingType.Wall && pivotScript.wallSocket == false )
			{

				transform.parent = null;
				transform.position = chekd.position;
				transform.rotation = chekd.rotation;

				
				pivotScript.wallSocket = true;
				isPlacing = false;
			}
				
			else if (buildingType == BuildingType.Foundation && pivotScript.platSocket == false )
			{

				transform.parent = null;
				transform.position = chekd.position + chekd.forward * 2f;
				transform.rotation = chekd.rotation;

				SnapPoints[2].GetComponent<PivotScript>().platSocket = true;

				pivotScript.platSocket = true;
				isPlacing = false;
			}
				

			
		}
		else
		{

			if (Physics.Raycast(transform.position, -transform.up * 3, out hitInfo) && buildingType == BuildingType.Foundation )
			{
				transform.parent = null;
				transform.position = new Vector3( transform.position.x , hitInfo.point.y , transform.position.z );
				isPlacing = false;
			}

		}
	}

	/// <summary>
	/// Updates the building's position, includes snapping
	/// </summary>
	public void UpdatePosition()
	{

		Transform chekd = CheckForSnapping();

		if (chekd == null ) // Snap to the player if we can't snap to a point
		{
			transform.position = player.transform.position + player.transform.forward * 3;
			transform.rotation = player.transform.rotation;
		}
		else if (chekd != null )
		{

			if (buildingType == BuildingType.Foundation)
				transform.position = chekd.position + chekd.forward * 2f;
			else
				transform.position = chekd.position;

			transform.rotation = chekd.rotation;
			Debug.DrawLine(player.position, transform.position, Color.red, 0, false);
		}
		
	}

	/// <summary>
	/// Checks for the closest snapping point
	/// </summary>
	/// <returns>Closest snapping point</returns>
	public Transform CheckForSnapping()
	{
		closestSnapPoint = null;
		nearbyBuildings.Clear();
		closestBuilding = null;

		playerHand = player.GetComponent<PlayerController>().playerHand;

		if (playerHand == null)
			return null;
		
		// Check for nearby colliders.
		Collider[] nearbyColliders = Physics.OverlapSphere(playerHand.position , buildingDistance );

		// Scan for the buildings
		foreach (Collider hit in nearbyColliders)
		{
			if ( hit.GetComponent<BuildingScript>() && hit.GetComponent<BuildingScript>() != this )
			{
				
				if (buildingType == BuildingType.Foundation)
				{
					if (buildingType == hit.GetComponent<BuildingScript>().buildingType)
					{
						nearbyBuildings.Add(hit.GetComponent<BuildingScript>());
					}
				}

				

				else if ( buildingType == BuildingType.Wall )
				{
					nearbyBuildings.Add(hit.GetComponent<BuildingScript>());
				}

			}
		}
		
		float closestDistance = 500f;

		

		// Check for the closest building
		foreach (var building in nearbyBuildings)
		{
			if ( Vector3.Distance (playerHand.position, building.transform.position ) < closestDistance && building != this)
			{
				closestBuilding = building;
				closestDistance = Vector3.Distance(playerHand.position, building.transform.position);
			}
		}

		closestDistance = 500f;

		if (closestBuilding == null)
			return null;

		scannedPoints = closestBuilding.SnapPoints;

		// Scan for the closest snappingPoint
		foreach (Transform point in scannedPoints)
		{
			
			if ( buildingType == BuildingType.Foundation ) // If we are placing a foundation
			{
				if (point.GetComponent<PivotScript>().platSocket == false)
				{
					if (Vector3.Distance(playerHand.position, point.position) < closestDistance)
					{
						closestDistance = Vector3.Distance(player.GetComponent<PlayerController>().playerHand.position, point.position);
						closestSnapPoint = point;
					}
				}
			}

			else

			{
				if ( buildingType == BuildingType.Wall ) // if we are placing a wall
				{
					if (point.GetComponent<PivotScript>().wallSocket == false)
					{
						if (Vector3.Distance(player.GetComponent<PlayerController>().playerHand.position, point.position) < closestDistance)
						{
							closestDistance = Vector3.Distance(player.GetComponent<PlayerController>().playerHand.position, point.position);
							closestSnapPoint = point;
						}
					}
				}
			}

		}



		if (closestSnapPoint != null)
			return closestSnapPoint;
		else
			return null;
	}

	/// <summary>
	/// Initializes the script
	/// </summary>
	void Initialize()
	{

		for (int i = 0; i < transform.childCount; i++)
		{
			if ( transform.GetChild(i).tag == "SnapPoint" )
			{
				SnapPoints.Add( transform.GetChild (i) );
			}

			if ( transform.GetChild(i).tag == "Model" )
			{
				Model = transform.GetChild(i);
			}
		}

		

	}


}



public enum BuildingType
{
	Foundation, // 0
	Wall, // 1
	Floor // 2
}