using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
  public float speed = 1;
  public Camera view;

  public LayerMask pickupLayer;
  public LayerMask infoLayer;

  public GameObject notifyIcon;
  public GameObject infoIcon;

  private Rigidbody2D body;
  private Dictionary<Collider2D, GameObject> inRange = new Dictionary<Collider2D, GameObject>();

  void Awake() {
    // Snap();
  }

  void Start() {
    view.orthographicSize = Manager.size;
    body = GetComponent<Rigidbody2D>();
    // body.isKinematic = true;
  }

  void Update() {
    Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 6, (pickupLayer | infoLayer));
    foreach (Collider2D coll in colls) {
      if (!inRange.ContainsKey(coll)) {
        bool isInfo = infoLayer == (infoLayer | (1 << coll.gameObject.layer));
        inRange.Add(coll, Instantiate(isInfo ? infoIcon : notifyIcon, coll.bounds.center, Quaternion.identity));
      }
    }

    Dictionary<Collider2D, GameObject> toRemove = new Dictionary<Collider2D, GameObject>();
    foreach (KeyValuePair<Collider2D, GameObject> coll in inRange) {
      if (Vector2.Distance(coll.Key.bounds.center, transform.position) > 6) {
        toRemove.Add(coll.Key, coll.Value);
      }
    }

    foreach (KeyValuePair<Collider2D, GameObject> coll in toRemove) {
      inRange.Remove(coll.Key);
      Destroy(coll.Value);
    }
  }

  void FixedUpdate() {
    Vector3 velocity = new Vector3();

    if (Input.GetKey(KeyCode.W)) velocity.y += 1;
    if (Input.GetKey(KeyCode.S)) velocity.y -= 1;
    if (Input.GetKey(KeyCode.D)) velocity.x += 1;
    if (Input.GetKey(KeyCode.A)) velocity.x -= 1;

    velocity.Normalize();

    body.MovePosition(body.position + (Vector2)(velocity * speed * Time.fixedDeltaTime));

    // Snap();
  }

  /*
  private void Snap() {
    Vector3 snapped = transform.position;
    snapped = new Vector3(Mathf.Round(snapped.x * Manager.ppu) / Manager.ppu, Mathf.Round(snapped.y * Manager.ppu) / Manager.ppu, snapped.z);
    transform.position = snapped;
  }
  */
}
