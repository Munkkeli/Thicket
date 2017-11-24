using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
  public Controller controller;
  public string level;

  void OnTriggerEnter2D (Collider2D collider) {
    if (collider.transform.parent.gameObject == controller.player) {
      controller.LoadScene(level);
    }
  }
}
