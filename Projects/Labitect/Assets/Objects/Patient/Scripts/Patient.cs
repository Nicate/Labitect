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

	public List<Item> inventory;

	private Inventory[] inventories;

	private WaitForSeconds wait = new WaitForSeconds(0.1f);

	private Item desire;
	private Inventory destination;
	private Patient target;


	public void Awake() {
		inventories = GameObject.FindObjectsOfType<Inventory>();

		// Generate these here so scripts can use their values in Start().
		speed = Random.Range(0.0f, 1.0f);
		strength = Random.Range(0.0f, 1.0f);
		covet = Random.Range(0.0f, 1.0f);

		numberOfPockets = Random.Range(0, 3);
	}

	public void Start() {
		calm = Random.Range(0.9f, 1.0f);
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

			hunger += 0.001f;

			fear -= 0.002f * (1.0f - getLight(transform.position));

			calm = Mathf.Clamp01(calm);
			fear = Mathf.Clamp01(fear);
			hunger = Mathf.Clamp01(hunger);
			health = Mathf.Clamp01(health);

			// First the destinations.
			moveToFood();

			// Then the gradients.
			moveToLight();
		}
	}


	private void moveToGet(Inventory inventory, Item item) {
		desire = item;
		destination = inventory;
		target = null;

		move(inventory.transform.position);
	}
	
	private void moveToGet(Patient patient, Item item) {
		desire = item;
		destination = null;
		target = patient;

		move(patient.transform.position);
	}


	private void moveToFood() {
		if(hunger > 0.25 && hasSpace()) {
			Item food;
			Inventory inventory;
			if(getBestFood(out inventory, out food)) {
				moveToGet(inventory, food);
			}
		}
	}


	private void moveToLight() {
		if(fear < calm) {
			// Don't care.
			return;
		}

		float desiredLight = 2.0f - Mathf.Pow(2.0f, fear);

		float bestSample = float.MaxValue;
		Vector3 bestSamplePosition = transform.position;

		for(int count = 0; count < 9; count++) {
			float angle = count * 360.0f / 8.0f;
			
			Vector3 samplePosition = transform.position + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;

			// Hackity.
			if(count == 8) {
				samplePosition = transform.position;
			}

			float sample = getLight(samplePosition);

			float gradient = Mathf.Abs(sample - desiredLight);

			if(gradient < bestSample) {
				bestSample = gradient;
				bestSamplePosition = samplePosition;
			}
		}

		move(bestSamplePosition);
	}
	
	private float getLight(Vector3 position) {
		float light = 0.0f;
		int lights = 0;

		foreach(Inventory inventory in inventories) {
			float distance = Vector3.Distance(inventory.transform.position, position);

			if(distance > 0.0) {
				light += inventory.lamp / distance;

				if(inventory.lamp > 0.0f) {
					lights++;
				}
			}
		}

		if(lights == 0) {
			return 0.0f;
		}
		else {
			return light / lights;
		}
	}


	public void move(Vector3 position) {
		GetComponent<NavMeshAgent>().destination = position;
	}


	private bool getBestFood(out Inventory inventory, out Item food) {
		float bestFoodValue = float.MinValue;
		Item bestFood = null;
		Inventory bestInventory = null;

		foreach(Inventory currentInventory in inventories) {
			foreach(Item item in currentInventory.inventory) {
				if(item.food > bestFoodValue) {
					bestFoodValue = item.food;
					bestFood = item;
					bestInventory = currentInventory;
				}
			}
		}
		
		food = bestFood;
		inventory = bestInventory;

		return bestFood != null;
	}


	private Item selectBestWeapon() {
		float bestWeaponValue = float.MinValue;
		Item bestWeapon = null;

		foreach(Item item in inventory) {
			if(bestWeaponValue < item.weapon) {
				bestWeaponValue = item.weapon;
				bestWeapon = item;
			}
		}

		return bestWeapon;
	}


	public bool hasSpace() {
		return inventory.Count < numberOfPockets;
	}
}
