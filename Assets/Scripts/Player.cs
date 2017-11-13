using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using View;

public class Player : MonoBehaviour {
  public Router router;
  public Viewport viewport;

  public Transform selector;

  public float speed = 1;
  public float maxSpeed = 1;
  public float smoothing = 1;

  public Collider2D main;

  public float pickupDistance = 4;
  public LayerMask pickupLayer;

  public float infoDistance = 6;
  public LayerMask infoLayer;

  public GameObject notifyIcon;
  public GameObject infoIcon;

  public List<Item> inventory = new List<Item>();

  private Vector2[] path;
  private int progress;

  private Rigidbody2D body;
  private Dictionary<Collider2D, GameObject> inRange = new Dictionary<Collider2D, GameObject>();

  private Vector2 position;
  private Vector2 velocityRef;

  private Vector2 correction { get { return transform.position - main.bounds.center; } }

  void Start() {
    body = GetComponent<Rigidbody2D>();
    position = transform.position;
  }

  void Update() {
    Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, infoDistance, (pickupLayer | infoLayer));
    foreach (Collider2D coll in colls) {
      if (!inRange.ContainsKey(coll) && Vector2.Distance(coll.bounds.center, transform.position) <= infoDistance) {
        bool isInfo = infoLayer == (infoLayer | (1 << coll.gameObject.layer));
        inRange.Add(coll, Instantiate(isInfo ? infoIcon : notifyIcon, coll.bounds.center, Quaternion.identity));
      }
    }

    bool pickup = false;
    Dictionary<Collider2D, GameObject> toRemove = new Dictionary<Collider2D, GameObject>();
    Dictionary<Collider2D, GameObject> toPickup = new Dictionary<Collider2D, GameObject>();
    foreach (KeyValuePair<Collider2D, GameObject> coll in inRange) {
      float distance = Vector2.Distance(coll.Key.bounds.center, transform.position);

      if (!pickup && Input.GetMouseButtonDown(0) && coll.Key.bounds.Contains(viewport.mouse)) {
        if (pickupLayer == (pickupLayer | (1 << coll.Key.gameObject.layer)) && distance <= pickupDistance) {
          Item item = coll.Key.gameObject.GetComponent<Pickup>().item;
          Debug.Log("Pickup " + item.name);
          inventory.Add(item);

          toPickup.Add(coll.Key, coll.Value);
          pickup = true;
        }

        if (infoLayer == (infoLayer | (1 << coll.Key.gameObject.layer)) && distance <= pickupDistance) {
          Debug.Log(coll.Key.gameObject);
          coll.Key.GetComponent<Usable>().OnClick(this);
          toRemove.Add(coll.Key, coll.Value);
        }
      }

      if (distance > infoDistance) {
        toRemove.Add(coll.Key, coll.Value);
      }
    }

    foreach (KeyValuePair<Collider2D, GameObject> coll in toRemove) {
      inRange.Remove(coll.Key);
      Destroy(coll.Value);
    }

    foreach (KeyValuePair<Collider2D, GameObject> coll in toPickup) {
      inRange.Remove(coll.Key);
      Destroy(coll.Value);
      Destroy(coll.Key.gameObject);
    }

    if (!pickup && Input.GetMouseButtonUp(0)) {

      Navigate(viewport.mouse);
    }

    //transform.position = position; //Manager.Snap(position);
  }

  void FixedUpdate() {
    Vector3 velocity = new Vector3();

    if (Input.GetKey(KeyCode.W)) velocity.y += 1;
    if (Input.GetKey(KeyCode.S)) velocity.y -= 1;
    if (Input.GetKey(KeyCode.D)) velocity.x += 1;
    if (Input.GetKey(KeyCode.A)) velocity.x -= 1;

    velocity.Normalize();

    // target += (Vector2)(velocity * speed * Time.fixedDeltaTime);

    // Vector3 position = Vector2.SmoothDamp(body.position, target, ref velocityRef, smoothing, maxSpeed, Time.fixedDeltaTime);

    body.MovePosition(position);

    // transform.position = position;
  }

  void OnDrawGizmos() {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(transform.position, 6);

    if (path == null) return;

    for (int i = progress; i < path.Length; i++) {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(path[i], 0.25f);

      if (i < path.Length - 1) {
        Gizmos.DrawLine(path[i], path[i + 1]);
      }
    }
  }

  private void Navigate(Vector2 point) {
    Vector2[] path = router.Find(transform.position, point);

    selector.transform.position = router.grid.Get(point).position;

    StopCoroutine("Flash");
    StartCoroutine("Flash");

    if (path == null) return;
    progress = 0;
    this.path = path;

    StopCoroutine("Follow");
    StartCoroutine("Follow");
  }

  private IEnumerator Follow() {
    if (path.Length > 0) {
      Vector2 current = path[0];

      while (true) {
        if (Vector2.Distance(transform.position, current + correction) < 0.5f) {
          progress++;

          if (progress >= path.Length) yield break;

          current = path[progress];
        }

        position = Vector2.MoveTowards(position, current + correction, speed);

        yield return null;
      }
    }
  }

  private IEnumerator Flash() {
    for (int i = 0; i < 3; i++) {
      selector.gameObject.SetActive(true);
      yield return new WaitForSeconds(0.5f);
      selector.gameObject.SetActive(false);
      yield return new WaitForSeconds(0.5f);
    }
  }
}
