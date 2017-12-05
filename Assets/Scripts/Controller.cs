using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using Pathfinding;
using View;
using UI;

public class Controller : MonoBehaviour {
  public static event Action OnScreenResize;
  public static Controller current;

  public GameObject UI;
  public Router router;

  public GameObject playerPrefab;
  public Transform startFrom;
  public Transform walkTo;
  [HideInInspector]
  public GameObject player;

  public GameObject viewportPrefab;
  public Viewport viewport;
  [HideInInspector]
  public Dialog dialog;

  public bool paused = false;
  public bool demo = false;

  private Canvas canvas;
  private CanvasScaler scaler;
  private int width = 0;
  private int height = 0;
  private string level = "";
  private float timer = 0;

  void Awake() {
    GameObject ui = Instantiate(UI, Vector3.zero, Quaternion.identity, transform);
    canvas = ui.GetComponent<Canvas>();
    scaler = ui.GetComponent<CanvasScaler>();
    dialog = ui.GetComponentInChildren<Dialog>();

    if (!demo) {
      viewport = Instantiate(viewportPrefab, startFrom != null ? startFrom.position : Vector3.zero, Quaternion.identity).GetComponent<Viewport>();
      canvas.worldCamera = viewport.viewport;
      canvas.sortingLayerName = "UI";

      player = Instantiate(playerPrefab, startFrom.position, Quaternion.identity);

      Player playerScript = player.GetComponent<Player> ();
      playerScript.Move(walkTo.position);

      playerScript.router = router;
      playerScript.viewport = viewport;

      viewport.follow = player.transform;
    }

    current = this;
  }

  void Update() {
    Vector2 point;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(dialog.content, viewport.viewport.WorldToScreenPoint(viewport.mouse), viewport.viewport, out point);
    if (Input.GetMouseButtonUp(0) && dialog.visible && dialog.content.rect.Contains(point)) ClickDialog();
  }

  void FixedUpdate() {
    // Check if screen size has changed & call event
    if (width != Screen.width || height != Screen.height) {
      width = Screen.width;
      height = Screen.height;
      if (OnScreenResize != null) OnScreenResize();
    }

    if (level != "" && timer <= 0) {
      OnScreenResize = null;
      SceneManager.LoadScene(level, LoadSceneMode.Single);
    } else if (level != "") {
      timer -= Time.fixedDeltaTime;
    }
  }

  public void LoadScene(string scene) {
    level = scene;
    timer = 2;

    viewport.curtain.visible = true;
  }

  public void OpenDialog(string text) {
    paused = true;
    dialog.Set(text);
  }

  public void ClickDialog() {
    dialog.Scroll();
    if (!dialog.visible) StartCoroutine(UnPause());
  }

  public IEnumerator UnPause() {
    yield return new WaitForSeconds(0.1f);
    paused = false;
  }
}
