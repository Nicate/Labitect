using System.Collections.Generic;
using UnityEngine;

public class TileComparer : IComparer<GameObject> {
	public int Compare(GameObject tile1, GameObject tile2) {
		int comparison = tile1.transform.position.z.CompareTo(tile2.transform.position.z);

		if(comparison == 0) {
			return tile1.transform.position.x.CompareTo(tile2.transform.position.x);
		}
		else {
			return comparison;
		}
	}
}
