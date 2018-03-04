﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class LedgeWip : MonoBehaviour {

  [Header("Gizmos")]
  [Tooltip("The ledge line color")]
  public Color ledgeColor = Color.blue;
  [Tooltip("The ledge direction indicators size")]
  public float ledgeExtendsSize = 0.1f;
  [Tooltip("The needle color when grounded")]
  public Color groundedNeedleColor = Color.green;
  [Tooltip("The needle color when falling")]
  public Color fallingNeedleColor = Color.red;
  [Tooltip("The needle gizmos radius")]
  public float needleRadius = 0.1f;
  [Tooltip("The ray color when really crossing the ledge")]
  public Color trueCrossColor = Color.green;
  [Tooltip("The ray color when crossing the ledge extends only")]
  public Color crossExtendsColor = Color.red;

  [Header("Objects")]
  [Tooltip("The first ledge handle")]
  public Transform Ledge0;

  [Tooltip("The second ledge handle")]
  public Transform Ledge1;

  [Tooltip("The object that mimics a player")]
  public Transform needle;

  /// Returns the ledge vector
  public Vector3 LedgeVector {
    get {
      return Ledge0 == null || Ledge1 == null ? Vector3.zero : Ledge1.position - Ledge0.position;
    }
  }

  public Vector3 FloorVector {
    get {
      return Vector3.Cross(LedgeVector, Vector3.up).normalized;
    }
  }

  void OnDrawGizmos() {
    DrawExtendsGizmos();
    DrawNeedleGizmos();
  }

  void Update() {
  }

  private void DrawExtendsGizmos() {
    if (Ledge0 == null || Ledge1 == null) return;

    Gizmos.color = ledgeColor;

    // draws the ledge line
    Gizmos.DrawRay(Ledge0.position, LedgeVector);

    // draws rays at ledge_0
    Gizmos.DrawRay(Ledge0.position, -Vector3.up * ledgeExtendsSize);
    Gizmos.DrawRay(Ledge0.position, FloorVector * ledgeExtendsSize);

    // draws rays at ledge_1
    Gizmos.DrawRay(Ledge1.position, -Vector3.up * ledgeExtendsSize);
    Gizmos.DrawRay(Ledge1.position, FloorVector * ledgeExtendsSize);
  }

  private void DrawNeedleGizmos() {
    if (needle == null || Ledge0 == null || Ledge1 == null) return;

    // find the nearest point on the ledge vector
    var ledgeNormal = LedgeVector.normalized;
    var needleOnNormalized = needle.position - Ledge0.position;
    var projectedNormalized = Vector3.Project(needleOnNormalized, ledgeNormal);
    var res_nearestLedgePoint = Ledge0.position + projectedNormalized;

    // find whether the point is really on the ledge or on the extends only
    var pointFromLedge0 = res_nearestLedgePoint - Ledge0.position;
    var pointFromLedge1 = res_nearestLedgePoint - Ledge1.position;
    var magnitudeFromLedge0 = Vector3.SqrMagnitude(pointFromLedge0);
    var magnitudeFromLedge1 = Vector3.SqrMagnitude(pointFromLedge1);
    var res_isLedge0Extends = magnitudeFromLedge0 > LedgeVector.sqrMagnitude;
    var res_isLedge1Extends = magnitudeFromLedge1 > LedgeVector.sqrMagnitude;
    var res_isLedgeExtends = res_isLedge0Extends || res_isLedge1Extends;

    // find whether the character is on the ground side of the ledge or not
    var res_isGrounded = Vector3.Dot(FloorVector, needleOnNormalized) >= 0;


    // draws the grab ray
    var needleAtLedgeHeight = new Vector3(needle.position.x, res_nearestLedgePoint.y, needle.position.z);
    Gizmos.color = res_isLedgeExtends ? crossExtendsColor : trueCrossColor;
    Gizmos.DrawLine(needleAtLedgeHeight, res_nearestLedgePoint);
    Gizmos.DrawLine(needleAtLedgeHeight, needle.position);

    // draws the needle gizmos
    Gizmos.color = res_isGrounded ? groundedNeedleColor : fallingNeedleColor;
    Gizmos.DrawWireSphere(needle.position, needleRadius);
  }
}
