using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour {
  public Camera render;
  public Transform screen;
  public RenderTexture texture;

  void Start () {
    texture.width = Manager.width;
    texture.height = Manager.height;

    render.orthographicSize = (float)texture.width * ((float)Screen.height / (float)Screen.width) * 0.5f;
    screen.localScale = new Vector3(texture.width, texture.height, 1);
  }
}
