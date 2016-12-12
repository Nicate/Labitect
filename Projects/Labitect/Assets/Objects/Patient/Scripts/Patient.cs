using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour {
	public float speed;
	public float strength;
	public float covet;

	public float calm;
	public float fear;
	public float hunger;
	public float health;
	
	/// <summary>
	/// Pocket 0 is what they have in their hands.
	/// </summary>
	public int numberOfPockets;

	public List<GameObject> inventory;

	private Inventory[] inventories;

	private WaitForSeconds wait = new WaitForSeconds(0.1f);


	public void Awake() {
		inventories = GameObject.FindObjectsOfType<Inventory>();

		// Generate these here so scripts can use their values in Start().
		speed = Random.Range(0.0f, 1.0f);
		strength = Random.Range(0.0f, 1.0f);
		covet = Random.Range(0.0f, 1.0f);

		numberOfPockets = Random.Range(0, 3);
	}

	public void Start() {
		calm = Random.Range(0.0f, 0.1f);
		fear = Random.Range(0.0f, 1.0f);
		hunger = Random.Range(0.0f, 0.5f);
		health = Random.Range(0.8f, 1.0f);

		NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

		float spry = Mathf.Pow(2.0f, speed);

		navMeshAgent.speed = navMeshAgent.speed * spry;
		navMeshAgent.angularSpeed = navMeshAgent.angularSpeed * spry;

		StartCoroutine(behave());
	}


	private IEnumerator behave() {
		while(true) {
			yield return wait;

			hunger -= 0.1f;

			if(hunger < 0.5) {
				Inventory inventory = getBestInventoryForFood();
				move(inventory.transform.position);
			}
		}
	}


	public void move(Vector3 position) {
		GetComponent<NavMeshAgent>().destination = position;
	}


	private Inventory getBestInventoryForFood() {
		float bestFoodValue = float.MinValue;
		Item bestFood = null;
		Inventory bestInventory = null;

		foreach(Inventory inventory in inventories) {
			foreach(Item item in inventory.inventory) {
				if(item.food > bestFoodValue) {
					bestFoodValue = item.food;
					bestFood = item;
					bestInventory = inventory;
				}
			}
		}
		
		return bestInventory;
	}


	public bool hasSpace() {
		return inventory.Count < numberOfPockets;
	}
}
