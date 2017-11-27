using System.Collections;
using System.Collections.Generic;
using UnityEngine.PostProcessing;
using UnityEngine;

public class VisualTrigger : MonoBehaviour {
  public PostProcessingProfile profile;
  public int visibility = 0;

  void OnTriggerEnter2D (Collider2D collider) {
    if (collider.transform.parent.gameObject == Controller.current.player && Visuals.current.Check(profile)) {
      Debug.Log("Switch " + profile.name);
      Visuals.current.Switch(profile, visibility);
    }
  }
}
