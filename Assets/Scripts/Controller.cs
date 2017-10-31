using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
  public static event Action OnScreenResize;

  private int width = 0;
  private int height = 0;

  void Update () {
    // Check if screen size has changed & call event
    if (width != Screen.width || height != Screen.height) {
      width = Screen.width;
      height = Screen.height;
      if (OnScreenResize != null) OnScreenResize();
    }
  }
}
