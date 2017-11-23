﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Pathfinding;
using View;

public class Controller : MonoBehaviour {
  public static event Action OnScreenResize;

  public GameObject UI;
  public Router router;

  public GameObject playerPrefab;
  public Transform startFrom;
  public Transform walkTo;

  public GameObject viewportPrefab;
  [HideInInspector]
  public Viewport viewport;

  private Canvas canvas;
  private CanvasScaler scaler;
  private int width = 0;
  private int height = 0;

  void Awake() {
    GameObject ui = Instantiate(UI, Vector3.zero, Quaternion.identity, transform);
    canvas = ui.GetComponent<Canvas>();
    scaler = ui.GetComponent<CanvasScaler>();

    viewport = Instantiate(viewportPrefab, Vector3.zero, Quaternion.identity).GetComponent<Viewport>();
    canvas.worldCamera = viewport.current.viewport;
    canvas.sortingLayerName = "UI";

    Player player = Instantiate(playerPrefab, startFrom.position, Quaternion.identity).GetComponent<Player>();
    player.Move(walkTo.position);

    player.router = router;
    player.viewport = viewport;

    viewport.follow = player.transform;
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
