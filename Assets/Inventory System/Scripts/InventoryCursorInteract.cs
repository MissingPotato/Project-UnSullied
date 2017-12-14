using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCursorInteract : MonoBehaviour 
{
	#region Variables

	public List<RaycastResult> underCursor1; // These are all the things under our cursor when we press click (0)
	public List<RaycastResult> underCursor2; // These are all the things under our cursor when we let go of click (0)

	InventorySlot currentSlot; // This is the original slot we clicked on
	InventorySlot underSlot; // This is the slot from under the cursor when we lift our finger from the mouse's click
	InventorySlot heldSlotInv; 
	
	bool dragging = false; // This is telling us if we are dragging an item
	bool justDragged = false; // This is a variable that stops crashes/erorrs from happening :)
	bool isSplitting = false;

	Transform draggedTransform = null; // This is the dragged Object's transform
	Vector3 ogPos = Vector3.zero; // This is th eoriginal position of the Draggedobject
	GameObject generatedIcon;
	GameObject heldSlot;
	

	#endregion

	private void Awake()
	{
		#region Generated Icon Region

		generatedIcon = new GameObject();
		generatedIcon.name = "Generated Icon";
		generatedIcon.AddComponent<Image>();
		generatedIcon.transform.localScale *= 0.68f;

		#endregion

		#region Held Item

		heldSlot = new GameObject();
		heldSlot.AddComponent<InventorySlot>();
		heldSlotInv = heldSlot.GetComponent<InventorySlot>();

		#endregion

	}

	private void Update()
	{
		#region LeftClickItemMovement

		if ( Input.GetMouseButtonDown(0) && !dragging )
		{
			underCursor1 = RaycastMouse();
			foreach (RaycastResult rez in underCursor1)
			{
				if (rez.gameObject.GetComponent<InventorySlot>())
				{
					if (rez.gameObject.GetComponent<InventorySlot>().heldItem != null)
					{
						currentSlot = rez.gameObject.GetComponent<InventorySlot>();

						draggedTransform = currentSlot.transform.Find("Image").transform;

						ogPos = draggedTransform.position;

						currentSlot.transform.Find("ItemAmount").gameObject.SetActive(false);

						draggedTransform.SetParent(currentSlot.transform.parent.parent);

						dragging = true;
					}

				}
					
			}

		}


		else if (Input.GetMouseButtonDown(0) && dragging )
		{
			underCursor2 = RaycastMouse();
			foreach (RaycastResult rez in underCursor2)
			{
				if ( rez.gameObject.GetComponent<InventorySlot>() )
				{
					if ( rez.gameObject.GetComponent<InventorySlot>() != currentSlot )
					{

						underSlot = rez.gameObject.GetComponent<InventorySlot>();

						// If we can only swap the items
						if ( currentSlot.heldItem != underSlot.heldItem && !isSplitting || underSlot.heldItem == null && !isSplitting)
						{


							if (currentSlot.transform.Find("ItemAmount"))
							{
								currentSlot.transform.Find("ItemAmount").gameObject.SetActive(true);
							}

							underSlot.ParentedInventory.Slot_Transfer(underSlot.ParentedInventory.Slot_FindID(underSlot.gameObject), currentSlot.ParentedInventory.Slot_FindID(currentSlot.gameObject));

						}


						// If we can stack the items
						else if (currentSlot.itemAmount > 0 && underSlot.heldItem == currentSlot.heldItem || underSlot.heldItem == null)
						{

							if (underSlot.heldItem == null)
							{
								underSlot.ParentedInventory.AddItemToSlot(currentSlot.heldItem, currentSlot.itemAmount, underSlot.ParentedInventory.Slot_FindID(underSlot.gameObject));
								currentSlot.Slot_Clear();

								dragging = false;
							}

							// If we can stack, and we are not oveflowing, we will just add the items in hand to the slot we are clicking.
							else if (underSlot.itemAmount + currentSlot.itemAmount <= underSlot.heldItem.StackSize)
							{

								underSlot.ParentedInventory.AddItemToSlot(currentSlot.heldItem, currentSlot.itemAmount, underSlot.ParentedInventory.Slot_FindID(underSlot.gameObject));
								currentSlot.Slot_Clear();

								dragging = false;

							}

							else

							// If we are trying to add the items, but it is overflowing.
							{

								int calculator = underSlot.itemAmount + currentSlot.itemAmount - underSlot.heldItem.StackSize;

								underSlot.itemAmount = underSlot.heldItem.StackSize;

								currentSlot.itemAmount = calculator;

							}


							// Update the slots
							currentSlot.UpdateSlot();
							underSlot.UpdateSlot();
						}

					}
				}
				
				else

				{
					if ( !isSplitting )
						dragging = false;
				}

			}
			
		}
		//
		#endregion

		#region RightClickSplitDragging

		// If we are holding an item ( dragging ), then we can place one item for each right-click in an empty slot
		if ( Input.GetMouseButtonDown(1) && dragging)
		{

			underCursor2 = RaycastMouse();
			foreach (RaycastResult rez in underCursor2)
			{
				if (rez.gameObject.GetComponent<InventorySlot>() && dragging)
				{
					if (rez.gameObject.GetComponent<InventorySlot>() != currentSlot)
					{

						underSlot = rez.gameObject.GetComponent<InventorySlot>();

						if (currentSlot.itemAmount > 0 && underSlot.heldItem == currentSlot.heldItem || underSlot.heldItem == null)
						{
							underSlot.ParentedInventory.AddItemToSlot(currentSlot.heldItem, 1, underSlot.ParentedInventory.Slot_FindID(underSlot.gameObject));
							currentSlot.itemAmount -= 1;

							currentSlot.UpdateSlot();
							underSlot.UpdateSlot();
						}
					}
				}

				if (currentSlot.itemAmount < 1)
				{
					dragging = false;
					return;
				}
					
			}

		}
		
		// If we are not holding an item ( !dragging ), then we can split the stack in half and place it somewhere else.
		if ( Input.GetMouseButtonDown(1) && !dragging)
		{

			underCursor1 = RaycastMouse();
			foreach (RaycastResult rez in underCursor1)
			{
				if (rez.gameObject.GetComponent<InventorySlot>())
				{
					currentSlot = rez.gameObject.GetComponent<InventorySlot>();

					if (currentSlot.itemAmount == 1)
						return;

					
					
					// draggedTransform = currentSlot.transform.Find("Image").transform;

					generatedIcon.GetComponent<Image>().sprite = currentSlot.heldItem.itemSprite;
					generatedIcon.GetComponent<Image>().enabled = true;

					heldSlotInv.itemAmount = currentSlot.itemAmount / 2; // set the held item's item amount = current's amount
					currentSlot.itemAmount -= heldSlotInv.itemAmount; // Set the current item's amount

					heldSlotInv.heldItem = currentSlot.heldItem;
					

					draggedTransform = generatedIcon.transform;

					ogPos = draggedTransform.position;

					draggedTransform.SetParent(currentSlot.transform.parent.parent);

					currentSlot.UpdateSlot();

					currentSlot = heldSlotInv;

					isSplitting = true;

					dragging = true;


				}

			}



		}


		#endregion

	}

	private void LateUpdate()
	{

		if (dragging)
		{
			draggedTransform.position = Input.mousePosition;
			justDragged = true;
		} // While we are dragging

		else if (!dragging && justDragged)
		{
			isSplitting = false;

			generatedIcon.GetComponent<Image>().enabled = false;
			draggedTransform.position = ogPos;
			draggedTransform.SetParent(currentSlot.transform);

			if (currentSlot != null)
			{
				currentSlot.UpdateSlot();
			} // Update slot if you can

			if (underSlot != null)
			{
				underSlot.UpdateSlot();
			} // Update slot if you can

			if (currentSlot.transform.Find("ItemAmount"))
			{
				currentSlot.transform.Find("ItemAmount").gameObject.SetActive(true); // Show the amount of items in the first slot.
			}

			underSlot = null;
			currentSlot = null;

			justDragged = false;
		} // The frame we let go in.
	}

	/// <summary>
	/// This function will return all the UI elements underneath the cursor
	/// </summary>
	/// <returns></returns>
	public List<RaycastResult> RaycastMouse()
	{

		PointerEventData pointerData = new PointerEventData(EventSystem.current)
		{
			pointerId = -1,
		};

		pointerData.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult>();

		EventSystem.current.RaycastAll(pointerData, results);

		return results;
	}

}