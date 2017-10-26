using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : MonoBehaviour {
  public Transform follow;

  void Update () {
    Vector3 position = Manager.Snap(follow.position);
    position.z = -10;
    transform.position = position;
  }
}
