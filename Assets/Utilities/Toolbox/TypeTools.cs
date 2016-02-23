using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Toolbox {
	public class TypeTools {
		public static Type[] GetAllSubTypes(Type baseClass) {
			List<Type> result = new List<Type>();
			Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly a in assemblies) {
				Type[] types = a.GetTypes();
				foreach (Type t in types) {
					if (t.IsSubclassOf(baseClass))
						result.Add(t);
				}
			}
			return result.ToArray();
		}	
	}
}	
