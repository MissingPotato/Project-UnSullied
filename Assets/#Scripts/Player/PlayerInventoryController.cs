using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryController : MonoBehaviour 
{
	#region Variables

	public GameObject inventoryGameObject;
	Vector2 inventoryPoz;
	RectTransform inventoryTransform;
	bool isHidden = false;
	#endregion


	private void Awake()
	{
		inventoryTransform = inventoryGameObject.transform.GetComponent<RectTransform>();
		inventoryPoz = inventoryTransform.position;
	}

	void FixedUpdate () 
	{

		if (Input.GetButtonDown("InventoryToggle"))
		{

			ToggleInventory();

		}

	}


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
