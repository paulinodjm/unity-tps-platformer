using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Player Control/Player Controller")]
public class PlayerController : MonoBehaviour {
  public PlayerCharacterInput playerInput;
  public PlayerCharacter character;
  public CameraController playerCamera;

  void OnValidate() {
    if (playerInput == null) {
      playerInput = GetComponent<PlayerCharacterInput>();
    }
    if (character == null) {
      character = GetComponent<PlayerCharacter>();
    }
    if (playerCamera == null) {
      playerCamera = GetComponent<CameraController>();
    }
  }

  void Update() {
    playerCamera.UpdatePov();
    GroundMove();
    character.ApplyMovement();
  }

  private void GroundMove() {
    var inputMove = playerInput.Move;

    Vector3 forward, right;
    GetGroundedAxis(out forward, out right);

    var move = TransformMoveDirection(inputMove, forward, right);
    character.PendingMove = move;
  }

  private void GetGroundedAxis(out Vector3 forward, out Vector3 right) {
    if (playerCamera == null) {
      forward = transform.forward;
      right = transform.right;
    } else {
      right = Vector3.Cross(playerCamera.transform.forward, Vector3.down).normalized;
      forward = Vector3.Cross(right, Vector3.up).normalized;
    }
  }

  private Vector3 TransformMoveDirection(Vector3 move, Vector3 forwardAxis, Vector3 rightAxis) {
    var forward = move.z * forwardAxis;
    var right = move.x * rightAxis;
    return forward + right;
  }
}
