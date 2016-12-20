using UnityEngine;

public class Entrance : MonoBehaviour {
	/// <summary>
	/// The prefab that is cloned to spawn the patients.
	/// </summary>
	public GameObject patient;

	/// <summary>
	/// The range of the number of patients to spawn.
	/// </summary>
	public Range numberOfPatients;

	/// <summary>
	/// The size of the entrance.
	/// </summary>
	public Size size;


	/// <summary>
	/// Spawns a number of patients within the entrance.
	/// </summary>
	public void spawnPatients() {
		int numberofPatients = Random.Range(numberOfPatients.minimum, numberOfPatients.maximum + 1);

		for(int count = 0; count < numberofPatients; count++) {
			GameObject patientInstance = Instantiate(patient, transform.parent) as GameObject;

			NavMeshAgent navMeshAgent = patientInstance.GetComponent<NavMeshAgent>();

			patientInstance.transform.position = generateRandomPosition(navMeshAgent.radius);
			patientInstance.transform.rotation = generateRandomRotation();
		}
	}


	/// <summary>
	/// Generates a random position inside the entrance.
	/// </summary>
	/// <param name="radius">The radius of the object to spawn on the position. Its bounds will be taken into account.</param>
	/// <returns>A random position inside the entrance taking into account the given radius.</returns>
	public Vector3 generateRandomPosition(float radius) {
		float x = Random.Range(size.minimumX + radius, size.maximumX - radius);
		float z = Random.Range(size.minimumZ + radius, size.maximumZ - radius);

		return transform.position + new Vector3(x, 0.0f, z);
	}

	/// <summary>
	/// Generates a random rotation inside the entrance.
	/// </summary>
	/// <returns>A random rotation inside the entrance.</returns>
	public Quaternion generateRandomRotation() {
		float angle = Random.Range(-180.0f, 180.0f);

		return Quaternion.AngleAxis(angle, Vector3.up);
	}
}
