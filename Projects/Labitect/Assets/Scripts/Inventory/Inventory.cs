using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public List<GameObject> inventory;

	public void Start() {
		inventory = new List<GameObject>();
	}
}
