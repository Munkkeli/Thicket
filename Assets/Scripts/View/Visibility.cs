using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View {
  public class Visibility : MonoBehaviour {
    public float speed = 0.5f;

    private Renderer render;
    private float state;
    private float stateRef;

    void Awake() {
      render = GetComponent<Renderer>();
      Controller.OnScreenResize += Resize;
    }

    void Update() {
      state = Mathf.SmoothDamp(state, Visuals.current.visibilityUnits, ref stateRef, speed);
      render.material.SetFloat("_Size", state);
    }

    /// <summary>
    /// Resizes the mask to fill the screen.
    /// Gets called automagically when screen resolution changes.
    /// </summary>
    public void Resize() {
      transform.localScale = new Vector2(Render.size * 2 * Render.ratio, Render.size * 2);
    }
  }
}
