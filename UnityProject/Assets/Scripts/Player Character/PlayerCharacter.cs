using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : MonoBehaviour {

  public NavMeshAgent navMeshAgent;

  [Header("Displacements")]
  public float runSpeed = 7f;
  public float rotationSpeed = 360f;

  private Vector3 _pendingMove;
  private Vector3? _aimTarget;

  /// Gets or sets the pending move for the current frame
  public Vector3 PendingMove {
    get { return _pendingMove; }
    set { _pendingMove = value; }
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
