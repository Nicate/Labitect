using System.Collections.Generic;
using UnityEngine;

public class Inventories : MonoBehaviour {
	private List<Inventory> inventories;


	public Inventory getNearestInventory(Vector3 position) {
		Inventory[] inventories = GameObject.FindObjectsOfType<Inventory>();

		float minimumDistance = float.MaxValue;
		Inventory nearestInventory = null;

		foreach(Inventory inventory in inventories) {
			float distance = Vector3.Distance(inventory.transform.position, position);

			if(distance < minimumDistance) {
				minimumDistance = distance;
				nearestInventory = inventory;
			}
		}

		return nearestInventory;
	}
	
	public Inventory getNearestInventory(Vector3 position, Inventory exclude) {
		Inventory[] inventories = GameObject.FindObjectsOfType<Inventory>();

		float minimumDistance = float.MaxValue;
		Inventory nearestInventory = null;

		foreach(Inventory inventory in inventories) {
			if(inventory != exclude) {
				float distance = Vector3.Distance(inventory.transform.position, position);

				if(distance < minimumDistance) {
					minimumDistance = distance;
					nearestInventory = inventory;
				}
			}
		}

		return nearestInventory;
	}
}
