using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour {
  public Pathfinding.Grid grid;
  public string required;
  public GameObject[] onDisabled;
  public GameObject[] onEnabled;
  public Vector2[] collision;
  public bool state;

  public bool toggle;

  void Start() {
    Toggle(this.state);
  }

  void Update() {
    if (toggle) {
      toggle = false;
      Toggle(!state);
    }
  }

  public void Toggle(bool state) {
    this.state = state;

    foreach (GameObject thing in onDisabled) {
      thing.SetActive(!state);
    }

    foreach (GameObject thing in onEnabled) {
      thing.SetActive(state);
    }

    foreach (Vector2 point in collision) {
      grid.Set(point, state);
    }
  }
}
