using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Viewport : MonoBehaviour {
  public bool usePixelPerfect = true;

  public View view;
  public Transform follow;
  public float speed = 1;
  public float limit = 2;

  public Vector2 mouse;

  private Camera viewport;
  private Vector3 position;
  private Vector3 velocity;

  void Awake() {
    viewport = GetComponent<Camera>();
    viewport.orthographicSize = Manager.size;
  }

  void Update () {
    Vector3 next = follow.position;
    next.z = -10;
    position = Vector3.SmoothDamp(position, next, ref velocity, speed, limit, Time.deltaTime);

    transform.position = position; //Manager.Snap(position);

    if (usePixelPerfect && !view.gameObject.activeInHierarchy) {
      viewport.targetTexture = view.texture;
      view.gameObject.SetActive(true);
    } else if (!usePixelPerfect && view.gameObject.activeInHierarchy) {
      viewport.targetTexture = null;
      view.gameObject.SetActive(false);
    }

    TrackMouse();
  }

  private void TrackMouse() {
    float ratio = (float)Screen.width / (float)Screen.height;
    float from = viewport.orthographicSize * 2;
    float to = view.render.orthographicSize * 2;

    float projection = from / to;
    Vector3 mousePositionRelativeToCenter = (view.render.ScreenToWorldPoint(Input.mousePosition) - position) * projection;
    float correction = usePixelPerfect ? (from * ratio) / (from * Manager.ratio) : 1;

    this.mouse = position + (mousePositionRelativeToCenter / correction);
  }
}
