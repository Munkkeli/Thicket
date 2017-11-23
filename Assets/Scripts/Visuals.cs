using System.Collections;
using System.Collections.Generic;
using UnityEngine.PostProcessing;
using UnityEngine;

public class Visuals : MonoBehaviour {
  public static Visuals current;

  [Range(1, 50)]
  public float visibility = 20;
  public bool hasClouds = true;
  public Texture2D lut;

  void Awake () {
    current = this;
  }
}
