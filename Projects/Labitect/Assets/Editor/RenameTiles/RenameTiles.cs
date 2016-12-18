using UnityEditor;
using UnityEngine;

public class RenameTiles : Editor {
	[MenuItem("/Labitect/Rename tiles")]
	public static void renameTiles() {
		GameObject floor = GameObject.Find("Floor");

		if(floor != null) {
			for(int index = 0; index < floor.transform.childCount; index++) {
				floor.transform.GetChild(index).name = "Tile " + (index + 1);
			}
		}
	}
}
