using UnityEditor;
using UnityEngine;

// IngredientDrawer
[CustomPropertyDrawer(typeof(RangeFloat))]
public class RangeFloatDrawer : PropertyDrawer
{
	private float min, max;

	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		min = property.FindPropertyRelative ("min").floatValue;
	    max = property.FindPropertyRelative ("max").floatValue;

		EditorGUI.MinMaxSlider(
			position,
			GUIContent.none,
			ref min, ref max,
			property.FindPropertyRelative("minBorder").floatValue, property.FindPropertyRelative("maxBorder").floatValue);

		property.FindPropertyRelative ("min").floatValue = min;
		property.FindPropertyRelative ("max").floatValue = max;

		property.serializedObject.ApplyModifiedProperties ();

		EditorGUI.EndProperty();
	}
}