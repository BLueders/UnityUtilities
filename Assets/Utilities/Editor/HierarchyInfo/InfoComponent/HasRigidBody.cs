using UnityEngine;
using System.Collections;
using NiHierarchyInfo;

namespace NiHierarchyInfoComponents {
	public class HasRigidBody : HierarchyIndicatorBase {
		static string OnIndicatorCheck(GameObject gameObject) {
			if (gameObject.GetComponent<Rigidbody>() != null || gameObject.GetComponent<Rigidbody2D>() != null)
				return "R";
			return null;
		}

		static Color GetIndicatorColor() {
			return new Color(1, 0.4f, 0.4f);			
		}
	}
}
