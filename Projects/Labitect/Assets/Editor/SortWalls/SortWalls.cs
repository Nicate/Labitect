using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SortWalls : Editor {
	[MenuItem("/Labitect/Sort walls")]
	public static void sortWalls() {
		GameObject parent = GameObject.Find("Walls");

		if(parent != null) {
			List<GameObject> walls = new List<GameObject>();

			foreach(Transform wallTransform in parent.transform) {
				walls.Add(wallTransform.gameObject);
			}

			parent.transform.DetachChildren();

			walls.Sort(new WallComparer());

			foreach(GameObject wall in walls) {
				wall.transform.SetParent(parent.transform);
			}
		}
	}
}
