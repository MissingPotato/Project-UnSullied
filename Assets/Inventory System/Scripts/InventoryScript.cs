using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryScript : MonoBehaviour 
{
	#region Variables

	

	public bool DebugMode = false;
	[HideInInspector]
	public int DebugAmount = 64;

	public List<Item> DebugItems = new List<Item>();

	public List<InventorySlot> inventorySlots = new List<InventorySlot>();

	[Space]
	[Space]
	[Space]

	public Texture AddItemsTexture;

	public Texture RemoveItemTexture;

	public Texture ResetInventoryTexture;

	public Texture SwapItemsDebugTexture;

	public static InventoryScript Singleton;

	#endregion

	private void Awake()
	{
		if (Singleton == null)
			Singleton = this;
		else
			Destroy(gameObject);
	}

	// This script will be updated with better "same type of item" finding soon, so that they
	// do stack and not just randomly add eachother even tho you could stack them.
	/// <summary>
	/// This function is going to add the designated "_item" to the inventory in a slot
	/// </summary>
	/// <param name="_item">The type of item you want to add</param>
	/// <param name="_amount">The amount of items to add</param>
	public bool AddItem(Item _item, int _amount)
	{

		if (_amount == 0)
			return false;

		// Loop thru all the slots
		for ( int i = 0; i < inventorySlots.Count; i++ )
		{
			// if we succesfully added the item, stop!
			if ( inventorySlots[i].Slot_AddItem ( _item, _amount ) )
			{
				if ( DebugMode )
					Debug.Log(_item.itemName + " has been sucesfully added to the " + inventorySlots[i].gameObject.name);
				return true;
			}
			
			else
			{
				
				if ( DebugMode )
				{
					Debug.Log(_item.itemName + " has not been able to fit in the " + inventorySlots[i].gameObject.name + " Moving to the next slot!");
				}
			} // This else is just for debugging

			// if not, keep going!
		}
		return false;
	}



	/// <summary>
	/// Adds an Item to a specific spot in the inventory
	/// </summary>
	/// <param name="_item">The kind of item to add</param>
	/// <param name="_amount">The amount of the item to add</param>
	/// <param name="_slotID">The slot in which the item will be added!</param>
	/// <returns></returns>
	public bool AddItemToSlot(Item _item, int _amount, int _slotID)
	{
		// Checking for any kind of waste of processing power/mistake
		if (_amount == 0 || _slotID < 0 || _slotID > inventorySlots.Count - 1 )
			return false;

		InventorySlot currentSlot = inventorySlots[_slotID]; // assigning a reference to the slot's slot script! :)

		if (currentSlot.heldItem != null && currentSlot.itemAmount + _amount >= currentSlot.heldItem.StackSize) // If we are overflowing swap the items! ( for now it just stops all together )
			return false;

		// Checking if we are adding to the same Item or not
		if (currentSlot.heldItem == _item || currentSlot.heldItem == null)
		{

			if (currentSlot.heldItem == null) // If the slot is null, we are going to make it the same type as the item we are adding
				currentSlot.heldItem = _item;

			currentSlot.itemAmount += _amount;

			return true;
		}


		return false;
	}

	/// <summary>
	/// This command will look for the "_item" and then remove an "_amount" from it, if not enough will return false.
	/// </summary>
	/// <param name="_item">Item to remove</param>
	/// <param name="_amount">Amount to substract</param>
	public void RemoveItem(Item _item, int _amount)
	{

		if (_amount == 0)
			return;

		for (int i = 0; i < inventorySlots.Count; i++)
		{

			if (inventorySlots[i].GetComponent<InventorySlot>().heldItem == _item)
			{

				if (inventorySlots[i].GetComponent<InventorySlot>().Slot_RemoveItem(_amount))
				{

					if (DebugMode)
						Debug.Log(_item.itemName + " has been sucesfully removed to the " + inventorySlots[i].gameObject.name);
					break;
				}

				
			}


			// if not, keep going!
		}
	}

	/// <summary>
	/// This command will add a random item to the next avaible spot in the inventory.
	/// </summary>
	public void AddRandomitem()
	{
		AddItem(DebugItems[Random.Range(0, DebugItems.Count)], DebugAmount);
	}

	/// <summary>
	/// This command resets/cleans a specific slot!
	/// </summary>
	/// <param name="_slotID"></param>
	public void ClearSlot(int _slotID)
	{
		inventorySlots[_slotID].Slot_RemoveItem(inventorySlots[_slotID].itemAmount);
	}

	/// <summary>
	/// This command clears all the slots and resets the inventory.
	/// </summary>
	public void ClearSlots()
	{
		foreach (InventorySlot slot in inventorySlots)
		{
			slot.Slot_Clear();
		}
	}

	/// <summary>
	/// Find a slot's gameobject by the ID
	/// </summary>
	/// <param name="ID">The slot's id?</param>
	public GameObject Slot_FindGameObject(int ID)
	{

		if (ID > inventorySlots.Count || ID < 0)
			return null;

		return inventorySlots[ID].gameObject;

	}


	/// <summary>
	/// Returns the slot's id by gameobject
	/// </summary>
	/// <param name="_slotGO"></param>
	/// <returns></returns>
	public int Slot_FindID(GameObject _slotGO)
	{

		for (int i = 0; i < inventorySlots.Count; i++)
		{

			if (inventorySlots[i].gameObject == _slotGO)
				return i;

		}
		return -1;		
	}

	/// <summary>
	/// This will swap the first slot with the second slot!
	/// </summary>
	/// <param name="iD1">This slot will get swapped with the second</param>
	/// <param name="iD2">This slot will get swapped with the first</param>
	public void Slot_Transfer (int iD1, int iD2)
	{

		// Alocate a swapper so that we can actually safely swap.
		InvetorySwapper swapper = new InvetorySwapper
		{
			heldItem = inventorySlots[iD2].heldItem,
			itemAmount = inventorySlots[iD2].itemAmount
		};

		// Switch the second slot data's with the first one's
		inventorySlots[iD2].heldItem = inventorySlots[iD1].heldItem;
		inventorySlots[iD2].itemAmount = inventorySlots[iD1].itemAmount;
		inventorySlots[iD2].UpdateSlot();

		// Switch the first slot's data with the second's
		inventorySlots[iD1].heldItem = swapper.heldItem;
		inventorySlots[iD1].itemAmount = swapper.itemAmount;
		inventorySlots[iD1].UpdateSlot();

		if (DebugMode)
			Debug.Log("Succesfully swapped " + iD1 + " slot with the " + iD2 + " slot!");
	}

}


/// <summary>
/// This is just so that we can swap in the Slot_Transfer
/// </summary>
class InvetorySwapper
{
	public Item heldItem;
	public int itemAmount;
}
