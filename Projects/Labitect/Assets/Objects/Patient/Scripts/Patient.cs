using UnityEngine;

public class Patient : MonoBehaviour {
	/// <summary>
	/// Pocket 0 is what they have in their hands.
	/// </summary>
	public int numberOfPockets;


	public void Awake() {
		
	}

	public void Start() {
		var bla = GetComponent<NavMeshAgent>();
		bla.destination = Vector3.zero;
	}

	public void Update() {
		
	}
}
