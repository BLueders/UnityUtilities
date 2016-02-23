using UnityEngine;
using System.Collections;
using NiHierarchyInfo;

namespace NiHierarchyInfoComponents
{
    public class HasLight : HierarchyIndicatorBase
    {
        static string OnIndicatorCheck(GameObject gameObject)
        {
            if (gameObject.GetComponent<Light>() != null)
                return "L";
            return null;
        }

        static Color GetIndicatorColor()
        {
            return new Color(1, 1, 0.4f);
        }
    }
}
