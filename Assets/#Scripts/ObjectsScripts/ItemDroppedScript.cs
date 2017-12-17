using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroppedScript : MonoBehaviour 
{
	#region Variables

	public int itemAmount;
	public Item heldItem;

	#endregion

	public int PickUp()
	{
		Destroy(gameObject);
		return itemAmount;
	}

	

}
