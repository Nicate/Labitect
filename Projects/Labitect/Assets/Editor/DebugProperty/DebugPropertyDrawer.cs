using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DebugPropertyAttribute))]
public class DebugPropertyDrawer : PropertyDrawer {
	/// <summary>
	/// The color used for debug properties.
	/// </summary>
	private static readonly Color color = new Color(1.0f, 0.5f, 0.5f);


	/// <summary>
	/// Displays the background in the debug color to indicate this is a debug property.
	/// </summary>
	/// <param name="position">The position of the property.</param>
	/// <param name="property">The property to display with the debug color as background.</param>
	/// <param name="label">The label of the property.</param>
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		GUI.backgroundColor = color;

		EditorGUI.PropertyField(position, property, label, true);
	}
}
