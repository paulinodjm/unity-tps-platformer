using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/AI Characters/Target")]
public class Target : MonoBehaviour {
  public Target nextPoint;

  protected void OnDrawGizmosSelected() {
    if (nextPoint != null) {
      Gizmos.color = Color.blue;
      Gizmos.DrawLine(transform.position, nextPoint.transform.position);
    }
  }
}
