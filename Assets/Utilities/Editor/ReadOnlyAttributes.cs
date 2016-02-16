
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// An Attribute that will show properties in the Unity inspector but does not allow manipulating them. Works with all primitive types, vector2,3,4 and enums
/// </summary>
public class ShowOnlyAttribute : PropertyAttribute
{

}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;
		
		switch (prop.propertyType)
		{
		case SerializedPropertyType.Integer:
			valueStr = prop.intValue.ToString();
			break;
		case SerializedPropertyType.Boolean:
			valueStr = prop.boolValue.ToString();
			break;
		case SerializedPropertyType.Float:
			valueStr = prop.floatValue.ToString("0.00000");
			break;
		case SerializedPropertyType.String:
			valueStr = prop.stringValue;
			break;
		case SerializedPropertyType.Enum:
			valueStr = prop.enumNames[prop.enumValueIndex];
			break;
		case SerializedPropertyType.Vector2:
			valueStr = prop.vector2Value.ToString();
			break;
		case SerializedPropertyType.Vector3:
			valueStr = prop.vector3Value.ToString();
			break;
		case SerializedPropertyType.Vector4:
			valueStr = prop.vector4Value.ToString();
			break;
		default:
			valueStr = "(not supported)";
			break;
		}
		
		EditorGUI.LabelField(position,label.text, valueStr);
	}
}
#endif