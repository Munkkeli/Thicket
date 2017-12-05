using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Title : MonoBehaviour {
  public bool visible = false;

  private Text text;
  private RectTransform image;

  private Color transparent;
  private float color;
  private float underline;
  private float colorRef;
  private float underlineRef;

  void Awake() {
    transparent = Color.white;
    transparent.a = 0;

    text = GetComponent<Text>();
    image = transform.GetChild(0).GetComponent<RectTransform>();
  }

  void Update () {
    color = Mathf.SmoothDamp(color, visible ? 1 : 0, ref colorRef, 1f);
    underline = Mathf.SmoothDamp(underline, color > 0.9f ? 1 : 0, ref underlineRef, 1.5f);

    text.color = Color.Lerp(transparent, Color.white, color);
    image.localScale = new Vector3(underline, 1, 1);
  }
}
