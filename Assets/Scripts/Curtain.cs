using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour {
  public bool visible = true;
  public float speed = 0.5f;

  private Renderer render;
  private float state;
  private float stateRef;

  void Awake() {
    render = GetComponent<Renderer>();
    Controller.OnScreenResize += Resize;
    StartCoroutine("Load");
  }

  void Update() {
    state = Mathf.SmoothDamp(state, visible ? 0 : 0.75f, ref stateRef, speed);
    render.material.SetFloat("_Size", state);
  }

  public void Resize() {
    float ratio = (float)Screen.width / (float)Screen.height;
    float size = ratio > 1 ? (Render.size * 2) * ratio : Render.size * 2;
    transform.localScale = new Vector2(size, size);
    state = visible ? 0 : 0.75f;
  }

  private IEnumerator Load() {
    yield return new WaitForSeconds(0.5f);
    visible = false;
  }
}
