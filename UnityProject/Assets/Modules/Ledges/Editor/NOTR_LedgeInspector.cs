using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NOTR_Ledge)), CanEditMultipleObjects]
public class NOTR_LedgeInspector : Editor {

  override public void OnInspectorGUI() {
    base.OnInspectorGUI();
  }

  protected virtual void OnSceneGUI() {
    var ledge = (NOTR_Ledge)target;

    var toolsHidden = Tools.hidden;
    Tools.hidden = true;

    EditorGUI.BeginChangeCheck();
    var newPosition = Handles.PositionHandle(ledge.Extend1, Quaternion.identity);
    if (EditorGUI.EndChangeCheck()) {
      Undo.RecordObject(ledge, "Change extend position");
      ledge.Extend1 = newPosition;
      ledge.Update();
    }

    if (Event.current.type == EventType.Repaint) {
      Handles.CubeHandleCap(0, ledge.transform.position, ledge.transform.rotation, 0.3f, EventType.Repaint);
    }

    Tools.hidden = toolsHidden;
  }

  protected virtual void OnEnable() {
    Tools.hidden = true;
  }

  protected virtual void OnDisable() {
    Tools.hidden = false;
  }

}
