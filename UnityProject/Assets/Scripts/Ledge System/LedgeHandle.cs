using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeHandle : MonoBehaviour {
  private readonly Vector3 HandleSize = Vector3.one * 0.1f;

  public LedgeSystem ownerSystem;
  public int index;

  void OnDrawGizmos() {
    Gizmos.color = Color.blue;
    Gizmos.DrawCube(transform.position, HandleSize);
  }
}
