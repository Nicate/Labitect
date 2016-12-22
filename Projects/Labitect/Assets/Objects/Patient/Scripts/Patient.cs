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

	private bool desireToDrop;
	private bool desireToPoop;

	private bool dead;

	public Item poop;
	public Item flesh;
	public Inventory carcass;

	private Log performExperimentLog;


	public void Awake() {
		performExperimentLog = GameObject.Find("UserInterface").GetComponent<Log>();

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

		move(GameObject.FindObjectOfType<Lab>().generateRandomPosition(navMeshAgent.radius));

		StartCoroutine(behave());
	}


	private IEnumerator behave() {
		while(true) {
			yield return wait;

			if(!dead) {
				checkDestination();
			//	checkTarget();

				calm += 0.0005f;
				calm -= hunger / 1000.0f + (1.0f - health) / 1000.0f;

				fear -= 0.002f * (1.0f - getLight(transform.position));
				fear += 0.002f * getBully(transform.position);

				if(hunger > 0.1f) {
					hunger += 0.001f;
				}
				else {
					hunger -= 0.001f;
				}

				if(health > 0.2f) {
					if(hunger < 0.95f) {
						health += 0.005f;
					}
					else {
						health -= 0.02f;
					}
				}
				else {
					health -= 0.005f;
				}

				calm = Mathf.Clamp01(calm);
				fear = Mathf.Clamp01(fear);
				hunger = Mathf.Clamp01(hunger);
				health = Mathf.Clamp01(health);
			}

			checkDeath();

			if(!dead) {
				// First the destinations.
				moveToFood();
				moveToMedicin();

				// Then the gradients.
				moveToLight();
				moveToBully();
			//	moveToSmell();
			}
		}
	}


	private void checkDestination() {
		if(destination == null) {
			return;
		}

		float distance = Vector3.Distance(destination.transform.position, transform.position);

		if(distance < 1.0f) {
			if(desireToPoop) {
				if(destination.hasSpace()) {
					destination.addItem(Instantiate(poop));

					hunger += 0.25f;

					log(getName() + " pooped in " + destination.logName + ".");
				}
				else {
					log(destination.name + " is full!");
				}

				desireToPoop = false;
			}
			else if(desireToDrop) {
				if(destination.hasSpace()) {
					inventory.Remove(desire);
					destination.addItem(desire);
					
					// This can happen apparently, this is not a fix but no time to fix it!
					if(desire != null) {
						log(getName() + " put " + desire.logName + " in " + destination.logName + ".");
					}
				}
				else {
					log(destination.inventoryName + " has no space!");
				}

				desireToDrop = false;
			}
			else {
				if(hasSpace()) {
					if(destination.hasItem(desire)) {
						destination.removeItem(desire);
						inventory.Add(desire);

						log(getName() + " took " + desire.logName + " from " + destination.logName + ".");
					}
					else {
						log(destination.inventoryName + " no longer contains " + desire.logName + "!");
					}
				}
				else {
					log(getName() + " has no empty pockets!");
				}
			}

			desire = null;
			destination = null;
		}
	}


	private void checkDeath() {
		if(!dead && health < 0.01f) {
			// Patient is dead.
			dead = true;

			log(getName() + " has died!");

			GetComponent<Appearance>().playDead();

			// Hackity.
			Inventory carcassInstance = Instantiate(carcass, transform.position, Quaternion.identity, GameObject.Find("Experiment").transform) as Inventory;

			carcassInstance.addItem(Instantiate(flesh));
			carcassInstance.addItem(Instantiate(flesh));

			if(hunger < 0.1f) {
				carcassInstance.addItem(Instantiate(poop));

				log(getName() + " defecated post-death.");
			}

			// Finder in code...
			Inventory[] inventories = GameObject.FindObjectsOfType<Inventory>();
			foreach(Patient patient in GameObject.FindObjectsOfType<Patient>()) {
				patient.inventories = inventories;
			}
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


	private void moveToPut(Inventory inventory, Item item) {
		// This can happen, no time to fix it!
		if(inventory == null) {
			return;
		}

		desire = item;
		destination = inventory;
		target = null;

		desireToDrop = true;

		move(inventory.transform.position);
	}


	private void moveToFood() {
		if(hunger > 0.75) {
			// TODO steal
		}
		else if(hunger > 0.5) {
			Item bestFood = selectBestFood();

			if(bestFood == null) {
				if(hasSpace()) {
					Item food;
					Inventory inventory;

					if(getBestFood(out inventory, out food)) {
						moveToGet(inventory, food);
					}
				}
				else {
					// We need to get rid of something.
					moveToPut(getBestStorage(), selectWorstToy());
				}
			}
			else {
				eatFood(bestFood);
			}
		}
		else if(hunger > 0.25) {
			if(hasSpace()) {
				Item food;
				Inventory inventory;

				if(getBestFood(out inventory, out food)) {
					moveToGet(inventory, food);
				}
			}
		}
		else if(hunger < 0.1f) {
			moveToPoop();
		}
		else if(hunger < 0.01f) {
			// Patient has died of sepsis.
			health = 0.0f;

			log(getName() + " has sepsis!");
		}
	}

	private void eatFood(Item food) {
		// Small calm bonus.
		calm += food.food / 10.0f;

		hunger -= food.food;

		// Hack.
		if(food.itemName == "Poop") {
			health -= 0.5f;
		}

		inventory.Remove(food);

		Destroy(food);

		log(getName() + " ate " + food.logName + ".");
	}


	private void moveToPoop() {
		Inventory toilet = getBestToilet();

		if(toilet == null) {
			return;
		}

		desire = null;
		destination = toilet;
		target = null;

		desireToPoop = true;

		move(toilet.transform.position);
	}


	private void moveToMedicin() {
		if(health < 0.1) {
			// TODO steal
		}
		else if(health < 0.3) {
			Item bestMedicin = selectBestMedicin();

			if(bestMedicin == null) {
				if(hasSpace()) {
					Item medicin;
					Inventory inventory;

					if(getBestMedicin(out inventory, out medicin)) {
						moveToGet(inventory, medicin);
					}
				}
				else {
					// We need to get rid of something.
					moveToPut(getBestStorage(), selectWorstToy());
				}
			}
			else {
				useMedicin(bestMedicin);
			}
		}
		else if(health < 0.5) {
			if(hasSpace()) {
				Item medicin;
				Inventory inventory;

				if(getBestFood(out inventory, out medicin)) {
					moveToGet(inventory, medicin);
				}
			}
		}
	}

	private void useMedicin(Item medicin) {
		// Small calm bonus.
		calm += medicin.medicin / 5.0f;

		health += medicin.medicin;

		inventory.Remove(medicin);

		Destroy(medicin);

		log(getName() + " used " + medicin.logName + ".");
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


	private void moveToBully() {
		if(fear < calm) {
			// Don't care.
			return;
		}

		float desiredBully = 2.0f - Mathf.Pow(2.0f, fear);

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

			float gradient = Mathf.Abs(sample - desiredBully);

			if(gradient < bestSample) {
				bestSample = gradient;
				bestSamplePosition = samplePosition;
			}
		}

		move(bestSamplePosition);
	}
	
	private float getBully(Vector3 position) {
		float bully = 0.0f;
		int bullies = 0;

		// Finder in code...
		foreach(Patient patient in GameObject.FindObjectsOfType<Patient>()) {
			float distance = Vector3.Distance(patient.transform.position, position);

			if(distance > 0.0) {
				bully += Mathf.Clamp01(patient.strength - strength) / distance;

				if(patient.strength > strength) {
					bullies++;
				}
			}
		}

		if(bullies == 0) {
			return 0.0f;
		}
		else {
			return bully / bullies;
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
			foreach(Item item in currentInventory.initialItems) {
				// Can occur for some reason, no time to fix it!
				if(item == null) {
					continue;
				}

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

	private bool getBestMedicin(out Inventory inventory, out Item medicin) {
		float bestMedicinValue = float.MinValue;
		Item bestMedicin = null;
		Inventory bestInventory = null;

		foreach(Inventory currentInventory in inventories) {
			foreach(Item item in currentInventory.initialItems) {
				if(item.medicin > bestMedicinValue) {
					bestMedicinValue = item.medicin;
					bestMedicin = item;
					bestInventory = currentInventory;
				}
			}
		}
		
		medicin = bestMedicin;
		inventory = bestInventory;

		return bestMedicin != null;
	}


	private Item selectBestFood() {
		float bestFoodValue = float.MinValue;
		Item bestFood = null;

		foreach(Item item in inventory) {
			if(bestFoodValue < item.food) {
				bestFoodValue = item.food;
				bestFood = item;
			}
		}

		return bestFood;
	}

	private Item selectBestMedicin() {
		float bestMedicinValue = float.MinValue;
		Item bestMedicin = null;

		foreach(Item item in inventory) {
			if(bestMedicinValue < item.medicin) {
				bestMedicinValue = item.medicin;
				bestMedicin = item;
			}
		}

		return bestMedicin;
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

	private Item selectWorstToy() {
		float worstToyValue = float.MaxValue;
		Item worstToy = null;

		foreach(Item item in inventory) {
			if(worstToyValue > item.toy) {
				worstToyValue = item.toy;
				worstToy = item;
			}
		}

		return worstToy;
	}


	private Inventory getBestStorage() {
		float bestStorageValue = float.MinValue;
		Inventory bestStorage = null;

		foreach(Inventory inventory in inventories) {
			if(inventory.hasSpace()) {
				if(bestStorageValue < inventory.storage) {
					bestStorageValue = inventory.storage;
					bestStorage = inventory;
				}
			}
		}

		return bestStorage;
	}
	
	private Inventory getBestToilet() {
		float bestToiletValue = float.MinValue;
		Inventory bestToilet = null;

		foreach(Inventory inventory in inventories) {
			if(inventory.hasSpace()) {
				if(bestToiletValue < inventory.toilet) {
					bestToiletValue = inventory.toilet;
					bestToilet = inventory;
				}
			}
		}

		return bestToilet;
	}


	public bool hasSpace() {
		return inventory.Count < numberOfPockets;
	}


	private void log(string message) {
		performExperimentLog.log(message);
	}

	private string getName() {
		return GetComponent<Identity>().patientName;
	}

	private string getName(Patient patient) {
		return patient.GetComponent<Identity>().patientName;
	}
}
