using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SortTiles : Editor {
	[MenuItem("/Labitect/Sort tiles")]
	public static void sortTiles() {
		GameObject floor = GameObject.Find("Floor");

		if(floor != null) {
			List<GameObject> tiles = new List<GameObject>();

			foreach(Transform tileTransform in floor.transform) {
				tiles.Add(tileTransform.gameObject);
			}

			floor.transform.DetachChildren();

			tiles.Sort(new TileComparer());

			foreach(GameObject tile in tiles) {
				tile.transform.SetParent(floor.transform);
			}
		}
	}
}
