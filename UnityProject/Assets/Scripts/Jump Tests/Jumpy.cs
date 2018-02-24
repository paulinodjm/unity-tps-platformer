using UnityEngine;

class Jumpy : MonoBehaviour {

  public Transform start;
  public Transform target;

  [Range(0, 1)]
  public float factor;

  public float a = -1;
  public float c = 0;

  /// <summary>
  /// Callback to draw gizmos that are pickable and always drawn.
  /// </summary>
  void OnDrawGizmos() {
    if (!start || !target) return;

    Gizmos.color = Color.blue;
    Gizmos.DrawLine(start.position, new Vector3(target.position.x, start.position.y, target.position.z));
    Gizmos.color = Color.green;
    Gizmos.DrawLine(new Vector3(target.position.x, start.position.y, target.position.z), target.position);

    // Get the "x" and "y" values, in world coordinates
    var distance = GetTargetDistance();
    var height = GetTargetHeight();

    // Find the "b" parameter for the equation reaching the target point
    var b = FindB(distance, height);
    // Find the current "x" value
    var x = factor * distance;
    // Find the "y" for the current "x" value.
    // "y" is the vertical offset from the start point
    var y = FindY(x, b);

    // Find the "x" and "y" for the higher point in the character's trajectory
    var middleX = FindMiddleX(b);
    var middleY = FindY(middleX, b);
    var middleFactor = Mathf.InverseLerp(0, distance, middleX);
    var middlePoint = Vector3.Lerp(start.position, new Vector3(target.position.x, start.position.y, target.position.z), middleFactor);

    // Find the character position for the current progression factor
    var currentPoint = Vector3.Lerp(start.position, target.position, factor);
    currentPoint.y = start.position.y + y;

    Gizmos.color = Color.red;
    Gizmos.DrawLine(middlePoint, middlePoint + new Vector3(0, middleY, 0));
    Gizmos.DrawSphere(currentPoint, 0.15f);
  }

  /// Find the "b" parameter for the jump equation
  private float FindB(float d, float h) {
    return (h + a * d * d) / d;
  }

  private float FindMiddleX(float b) {
    return b / (2 * a);
  }

  private float FindY(float x, float b) {
    return -(a * x * x) + (b * x) + c;
  }

  private float GetTargetDistance() {
    var start2d = new Vector2(start.position.x, start.position.z);
    var target2d = new Vector2(target.position.x, target.position.z);
    return Vector2.Distance(start2d, target2d);
  }

  private float GetTargetHeight() {
    return target.position.y - start.position.y;
  }
}
