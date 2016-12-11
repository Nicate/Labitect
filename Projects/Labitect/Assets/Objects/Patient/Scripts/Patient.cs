using UnityEngine;

public class Patient : MonoBehaviour {
	public GameObject[] heads;

	public GameObject[] straightjackets;
	public GameObject[] bodiesWithoutPockets;
	public GameObject[] bodiesWithPockets;


	public void Awake() {
		
	}

	public void Start() {
		var bla = GetComponent<NavMeshAgent>();
		bla.destination = Vector3.zero;

		GameObject head = Instantiate(heads[0], transform, false) as GameObject;
		GameObject body = Instantiate(bodiesWithoutPockets[0], transform, false) as GameObject;
	}

	public void Update() {
		
	}
}
