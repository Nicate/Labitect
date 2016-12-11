using UnityEngine;

public class Patient : MonoBehaviour {
	/// <summary>
	/// Pocket 0 is what they have in their hands.
	/// </summary>
	[HideInInspector]
	public int numberOfPockets;


	public void Awake() {
		
	}

	public void Start() {

	}

	public void Update() {
		
	}


	public void move(Vector3 position) {
		GetComponent<NavMeshAgent>().destination = position;
	}
}
