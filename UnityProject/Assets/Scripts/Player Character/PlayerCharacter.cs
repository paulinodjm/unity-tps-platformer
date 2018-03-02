using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : MonoBehaviour {

  public NavMeshAgent navMeshAgent;

  [Header("Displacements")]
  public float runSpeed = 7f;
  public float rotationSpeed = 360f;
  public float jumpSpeed = 1f;

  private Vector3 _pendingMove;
  private Vector3? _aimTarget;
  private bool _shouldJump;

  private Jump _jump;

  /// Gets or sets the pending move for the current frame
  public Vector3 PendingMove {
    get { return _pendingMove; }
    set { _pendingMove = value; }
  }

  /// Get or set a value telling whether the character should jump as soon as possible.
  public bool ShouldJump {
    get { return _shouldJump; }
    set { _shouldJump = value; }
  }

  void OnValidate() {
    if (navMeshAgent == null) {
      navMeshAgent = GetComponent<NavMeshAgent>();
    }
  }

  void LateUpdate() {
    Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.black);
  }

  /// Applies the pending movement for the current frame
  public void ApplyMovement() {
    if (_jump != null) {
      var success = _jump.Update(Time.deltaTime);
      navMeshAgent.transform.position = _jump.CurrentPosition;
      if (success) {
        _jump = null;
        navMeshAgent.enabled = true;
      }
      return;
    }

    if (ShouldJump) {
      print("Jump!");
      var jumpPosition = navMeshAgent.transform.position + _pendingMove * runSpeed;
      _jump = new Jump(navMeshAgent.transform.position, jumpPosition, runSpeed * _pendingMove.magnitude, jumpSpeed);
      navMeshAgent.enabled = false;
      ShouldJump = false;
      return;
    }

    navMeshAgent.velocity = _pendingMove * runSpeed;

    if (_aimTarget.HasValue) {
      // TODO aim logic
    } else {
      RotateToward(_pendingMove);
    }

    _pendingMove = Vector3.zero;
  }

  /// Makes the character facing direction depending on its rotation speed
  private void RotateToward(Vector3 direction) {
    if (direction == Vector3.zero) return;

    var desiredRotation = Quaternion.LookRotation(direction);
    var maxDelta = rotationSpeed * Time.deltaTime;
    transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, maxDelta);
  }
}

class Jump {
  private float _distance;
  private float _height;
  private float _targetDuration;
  private float _a;
  private float _b;
  private Vector3 _currentPosition;

  /// The start position
  public Vector3 StartPosition {
    get; private set;
  }

  /// The target position
  public Vector3 TargetPosition {
    get; private set;
  }

  /// The current jump position
  public Vector3 CurrentPosition {
    get { return _currentPosition; }
  }

  /// The applyed gravity
  public float Gravity {
    get; private set;
  }

  /// The character horizontal speed
  public float HorizontalSpeed {
    get; private set;
  }

  /// The time since the jump start
  public float TimeSinceJumpStart {
    get; private set;
  }

  public Jump(Vector3 start, Vector3 target, float speed, float gravity = 9.81f) {
    StartPosition = start;
    TargetPosition = target;
    HorizontalSpeed = speed;
    Gravity = gravity;
    _currentPosition = StartPosition;
    TimeSinceJumpStart = 0;

    _distance = GetTargetDistance();
    _height = GetTargetHeight();
    _targetDuration = GetDuration();

    _a = Gravity * _targetDuration;
    _b = (_height + _a * _distance * _distance) / _distance;

    Debug.Log(_targetDuration + "s " + _distance + "m => a=" + _a);
  }

  /// Updates the jump by the given delta time amount, and returns a value
  /// telling whether the target has been reached.
  public bool Update(float deltaTime) {
    TimeSinceJumpStart += deltaTime;
    var factor = Mathf.Clamp(Mathf.InverseLerp(0, _targetDuration, TimeSinceJumpStart), 0f, 1f);
    var x = factor * _distance;
    var y = -(_a * x * x) + (_b * x);

    _currentPosition.x = Mathf.Lerp(StartPosition.x, TargetPosition.x, factor);
    _currentPosition.z = Mathf.Lerp(StartPosition.z, TargetPosition.z, factor);
    _currentPosition.y = StartPosition.y + y;

    return factor == 1;
  }

  private float GetTargetDistance() {
    var start2d = new Vector2(StartPosition.x, StartPosition.z);
    var target2d = new Vector2(TargetPosition.x, TargetPosition.z);
    return Vector2.Distance(start2d, target2d);
  }

  private float GetTargetHeight() {
    return TargetPosition.y - StartPosition.y;
  }

  private float GetDuration() {
    return HorizontalSpeed > 0f ? _distance / HorizontalSpeed : 0f;
  }
}
