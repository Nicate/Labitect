using UnityEngine;

public class Appearance : MonoBehaviour {
	public GameObject[] heads;

	public GameObject[] straightjackets;
	public GameObject[] bodiesWithoutPockets;
	public GameObject[] bodiesWithPockets;


	public void Start() {
		if(heads.Length > 0) {
			Instantiate(heads[Random.Range(0, heads.Length)], transform, false);
		}

		int numberOfPockets = Random.Range(0, 3);

		if(numberOfPockets == 0) {
			if(straightjackets.Length > 0) {
				Instantiate(straightjackets[Random.Range(0, straightjackets.Length)], transform, false);
			}
		}
		else if(numberOfPockets == 1) {
			if(bodiesWithoutPockets.Length > 0) {
				Instantiate(bodiesWithoutPockets[Random.Range(0, bodiesWithoutPockets.Length)], transform, false);
			}
		}
		else if(numberOfPockets == 2) {
			if(bodiesWithPockets.Length > 0) {
				Instantiate(bodiesWithPockets[Random.Range(0, bodiesWithPockets.Length)], transform, false);
			}

			// Add extra pocket.
			numberOfPockets = 3;
		}

		Patient patient = GetComponent<Patient>();
		
		if(patient != null) {
			patient.numberOfPockets = numberOfPockets;
		}
	}
}
