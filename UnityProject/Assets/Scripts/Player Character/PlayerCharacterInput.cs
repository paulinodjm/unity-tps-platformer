using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This Behaviour is responsible for gathering the input for a character

[AddComponentMenu("Scripts/Player Control/Player Character Input")]
public class PlayerCharacterInput : MonoBehaviour {

  #region Inspector

  [Header("Displacements")]
  public string moveForwardAxis = "Vertical";
  public string moveRightAxis = "Horizontal";

  [Header("Walk")]
  public string walkHoldButton = "Walk";
  [Range(0f, 1f)]
  public float walkFactor = 0.5f;

  [Header("View")]
  public string horizontalViewAxis = "Mouse X";
  public string verticalViewAxis = "Mouse Y";

  #endregion

  /// Returns the input move
  public Vector3 Move {
    get {
      var move = GetRawMove();
      ClampMove(ref move);
      ApplyWalk(ref move);
      return move;
    }
  }

  /// Returns the view move input
  public Vector2 ViewMove {
    get {
      var horizontalMove = Input.GetAxis(horizontalViewAxis);
      var verticalMove = Input.GetAxis(verticalViewAxis);
      return new Vector2(horizontalMove, verticalMove);
    }
  }

  /// Returns the raw move input
  private Vector3 GetRawMove() {
    var forward = Input.GetAxis(moveForwardAxis);
    var right = Input.GetAxis(moveRightAxis);
    return new Vector3(right, 0.0f, forward);
  }

  /// Ensure that the move velocity is in the range [0, 1]
  private void ClampMove(ref Vector3 move) {
    if (move.sqrMagnitude > 1f) {
      move.Normalize();
    }
  }

  /// Apply the walk factor if needed
  private void ApplyWalk(ref Vector3 move) {
    if (Input.GetButton(walkHoldButton)) {
      move *= walkFactor;
    }
  }
}
