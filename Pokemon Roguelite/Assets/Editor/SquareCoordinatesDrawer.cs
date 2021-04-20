using UnityEngine;
using UnityEditor;


/// <summary>
/// This script changes how SquareCoordinates are displayed in editor.
/// </summary>
[CustomPropertyDrawer(typeof(SquareCoordinates))]
public class SquareCoordinatesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SquareCoordinates coordinates = new SquareCoordinates(property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue);

        position = EditorGUI.PrefixLabel(position, label);
        GUI.Label(position, coordinates.ToString());
    }
}
