using UnityEngine;

public class Lab : MonoBehaviour {
	/// <summary>
	/// The size of the lab.
	/// </summary>
	public Size size;


	/// <summary>
	/// Generates a random position inside the lab.
	/// </summary>
	/// <param name="radius">The radius of the object to spawn on the position. Its bounds will be taken into account.</param>
	/// <returns>A random position inside the lab taking into account the given radius.</returns>
	public Vector3 generateRandomPosition(float radius) {
		float x = Random.Range(size.minimumX - 0.5f + radius, size.maximumX + 0.5f - radius);
		float z = Random.Range(size.minimumZ - 0.5f + radius, size.maximumZ + 0.5f - radius);

		return transform.position + new Vector3(x, 0.0f, z);
	}

	/// <summary>
	/// Generates a random rotation inside the lab.
	/// </summary>
	/// <returns>A random rotation inside the lab.</returns>
	public Quaternion generateRandomRotation() {
		float angle = Random.Range(-180.0f, 180.0f);

		return Quaternion.AngleAxis(angle, Vector3.up);
	}
}
