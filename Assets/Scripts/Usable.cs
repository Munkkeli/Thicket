using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Usable : MonoBehaviour {
  public Pathfinding.Grid grid;
  public Item required;
  [TextArea]
  public string text;
  public AudioClip sound;
  public GameObject[] onDisabled;
  public GameObject[] onEnabled;
  public Vector2[] collision;

  [HideInInspector]
  public bool state;

  private int layer;
  private AudioSource audioSource;

  void Start() {
    grid = Controller.current.router.grid;
    audioSource = GetComponent<AudioSource>();
    this.layer = gameObject.layer;
    Toggle(this.state, true);
  }

  public void OnClick(Player player) {
    if (player.inventory.Contains(required)) {
      Toggle(true);
      player.inventory.Remove(required);
    } else if (text.Length > 0) {
      Controller.current.OpenDialog(text);
    }
  }

  private void Toggle(bool state, bool silent = false) {
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

    if (!silent) audioSource.PlayOneShot(sound, Random.Range(0.8f, 1.2f));
  }
}
