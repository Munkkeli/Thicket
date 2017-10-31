using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Viewport : MonoBehaviour {
  public bool usePixelPerfect = true;

  public Camera pixelRenderer;
  public Transform follow;
  public Transform screen;
  public float speed = 1;
  public float limit = 2;

  public Vector2 mouse;

  private Camera viewport;
  private RenderTexture texture;
  private Vector3 position;
  private Vector3 velocity;

  void Awake() {
    viewport = GetComponent<Camera>();
    texture = new RenderTexture(Render.width, Render.height, 16, RenderTextureFormat.ARGB32);
    texture.filterMode = FilterMode.Point;
    screen.GetComponent<Renderer>().material.mainTexture = texture;

    Controller.OnScreenResize += Resize;
  }

  void Update () {
    Vector3 next = follow.position;
    next.z = -10;
    position = Vector3.SmoothDamp(position, next, ref velocity, speed, limit, Time.deltaTime);

    transform.position = usePixelPerfect ? Render.Snap(position) : position;

    Track();
  }

  public void Resize() {
    if (usePixelPerfect && !pixelRenderer.gameObject.activeInHierarchy) {
      viewport.targetTexture = texture;
      pixelRenderer.gameObject.SetActive(true);
    } else if (!usePixelPerfect && pixelRenderer.gameObject.activeInHierarchy) {
      viewport.targetTexture = null;
      pixelRenderer.gameObject.SetActive(false);
    }

    // Pixel perfect camera needs to refresh to calculate input correctly
    if (!usePixelPerfect) {
      pixelRenderer.gameObject.SetActive(true);
      pixelRenderer.gameObject.SetActive(false);
    }

    viewport.orthographicSize = Render.size;
    pixelRenderer.orthographicSize = (float)Render.width * ((float)Screen.height / (float)Screen.width) * 0.5f;
    screen.localScale = new Vector3(Render.width, Render.height, 1);
  }

  private void Track() {
    float ratio = (float)Screen.width / (float)Screen.height;
    float from = viewport.orthographicSize * 2;
    float to = pixelRenderer.orthographicSize * 2;

    float projection = from / to;
    Vector3 mousePositionRelativeToCenter = (pixelRenderer.ScreenToWorldPoint(Input.mousePosition) - position) * projection;
    float correction = usePixelPerfect ? (from * ratio) / (from * Render.ratio) : 1;

    this.mouse = position + (mousePositionRelativeToCenter / correction);
  }
}
