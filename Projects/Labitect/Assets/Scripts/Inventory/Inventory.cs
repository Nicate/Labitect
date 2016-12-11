using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public List<GameObject> inventory;

	public int maximumSize;


	public void Start() {
		if(inventory.Count > maximumSize) {
			inventory.RemoveRange(maximumSize, inventory.Count - maximumSize);
		}
	}
}
