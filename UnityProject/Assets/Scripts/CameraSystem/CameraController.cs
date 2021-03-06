﻿using UnityEngine;

[AddComponentMenu("Scripts/Player Control/Camera Controller")]
public class CameraController : MonoBehaviour {
  #region Public fields

  [Space(10.0f)]
  [Tooltip("Point aimed by the camera")]
  public Transform Target;

  [Tooltip("Maximum distance between the camera and Target")]
  public float Distance = 2;

  [Tooltip("Distance lerp factor")]
  [Range(.0f, 1.0f)]
  public float LerpSpeed = .1f;

  [Space(10.0f)]
  [Tooltip("Collision parameters")]
  public TraceInfo RayTrace = new TraceInfo { Thickness = .2f };

  [Tooltip("Camera pitch limitations")]
  public LimitsInfo PitchLimits = new LimitsInfo { Minimum = -60.0f, Maximum = 60.0f };

  [Header("Player Controls")]
  public PlayerCharacterInput input;
  public float horizontalSensitivity = 15f;
  public float verticalSensitivity = 15f;

  #endregion

  #region Structs

  [System.Serializable]
  public struct LimitsInfo {
    [Tooltip("Minimum pitch angle, in the range [-90, Maximum]")]
    public float Minimum;

    [Tooltip("Maximum pitch angle, in the range [Minimum, 90]")]
    public float Maximum;
  }

  [System.Serializable]
  public struct TraceInfo {
    [Tooltip("Ray thickness")]
    public float Thickness;

    [Tooltip("Layers the camera collide with")]
    public LayerMask CollisionMask;
  }

  [System.Serializable]
  public struct InputInfo {
    [Tooltip("Horizontal axis")]
    public InputAxisInfo Horizontal;

    [Tooltip("Vertical axis")]
    public InputAxisInfo Vertical;
  }

  [System.Serializable]
  public struct InputAxisInfo {
    [Tooltip("Input axis name")]
    public string Name;

    [Tooltip("Axis sensitivity")]
    public float Sensitivity;
  }

  #endregion

  private float _pitch;
  private float _distance;

  void OnValidate() {
    if (input == null) {
      input = GetComponent<PlayerCharacterInput>();
    }
  }

  void OnDrawGizmosSelected() {
    if (RayTrace.Thickness > 0) {
      Gizmos.color = Color.white;
      Gizmos.DrawWireSphere(transform.position, RayTrace.Thickness);
    }
  }

  public void Start() {
    _pitch = Mathf.DeltaAngle(0, -transform.localEulerAngles.x);
    _distance = Distance;
  }

  public void UpdatePov() {
    if (input == null) return;

    var inputMove = input.ViewMove;
    float yaw = transform.localEulerAngles.y + inputMove.x * horizontalSensitivity;

    _pitch += inputMove.y * verticalSensitivity;
    _pitch = Mathf.Clamp(_pitch, PitchLimits.Minimum, PitchLimits.Maximum);

    transform.localEulerAngles = new Vector3(-_pitch, yaw, 0);
  }

  public void LateUpdate() {
    if (Target == null) return;

    var startPos = Target.position;
    var endPos = startPos - transform.forward * Distance;
    var result = Vector3.zero;

    RayCast(startPos, endPos, ref result, RayTrace.Thickness);
    var resultDistance = Vector3.Distance(Target.position, result);

    if (resultDistance <= _distance) // closest collision
    {
      transform.position = result;
      _distance = resultDistance;
    } else {
      _distance = Mathf.Lerp(_distance, resultDistance, LerpSpeed);
      transform.position = startPos - transform.forward * _distance;
    }
  }

  private bool RayCast(Vector3 start, Vector3 end, ref Vector3 result, float thickness) {
    var direction = end - start;
    var distance = Vector3.Distance(start, end);

    RaycastHit hit;
    if (Physics.SphereCast(new Ray(start, direction), thickness, out hit, distance, RayTrace.CollisionMask.value)) {
      result = hit.point + hit.normal * RayTrace.Thickness;
      return true;
    } else {
      result = end;
      return false;
    }
  }
}
