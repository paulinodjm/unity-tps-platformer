using UnityEngine;

[ExecuteInEditMode]
public class NOTR_Ledge : MonoBehaviour {

  [SerializeField]
  private Vector3 _extend1 = new Vector3();

  public Vector3 Extend1 {
    get { return _extend1; }
    set { _extend1 = value; }
  }

  public virtual void Update() {
  }

  void OnDrawGizmos() {
    Gizmos.color = Color.blue;
    Gizmos.DrawLine(transform.position, Extend1);
  }
}
