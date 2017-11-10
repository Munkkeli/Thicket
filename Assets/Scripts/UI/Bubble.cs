using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
  public class Bubble : MonoBehaviour {
    public float bobble = 0.5f;
    public float frequency = 1f;

    public Transform bubble;

    private float height = 0;

    void Start() {
      height = bubble.localPosition.y;

      bubble.position = Render.Snap(bubble.position, true);
    }

    void Update() {
      Vector3 local = bubble.localPosition;
      local.y = height + (Mathf.Sin(Time.time * frequency) * bobble);
      bubble.localPosition = local;

      // transform.position = Manager.Snap(transform.position, true);
    }
  }
}
