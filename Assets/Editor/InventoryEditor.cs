using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof (InventoryScript))]
[CanEditMultipleObjects]
public class InventoryEditor : Editor
{

	#region Variables

	int slotID0;
	int slotID1;

	bool showGuts = false;

	#endregion

	public override void OnInspectorGUI()
	{
		InventoryScript invScript = (InventoryScript)target;


		#region ButtonsRegion

		#region AddRI + RemoveI buttons
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();

		if (GUILayout.Button(invScript.AddItemsTexture) && Application.isPlaying)
		{
			invScript.AddRandomitem();
		}

		GUILayout.Space(10);

		if (GUILayout.Button(invScript.RemoveItemTexture) && Application.isPlaying)
		{
			invScript.ClearSlot(slotID0);
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		#endregion

		GUILayout.Space(7);

		#region Reset + Swap buttons
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		if (GUILayout.Button(invScript.ResetInventoryTexture) && Application.isPlaying)
		{
			invScript.ClearSlots();
		}

		GUILayout.Space(10);

		if (GUILayout.Button(invScript.SwapItemsDebugTexture) && Application.isPlaying)
		{
			invScript.Slot_Transfer(slotID0, slotID1);
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
		#endregion

		GUILayout.Space(7);

		#region OtherButtons

		// Momentarely Empty.

		#endregion

		#endregion

		#region Amount: 

		GUILayout.Label("Amount: " + invScript.DebugAmount);
		invScript.DebugAmount = (int)GUILayout.HorizontalSlider(invScript.DebugAmount, 0, 128);

		#endregion
		
		GUILayout.Space(5);

		#region Slot ID slider

		GUILayout.Label("Slot 1 ID: " + slotID0);
		slotID0 = (int)GUILayout.HorizontalSlider(slotID0, 0, invScript.inventorySlots.Count - 1);

		GUILayout.Label("Slot 2 ID: " + slotID1);
		slotID1 = (int)GUILayout.HorizontalSlider(slotID1, 0, invScript.inventorySlots.Count - 1);

		#endregion



		GUILayout.Space(40);



		#region Show Details

		GUILayout.BeginHorizontal();
		showGuts = GUILayout.Toggle(showGuts, "  Show Details");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.Space(40);

		if (showGuts)
			base.OnInspectorGUI();

		#endregion





	}

}