using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public string inventoryName;
	public string logName;

	public Texture image;

	public float seat;
	public float bed;
	public float toilet;
	public float storage;
	public float lamp;

	public int size;

	public Item[] initialItems;


	private List<Item> items;


	public void Start() {
		items = new List<Item>();

		// Only add the maximum amount of supported items.
		for(int index = 0; index < size; index++) {
			addItem(Instantiate(initialItems[index]));
		}

		if(initialItems.Length > size) {
			Debug.LogWarning(inventoryName + " has too many initial items.");
		}
	}


	public bool hasSpace() {
		return items.Count < size;
	}

	public bool hasItem(Item item) {
		return items.Contains(item);
	}

	public Item[] getItems() {
		return items.ToArray();
	}


	public void addItem(Item item) {
		if(hasSpace()) {
			items.Add(item);
			item.setInventory(this);
		}
		else {
			throw new InvalidOperationException("No space in " + inventoryName + " for " + item.itemName + ".");
		}
	}

	public void removeItem(Item item) {
		if(items.Contains(item)) {
			items.Remove(item);
			item.setNoInventory();
		}
		else {
			throw new InvalidOperationException(item.itemName + " not in " + inventoryName + ".");
		}
	}
}
