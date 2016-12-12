using UnityEngine;

public class UserInterface : MonoBehaviour {
	public GameObject[] gameObjectsToDisable;
	public GameObject[] gameObjectsToEnable;


	public void Start() {
		foreach(GameObject gameObject in gameObjectsToDisable) {
			gameObject.SetActive(true);
		}
		
		foreach(GameObject gameObject in gameObjectsToEnable) {
			gameObject.SetActive(false);
		}
	}


	public void toggleUserInterface() {
		foreach(GameObject gameObject in gameObjectsToDisable) {
			gameObject.SetActive(false);
		}
		
		foreach(GameObject gameObject in gameObjectsToEnable) {
			gameObject.SetActive(true);
		}
	}
}
