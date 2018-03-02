using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NOTR/Ledge System")]
public class LedgeSystem : MonoBehaviour {
  public List<LedgeHandle> _handles;

  void OnDrawGizmos() {
    if (_handles == null || _handles.Count < 2) return;

    for (var pointIndex = 0; pointIndex < _handles.Count; pointIndex += 1) {
      var isLast = pointIndex == _handles.Count - 1;

      var point0 = _handles[pointIndex];
      var point1 = isLast ? null : _handles[pointIndex + 1];

      Gizmos.color = Color.blue;
      if (point0 != null && point1 != null) {
        Gizmos.DrawLine(point0.transform.position, point1.transform.position);
      }
    }
  }
}
