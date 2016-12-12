using UnityEngine;

public class Patient : MonoBehaviour {
	[HideInInspector]
	public float speed;

	[HideInInspector]
	public float strength;

	/// <summary>
	/// Pocket 0 is what they have in their hands.
	/// </summary>
	[HideInInspector]
	public int numberOfPockets;


	public void Awake() {
		// Generate these here so scripts can use their values in Start().
		speed = Random.Range(0.0f, 1.0f);
		strength = Random.Range(0.0f, 1.0f);
		numberOfPockets = Random.Range(0, 3);
	}

	public void Start() {
		NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

		float spry = Mathf.Pow(2.0f, speed);

		navMeshAgent.speed = navMeshAgent.speed * spry;
		navMeshAgent.angularSpeed = navMeshAgent.angularSpeed * spry;
	}

	public void Update() {
		
	}


	public void move(Vector3 position) {
		GetComponent<NavMeshAgent>().destination = position;
	}
}
