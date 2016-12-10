using UnityEngine;
using UnityEditor;

public class RenameWalls : Editor {
	[MenuItem("/Labitect/Rename walls")]
	public static void renameTiles() {
		GameObject walls = GameObject.Find("Walls");

		if(walls != null) {
			for(int index = 0; index < walls.transform.childCount; index++) {
				walls.transform.GetChild(index).name = "Wall " + (index + 1);
			}
		}
	}
}
