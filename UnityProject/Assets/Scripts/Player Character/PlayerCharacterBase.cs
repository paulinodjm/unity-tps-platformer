using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayerCharacterBase : MonoBehaviour {

  /// Gets or sets the pending move for the current frame
  public abstract Vector3 PendingMove {
    get;
    set;
  }

  public void LateUpdate() {
    Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.black);
  }

  /// Applies the pending movement for the current frame
  public abstract void ApplyMovement();

  /// Makes the character facing direction depending on its rotation speed
  protected abstract void RotateToward(Vector3 direction);
}
