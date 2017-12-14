using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	#region Variables
	public Item heldItem;

	public Image slotSprite;
	public int itemAmount;
	public InventoryScript ParentedInventory;

	Image slotSpriteChild;
	Text slotText;

	#endregion


	/// <summary>
	/// This function will add the item to this slot, but if the slot is full it's going to recall the additem again
	/// </summary>
	/// <param name="_item">The added item</param>
	/// <returns></returns>
	public bool Slot_AddItem(Item _item, int _amount)
	{
		// The slot is empty!
		if ( heldItem == null)
		{
			heldItem = _item;
			itemAmount = _amount;

			UpdateSlot();
			return true;
		}
		
		// If we are aren't overadding, basically ( How much we have + adding.amount < Max Stack )
				//Same item?		  Is it stackable?	    Well, do we have space to add?
		else if ( heldItem.name == _item.name && heldItem.StackAble && itemAmount + _amount <= heldItem.StackSize )
		{
			itemAmount += _amount;

			UpdateSlot();
			return true;
		}

		// If we are trying to add more than it can hold, this will recall a function from the
		// inventory script, from where it will selfCall.
		else if ( heldItem == _item && heldItem.StackAble && itemAmount + _amount > heldItem.StackSize && itemAmount != heldItem.StackSize )
		{
			//Recall the add function using the stack difference from the inventory.
			// Stack difference = itemAmount + _item.amount - heldItem.StackSize
			if (InventoryScript.Singleton.DebugMode)
			{
				Debug.Log("The" + _item + "has been added in slot " + gameObject.name + "can't stack anymore, recalling...");
			}

			int calculus = _amount + itemAmount - _item.StackSize;
			itemAmount = heldItem.StackSize;

			UpdateSlot();

			if (InventoryScript.Singleton.DebugMode)
			{
				InventoryScript.Singleton.AddItem(_item, (calculus));
			}
			else
			{
				ParentedInventory.AddItem(_item, (calculus));
			}
				
			

			return true;
		}

		// If everything else fails!
		else
		{
			if ( InventoryScript.Singleton.DebugMode )
			{
				Debug.Log("The " + _item + "cannot be added in slot " + gameObject.name + "! Wrong type, or can't stack anymore.");
			}

			UpdateSlot();
			return false;
		}
	}

	/// <summary>
	/// This function will remove the present item from the slot, or substract the "_amount" from it, returns if it was sucesfull or not!
	/// </summary>
	/// <param name="_amount">The amount to substract from the slot</param>
	public bool Slot_RemoveItem(int _amount)
	{


		// If we are trying to substract as many items as there are.
		if ( _amount == itemAmount && heldItem != null)
		{

			if (InventoryScript.Singleton.DebugMode)
			{
				Debug.Log(" Succesfully removed " + heldItem.itemName + " from slot " + gameObject.name);
			} // Debug message to let us know that we removed the item

			heldItem = null;
			itemAmount = 0;

			UpdateSlot();
			return true;
		}


		// If we are just trying to extract less than we have.
		else if ( _amount < itemAmount)
		{
			itemAmount -= _amount;

			if (InventoryScript.Singleton.DebugMode )
			{
				Debug.Log(" Succesfully extracted " + _amount + " from the amout of " + heldItem.itemName + " from slot " + gameObject.name);
			} // Debug message to let us know that we have substracted from the total amount

			UpdateSlot();
			return true;
		}


		// If we are trying to remove more than we have, this will drop 
		else if (_amount > itemAmount)
		{
			int calculus = _amount - itemAmount;
			itemAmount = 0;
			
			if ( InventoryScript.Singleton.DebugMode )
			{
				InventoryScript.Singleton.RemoveItem ( heldItem, calculus); // Recall the script.
			}
			else
			{
				ParentedInventory.RemoveItem(heldItem, calculus); // Recall the script
			}

			UpdateSlot();
			return true;
		}

		// If it failed
		else
		{
			return false;
		}
	}

	public void Slot_Clear()
	{
		heldItem = null;
		slotSpriteChild = null;
		itemAmount = 0;

		UpdateSlot();
	}



	/// <summary>
	/// Update the slot's image and amount
	/// </summary>
	public void UpdateSlot()
	{
		if (transform.Find("Image"))
		{
			slotSpriteChild = transform.Find("Image").GetComponent<Image>();
		}
		
		if(transform.Find("ItemAmount"))
		{
			slotText = transform.Find("ItemAmount").GetComponent<Text>();
		}

		if (transform.Find("ItemAmount"))
		{
			transform.Find("ItemAmount").SetAsLastSibling();
		}

		if ( itemAmount == 0)
		{
			heldItem = null;
		}

		if (heldItem != null && slotSpriteChild != null && slotText != null )
		{
			slotSpriteChild.sprite = heldItem.itemSprite;
			slotSpriteChild.enabled = true;

			slotText.text = itemAmount.ToString();
			slotText.enabled = true;
		}

		else if ( slotSpriteChild != null )
		{
			slotSpriteChild.sprite = null;
			slotSpriteChild.enabled = false;

			slotText.text = null;
			slotText.enabled = false;
		}

	}
}
