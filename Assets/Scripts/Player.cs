using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
  public float speed = 1;
  public Camera view;

  void Awake() {
    // Snap();
  }

  void Start() {
    view.orthographicSize = Manager.size;
  }

  void FixedUpdate() {
    Vector3 velocity = new Vector3();

    if (Input.GetKey(KeyCode.W)) velocity.y += 1;
    if (Input.GetKey(KeyCode.S)) velocity.y -= 1;
    if (Input.GetKey(KeyCode.D)) velocity.x += 1;
    if (Input.GetKey(KeyCode.A)) velocity.x -= 1;

    velocity.Normalize();

    transform.Translate(velocity * speed * 0.1f);

    Snap();
  }

  private void Snap() {
    Vector3 snapped = transform.position;
    snapped = new Vector3(Mathf.Round(snapped.x * Manager.ppu) / Manager.ppu, Mathf.Round(snapped.y * Manager.ppu) / Manager.ppu, snapped.z);
    transform.position = snapped;
  }
}
