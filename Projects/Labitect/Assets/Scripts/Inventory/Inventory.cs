using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public string inventoryName;
	public string logName;

	public Texture image;

	public int size;

	public List<GameObject> inventory;


	public void Start() {
		if(inventory.Count > size) {
			inventory.RemoveRange(size, inventory.Count - size);
		}
	}


	public bool hasSpace() {
		return inventory.Count < size;
	}
}
