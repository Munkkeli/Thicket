﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.PostProcessing;
using UnityEngine;
using Pathfinding;
using View;

public class Visuals : MonoBehaviour {
  public static Visuals current;

  [Range(1, 24)]
  public float visibility = 5;
  public bool hasClouds = true;
  public Color background;
  public Texture2D lut;

  public float visibilityUnits { get { return (grid.square) * visibility; } }

  private Pathfinding.Grid grid;

  void Awake () {
    grid = GetComponent<Pathfinding.Grid>();
    current = this;
  }
}