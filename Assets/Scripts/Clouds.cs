using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {
  public MeshRenderer view;

  void Update () {
    view.material.mainTextureOffset = transform.position * 0.012f;
  }
}
