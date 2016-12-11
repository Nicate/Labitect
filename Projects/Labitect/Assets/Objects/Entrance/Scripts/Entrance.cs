using UnityEngine;

public class Entrance : MonoBehaviour {
	public GameObject patient;

	public int minimumNumberOfPatients;
	public int maximumNumberOfPatients;

	public Rect entranceSize;
	public Rect levelSize;


	public void spawnPatients() {
		int numberofPatients = Random.Range(minimumNumberOfPatients, maximumNumberOfPatients + 1);

		for(int count = 0; count < numberofPatients; count++) {
			Vector3 position = transform.position + new Vector3(Random.Range(entranceSize.xMin, entranceSize.xMax), 0.0f, Random.Range(entranceSize.yMin, entranceSize.yMax));
			Quaternion rotation = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), Vector3.up);

			GameObject patientInstance = Instantiate(patient, position, rotation, transform.parent) as GameObject;
			
			patientInstance.GetComponent<Patient>().move(new Vector3(Random.Range(levelSize.xMin, levelSize.xMax), 0.0f, Random.Range(levelSize.yMin, levelSize.yMax)));
		}
	}
}
