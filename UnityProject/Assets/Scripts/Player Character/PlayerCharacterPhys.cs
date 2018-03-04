using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacterPhys : PlayerCharacterBase {

  public CharacterController characterController;

  [Header("Displacements")]
  public float runSpeed = 7f;
  public float rotationSpeed = 360f;

  private Vector3 _pendingMove;
  private Vector3? _aimTarget;

  /// Gets or sets the pending move for the current frame
  public override Vector3 PendingMove {
    get { return _pendingMove; }
    set { _pendingMove = value; }
  }

  void OnValidate() {
    if (characterController == null) {
      characterController = GetComponent<CharacterController>();
    }
  }

  /// Applies the pending movement for the current frame
  public override void ApplyMovement() {
    characterController.Move(_pendingMove);

    if (_aimTarget.HasValue) {
      // TODO aim logic
    } else {
      RotateToward(_pendingMove);
    }

    _pendingMove = Vector3.zero;
  }

  /// Makes the character facing direction depending on its rotation speed
  protected override void RotateToward(Vector3 direction) {
    if (direction == Vector3.zero) return;

    var desiredRotation = Quaternion.LookRotation(direction);
    var maxDelta = rotationSpeed * Time.deltaTime;
    transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, maxDelta);
  }
}
