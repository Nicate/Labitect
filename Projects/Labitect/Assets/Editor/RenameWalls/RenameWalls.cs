using UnityEditor;
using UnityEngine;

public class RenameWalls : Editor {
	[MenuItem("/Labitect/Rename walls")]
	public static void renameWalls() {
		GameObject walls = GameObject.Find("Walls");

		if(walls != null) {
			for(int index = 0; index < walls.transform.childCount; index++) {
				walls.transform.GetChild(index).name = "Wall " + (index + 1);
			}
		}
	}
}
