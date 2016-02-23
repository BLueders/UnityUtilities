using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NiModelProcessor {
	public class ModelProcessor : AssetPostprocessor {
		void OnPreprocessModel() {
			ModelImporter modelImporter = (ModelImporter)assetImporter;
			if (modelImporter.importMaterials == true) {
				modelImporter.importMaterials = false;
				modelImporter.globalScale = 1.0f;
				modelImporter.animationType = ModelImporterAnimationType.None;
			}
		}
	}
}
