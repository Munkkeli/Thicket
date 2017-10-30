using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : MonoBehaviour {
  public bool usePixelPerfect = true;

  public View view;
  public Transform follow;
  public float speed = 1;
  public float limit = 2;

  private Vector3 position;
  private Vector3 velocity;

  void Update () {
    Vector3 next = follow.position;
    next.z = -10;
    position = Vector3.SmoothDamp(position, next, ref velocity, speed, limit, Time.deltaTime);

    transform.position = position; //Manager.Snap(position);

    if (usePixelPerfect && !view.gameObject.activeInHierarchy) {
      Camera camera = GetComponent<Camera>();
      camera.targetTexture = view.texture;
      view.gameObject.SetActive(true);
    } else if (!usePixelPerfect && view.gameObject.activeInHierarchy) {
      Camera camera = GetComponent<Camera>();
      camera.targetTexture = null;
      view.gameObject.SetActive(false);
    }
  }
}
