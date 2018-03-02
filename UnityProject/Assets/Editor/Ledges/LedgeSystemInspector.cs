using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LedgeSystem))]
public class NotrLedgeSystemInspector : Editor {

  override public void OnInspectorGUI() {
    if (GUILayout.Button("Add Before")) {
      Debug.Log("Add before");
    }

    if (GUILayout.Button("Add After")) {
      Debug.Log("Add after");
    }
  }
}
