using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour {
  public Camera view;
  public Transform screen;
  public RenderTexture texture;

  void Start () {
    texture.width = Manager.width;
    texture.height = Manager.height;

    view.orthographicSize = (float)(texture.width / 10) * ((float)Screen.height / (float)Screen.width) * 0.5f;
    screen.localScale = new Vector3(texture.width / 10, texture.height / 10, 1);
  }
}
