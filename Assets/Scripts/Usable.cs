using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Usable : MonoBehaviour {
  public Pathfinding.Grid grid;
  public string required;
  public GameObject[] onDisabled;
  public GameObject[] onEnabled;
  public Vector2[] collision;
  public bool state;

  private int layer;

  void Start() {
    this.layer = gameObject.layer;
    Toggle(this.state);
  }

  public void OnClick(Player player) {
    Item item = player.inventory.Find(x => x.name == required);
    if (item != null) {
      Toggle(true);
      player.inventory.Remove(item);
    }
  }

  private void Toggle(bool state) {
    this.state = state;

    gameObject.layer = !state ? this.layer : 0;

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
