using UnityEngine;

public class Appearance : MonoBehaviour {
	public GameObject[] heads;

	public GameObject[] straightjackets;
	public GameObject[] bodiesWithoutPockets;
	public GameObject[] bodiesWithPockets;


	public void Start() {
		Patient patient = GetComponent<Patient>();
		NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

		// Set up the patient's appearance.
		GameObject head = null;
		GameObject body = null;

		if(heads.Length > 0) {
			head = heads[Random.Range(0, heads.Length)];
		}

		if(patient.numberOfPockets == 0) {
			if(straightjackets.Length > 0) {
				body = straightjackets[Random.Range(0, straightjackets.Length)];
			}
		}
		else if(patient.numberOfPockets == 1) {
			if(bodiesWithoutPockets.Length > 0) {
				body = bodiesWithoutPockets[Random.Range(0, bodiesWithoutPockets.Length)];
			}
		}
		else if(patient.numberOfPockets == 2) {
			if(bodiesWithPockets.Length > 0) {
				body = bodiesWithPockets[Random.Range(0, bodiesWithPockets.Length)];
			}

			// Add an extra pocket.
			patient.numberOfPockets = 3;
		}

		GameObject headInstance = Instantiate(head, transform, false) as GameObject;
		GameObject bodyInstance = Instantiate(body, transform, false) as GameObject;

		// Beef up the patient.
		float beef = Mathf.Pow(1.5f, patient.strength);
		
		headInstance.transform.localScale = Vector3.Scale(headInstance.transform.localScale, new Vector3(beef, beef, beef));
		bodyInstance.transform.localScale = Vector3.Scale(bodyInstance.transform.localScale, new Vector3(beef, 1.0f, beef));
		
		navMeshAgent.radius = navMeshAgent.radius * beef;
	}
}
