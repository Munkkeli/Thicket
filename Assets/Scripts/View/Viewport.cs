using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View {
  [RequireComponent(typeof(Camera))]
  public class Viewport : MonoBehaviour {
    public bool usePixelPerfect = true;

    public Camera pixelRenderer;
    public Transform follow;
    public Transform screen;
    public GameObject clouds;
    public Curtain curtain;
    public float speed = 1;
    public float limit = 2;

    [HideInInspector]
    public Vector2 mouse;

    [HideInInspector]
    public Camera viewport;

    private RenderTexture texture;
    public Vector3 position;
    private Vector3 velocity;

    void Awake() {
      viewport = GetComponent<Camera>();
      CreateTexture();
      screen.GetComponent<Renderer>().material.mainTexture = texture;

      Controller.OnScreenResize += Resize;
    }

    void Start() {
      position = (follow != null) ? follow.position : transform.position;
      clouds.SetActive(Visuals.current.hasClouds);
      viewport.backgroundColor = Visuals.current.background;
    }

    void Update() {
      Vector3 next = (follow != null) ? follow.position : transform.position;
      next.z = -10;
      position = Vector3.SmoothDamp(position, next, ref velocity, speed, limit, Time.deltaTime);

      transform.position = usePixelPerfect ? Render.Snap(position) : position;

      Track();
    }

    /// <summary>
    /// Jumps the viewport to a position without smooth movement.
    /// </summary>
    /// <param name="point">The position you want to jump to.</param>
    public void Jump(Vector2 point) {
      position = (Vector3)point + new Vector3(0, 0, -10);
      velocity = Vector2.zero;
    }

    /// <summary>
    /// Resizes the pixel perfect renderer and the players viewport to fit the screen.
    /// Gets called automagically when screen resolution changes.
    /// </summary>
    public void Resize() {
      // Resize the render texture
      if (usePixelPerfect) {
        CreateTexture();
        screen.GetComponent<Renderer>().material.mainTexture = texture;
        if (usePixelPerfect) viewport.targetTexture = texture;
      }

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

    /// <summary>
    /// Transforms the mouse position from the screen into actual world coordinates.
    /// Takes into account the pixel perfext renderer if it's used.
    /// </summary>
    private void Track() {
      float ratio = (float)Screen.width / (float)Screen.height;
      float from = viewport.orthographicSize * 2;
      float to = pixelRenderer.orthographicSize * 2;

      float projection = from / to;
      Vector3 mousePositionRelativeToCenter = (pixelRenderer.ScreenToWorldPoint(Input.mousePosition) - position) * projection;
      float correction = usePixelPerfect ? (from * ratio) / (from * Render.ratio) : 1;

      this.mouse = position + (mousePositionRelativeToCenter / correction);
    }

    private void CreateTexture() {
      RenderTexture render = new RenderTexture(Render.width, Render.height, 16, RenderTextureFormat.ARGB32);
      render.filterMode = FilterMode.Point;
      render.name = Render.width + "x" + Render.height;
      render.Create();

      texture = render;
      screen.GetComponent<Renderer>().material.mainTexture = texture;
    }
  }
}
