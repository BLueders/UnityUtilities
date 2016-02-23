using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Toolbox;


namespace NiHierarchyInfo {
	[InitializeOnLoad]
	public static class HierarchyInfo {
		static HierarchyInfo() {
			EditorApplication.hierarchyWindowItemOnGUI += OnDrawItem;
			indicators = TypeTools.GetAllSubTypes(typeof(HierarchyIndicatorBase));
			onIndicatorChecks = new MethodInfo[indicators.Length];
			getIndicatorColors = new MethodInfo[indicators.Length];
			for (int i = 0; i < indicators.Length; i++) {
				onIndicatorChecks[i] = indicators[i].GetMethod("OnIndicatorCheck", BindingFlags.NonPublic | BindingFlags.Static);
				getIndicatorColors[i] = indicators[i].GetMethod("GetIndicatorColor", BindingFlags.NonPublic | BindingFlags.Static);
			}
		}

		static Type[] indicators = null;
		static MethodInfo[] onIndicatorChecks;
		static MethodInfo[] getIndicatorColors;
		static int cursorStep = 9;

		static void OnDrawItem(int instanceID, Rect rect) {
			GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

            GUIStyle style = new GUIStyle();
            

            if (gameObject == null)
				return;

			float cursor = rect.width - 16;

            for (int i = 0; i < indicators.Length; i++) {
				string indicator = (string)onIndicatorChecks[i].Invoke(null, new object[] {gameObject});
				if (string.IsNullOrEmpty(indicator))
					continue;

                style.normal.textColor = (Color)getIndicatorColors[i].Invoke(null, null);
                GUI.Label(new Rect(rect.x + cursor, rect.y + 1, 15, rect.height - 2), indicator, style);
				cursor -= cursorStep;
			}
		}
	}
}
