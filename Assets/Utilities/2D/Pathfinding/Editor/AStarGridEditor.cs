using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AStarGrid2D))]
public class AStarGridEditor : Editor {

    Vector3 oldPosition = Vector3.zero;

    public override void OnInspectorGUI() {
        if (Application.isPlaying) {
            return;
        }
        
        DrawDefaultInspector();
        
        AStarGrid2D myTarget = (AStarGrid2D)target;
        
        if (!myTarget.AutoUpdateGrid && GUILayout.Button("Create Grid")) {
            myTarget.CreateGrid();
            SceneView.RepaintAll();
        }
        
        Vector3 newPosition = myTarget.gameObject.transform.position;
        if (myTarget.AutoUpdateGrid && oldPosition != newPosition) {
            myTarget.CreateGrid();
            SceneView.RepaintAll();
        }
        oldPosition = newPosition;
    }
}

