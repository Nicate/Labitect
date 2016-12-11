using UnityEngine;

public class Identity : MonoBehaviour {
	public string[] patientNames;

	public string patientName;


	public void Start() {
		if(patientNames.Length > 0) {
			patientName = patientNames[Random.Range(0, patientNames.Length)];
		}
		else {
			patientName = "John Doe";
		}
	}
}
