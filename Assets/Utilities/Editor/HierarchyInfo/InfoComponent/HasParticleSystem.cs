using UnityEngine;
using System.Collections;
using NiHierarchyInfo;

namespace NiHierarchyInfoComponents
{
    public class HasParticleSystem : HierarchyIndicatorBase
    {
        static string OnIndicatorCheck(GameObject gameObject)
        {
            if (gameObject.GetComponent<ParticleSystem>() != null)
                return "★";
            return null;
        }

        static Color GetIndicatorColor()
        {
            return new Color(1, 0.4f, 1);
        }
    }
}

