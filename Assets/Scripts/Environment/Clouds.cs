using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment {
  public class Clouds : MonoBehaviour {
    public MeshRenderer view;
    public float speed = 0.012f;

    void Update () {
      view.material.mainTextureOffset = transform.position * 0.012f;
    }
  }
}
