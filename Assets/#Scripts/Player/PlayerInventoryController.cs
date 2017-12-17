using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryController : MonoBehaviour 
{

	//  --------Variables-------------------------------------------------------------------------------

	#region Variables

	public GameObject inventoryGameObject;
	Vector2 inventoryPoz;
	RectTransform inventoryTransform;
	bool isHidden = false;



	#endregion

	//  --------Sleeper Functions-------------------------------------------------------------------------------

	private void Awake()
	{
		inventoryTransform = inventoryGameObject.transform.GetComponent<RectTransform>();
		inventoryPoz = inventoryTransform.position;
		ToggleInventory();
	}

	void LateUpdate () 
	{

		if (Input.GetButtonDown("InventoryToggle"))
		{

			ToggleInventory();

		}

	}

	//  --------Sleeper Functions-------------------------------------------------------------------------------

	void ToggleInventory()
	{

		isHidden = !isHidden;

		if (isHidden)
		{
			inventoryTransform.position *= 50;
		}

		else

		{
			inventoryTransform.position = inventoryPoz;
		}

	}

}
