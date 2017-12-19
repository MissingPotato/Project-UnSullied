using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingController : MonoBehaviour 
{
	//  --------Variables-------------------------------------------------------------------------------

	GameObject playerHand;
	GameObject playerModel;
	GameObject placingBuilding;
	BuildingScript placingBuildingScript;

	[SerializeField]
	bool isBuilding = false;

	[Space]
	[Header("Debugging")]
	public bool debugMode = false;
	public GameObject debugBuilding;

	//  --------Active Functions-------------------------------------------------------------------------------

	private void Awake()
	{
		InitReferences();
	}

	private void Update()
	{
		
		if (Input.GetKeyDown ( KeyCode.Alpha1 ) && debugMode && !isBuilding )
		{
			isBuilding = true;
			StartBuilding ( debugBuilding );
		}

		if (Input.GetMouseButtonDown (0) && debugMode && isBuilding )
		{
			isBuilding = false;
			
			if ( placingBuilding.GetComponent<WallBuildingScript>())
			{
				placingBuilding.GetComponent<WallBuildingScript>().Place(placingBuilding.GetComponent<WallBuildingScript>().CheckClosest());
			}

			else

			{
				placingBuildingScript.Place(placingBuildingScript.CheckClosest());
			}

		}

	}

	//  --------Sleeper Functions-------------------------------------------------------------------------------


	/// <summary>
	/// Makes the player start placing a building.
	/// </summary>
	/// <param name="_building">The building's gameobject</param>
	/// <returns>if it was sucesfully placed or not</returns>
	public void StartBuilding(GameObject _building)
	{

		if (_building.GetComponent<BuildingScript>())
		{
			placingBuilding = Instantiate(_building, playerHand.transform);
			placingBuilding.transform.SetParent(null); // <--------------
			placingBuildingScript = placingBuilding.GetComponent<BuildingScript>();
			placingBuildingScript.placing = true;
			placingBuildingScript.ChangeCollider(0);
			placingBuilding.GetComponent<BuildingScript>().playerBuildingController = this;
			placingBuildingScript.player = this.transform;
		}

		else if ( _building.GetComponent<WallBuildingScript>() )
		{
			placingBuilding = Instantiate(_building, playerHand.transform);
			placingBuilding.transform.SetParent(null); // <--------------
			placingBuilding.GetComponent<WallBuildingScript>().placing = true;
			placingBuilding.GetComponent<WallBuildingScript>().ChangeCollider(0);
			placingBuilding.GetComponent<WallBuildingScript>().playerBuildingController = this;
			placingBuilding.GetComponent<WallBuildingScript>().player = this.transform;
		}


	}

	/// <summary>
	/// Initializes the references for the differite variables
	/// </summary>
	void InitReferences()
	{
		for (int i = 0; i < transform.childCount; i++)
		{

			if ( transform.GetChild(i).tag == "Hand" )
			{
				playerHand = transform.GetChild(i).gameObject;
			}

			if ( transform.GetChild(i).tag == "Model" )
			{
				playerModel = transform.GetChild(i).gameObject;
			}



		}
	}



}
