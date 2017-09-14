using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("Scripts/AI Characters/Move To Target")]
public class MoveToTarget : MonoBehaviour {
  public Target target;

  private NavMeshAgent _agent;

  public void SetTarget(Target newTarget) {
    if (newTarget != null) {
      _agent.destination = newTarget.transform.position;
    }
    target = newTarget;
  }

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    _agent = GetComponent<NavMeshAgent>();
  }

  void Start() {
    SetTarget(target);
  }

  void OnTriggerEnter(Collider other) {
    if (other.gameObject == target.gameObject) {
      SetTarget(target.nextPoint);
    }
  }
}
