using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour 
{

	//  --------Variables-------------------------------------------------------------------------------

	#region Variables

	InventoryScript playerInventory;

	#endregion

	//  --------Active Functions-------------------------------------------------------------------------------

	void Awake()
	{
		playerInventory = transform.parent.GetComponent<InventoryScript>();
	}

	void OnTriggerStay(Collider other)
	{
		
		if ( other.GetComponent <ResourceScript> () && Input.GetMouseButtonDown (0) )
		{

			ResourceScript rs = other.GetComponent<ResourceScript>();

			rs.Gather(50);

			playerInventory.AddItem(rs.resourceType, 50);
			
		}

		if (CheckForDroppedItems(other))
			return;

	}

	//  --------Sleeper Functions-------------------------------------------------------------------------------

	bool CheckForDroppedItems(Collider other)
	{
		
		if ( other.GetComponent<ItemDroppedScript>() )
		{

			if ( Input.GetButtonDown ( "Interact" ) )
			{
				playerInventory.AddItem(other.GetComponent<ItemDroppedScript>().heldItem, other.GetComponent<ItemDroppedScript>().PickUp());
				return true;
			}
		}
		return false;
	}

}
