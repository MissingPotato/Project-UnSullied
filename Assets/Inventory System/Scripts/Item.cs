using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject 
{
	[Header("Basic Information")]
	public string itemName = "Unconfigured Item";

	public Sprite itemSprite;

	public GameObject itemObject;

	public GameObject buildObject;

	[TextArea]
	public string Description;

	public int StackSize;

	public bool StackAble;

	[Space]

	public List<ItemAtributes> itemAtributes;

	[Space]

	[Header("Type of item")]
	[Tooltip("Includes swords, potions, anything you can hold in your hand.")]
	public bool wearable;
	[Tooltip("This defines any kind of usable item, food, potions, etc..")]
	public bool useable;
	[Tooltip("This is any kind of special item, includes all the quest items")]
	public bool specialItem;
	[Tooltip("If it is a placeable object in the world!")]
	public bool placeableObject;
}


[System.Serializable]
public class ItemAtributes
{
	public string atributeName;
	public float atributePower;
	public float atributeExtra;
}