using UnityEngine;

public class Item : MonoBehaviour {
	public string itemName;
	public string logName;

	public Texture image;

	public float food;
	public float smell;
	public float weapon;
	public float toy;
	public float medicin;
	
	private Inventory inventory;


	public bool hasInventory() {
		return inventory != null;
	}

	public Inventory getInventory() {
		return inventory;
	}


	internal void setInventory(Inventory inventory) {
		this.inventory = inventory;
	}

	internal void setNoInventory() {
		inventory = null;
	}
}
