using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementDebug : MonoBehaviour 
{
	//  --------Variables-------------------------------------------------------------------------------
	[SerializeField]
	Transform playerHand;
	public GameObject debugBuilding;

	GameObject placingBuilding;

	//  --------Active Functions-------------------------------------------------------------------------------

	void Awake () 
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if ( transform.GetChild(i).tag == "Hand" )
			{
				playerHand = transform.GetChild(i);
			}
		}
	}
	

	void Update () 
	{
		
		if ( Input.GetKeyDown(KeyCode.E))
		{
			placingBuilding = Instantiate(debugBuilding, playerHand.transform);
			placingBuilding.transform.position = placingBuilding.transform.position + placingBuilding.transform.forward * 3;
			placingBuilding.GetComponent<BuildingScript>().isPlacing = true;
			placingBuilding.GetComponent<BuildingScript>().player = this.transform;
		}

		if ( Input.GetMouseButtonDown (0) )
		{
			placingBuilding.GetComponent<BuildingScript>().Place();
		}

	}
	
	//  --------Sleeper Functions-------------------------------------------------------------------------------
	
	
	
}
