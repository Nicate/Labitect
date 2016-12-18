using System.Collections.Generic;
using UnityEngine;

public class WallComparer : IComparer<GameObject> {
	public int Compare(GameObject wall1, GameObject wall2) {
		int comparison = wall1.transform.position.z.CompareTo(wall2.transform.position.z);

		if(comparison == 0) {
			return wall1.transform.position.x.CompareTo(wall2.transform.position.x);
		}
		else {
			return comparison;
		}
	}
}

