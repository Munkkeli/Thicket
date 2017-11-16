using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using View;

public class Controller : MonoBehaviour {
  public static event Action OnScreenResize;

  public GameObject UI;
  public Viewport viewport;

  private Canvas canvas;
  private CanvasScaler scaler;
  private int width = 0;
  private int height = 0;

  void Start () {
    GameObject ui = Instantiate(UI, Vector3.zero, Quaternion.identity, transform);
    canvas = ui.GetComponent<Canvas>();
    scaler = ui.GetComponent<CanvasScaler>();

    canvas.worldCamera = viewport.viewport;
    canvas.sortingLayerName = "UI";
  }

  void FixedUpdate () {
    // Check if screen size has changed & call event
    if (width != Screen.width || height != Screen.height) {
      width = Screen.width;
      height = Screen.height;
      if (OnScreenResize != null) OnScreenResize();
    }
  }
}
