using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(Draggable.DroppableConfig))]
public class DroppableConfigDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        Rect rect1 = new Rect(position.x, position.y, position.width, 16);
        Rect rect2 = new Rect(position.x, position.y + 16, position.width, 16);
        Rect rect3 = new Rect(position.x, position.y + 32, position.width, 16);
        Rect rect4 = new Rect(position.x, position.y + 48, position.width, 16);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        SerializedProperty isDroppableProp = property.FindPropertyRelative("isDroppable");
        EditorGUI.PropertyField(rect1, isDroppableProp);
        if (isDroppableProp.boolValue)
        {
            EditorGUI.PropertyField(rect2, property.FindPropertyRelative("collisionDistance"));
            EditorGUI.PropertyField(rect3, property.FindPropertyRelative("dropContainerLayers"));
            EditorGUI.PropertyField(rect4, property.FindPropertyRelative("overlapPhysics"));
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty isDroppableProp = property.FindPropertyRelative("isDroppable");
        if (isDroppableProp.boolValue) {
            return 75;
        }
        return 20;
    }
}

