using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Environment {
  public class Clouds : MonoBehaviour {
    public MeshRenderer view;
    public float speed = 0.012f;

    void Awake() {
      Controller.OnScreenResize += Resize;
    }

    void Update() {
      view.material.mainTextureOffset = transform.position * speed;
    }

    /// <summary>
    /// Resizes the cloud texture to fill the screen.
    /// Gets called automagically when screen resolution changes.
    /// </summary>
    public void Resize() {
      transform.localScale = new Vector2(Render.size * 2 * Render.ratio, Render.size * 2);
    }
  }
}
