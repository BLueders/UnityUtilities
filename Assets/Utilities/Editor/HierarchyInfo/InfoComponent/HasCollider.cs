using UnityEngine;
using System.Collections;
using NiHierarchyInfo;

namespace NiHierarchyInfoComponents {
	public class HasCollider : HierarchyIndicatorBase {
		static string OnIndicatorCheck(GameObject gameObject) {
			if (gameObject.GetComponent<Collider>() != null)
				return "C";
			return null;
		}

		static Color GetIndicatorColor() {
			return new Color(0.6f, 1, 0.6f);			
		}
	}
}
