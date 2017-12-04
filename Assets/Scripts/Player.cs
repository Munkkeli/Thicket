using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using View;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour {
  [HideInInspector]
  public Router router;
  [HideInInspector]
  public Viewport viewport;

  [Header("Pointer")]
  public GameObject selector;
  public Sprite move;
  public Sprite failure;

  [Header("Movement")]
  public float speed = 1;
  public float maxSpeed = 1;
  public float smoothing = 1;
  public float animationSpeed = 1;
  public Collider2D main;

  [Header("Sounds")]
  public AudioClip[] footsteps;
  public float footstepFrequency = 1;
  public AudioClip click;

  [Header("Info & Pickup")]
  public SpriteRenderer carryingRenderer;
  public float pickupDistance = 4;
  public LayerMask pickupLayer;
  public GameObject pickupIcon;

  [Space(10)]
  public float infoDistance = 6;
  public LayerMask infoLayer;
  public GameObject infoIcon;

  [Space(10)]
  public List<Item> inventory = new List<Item>();

  private SpriteRenderer selectorRenderer;
  private Vector2[] path;
  private int progress;

  private Rigidbody2D body;
  private Dictionary<Collider2D, GameObject> inRange = new Dictionary<Collider2D, GameObject>();

  private Vector2 position;
  private Vector2 lastPosition;

  private Vector2 correction { get { return transform.position - main.bounds.center; } }

  private AudioSource audioSource;
  private float footstepTimer = 0;

  private Animator animator;
  private bool fleeing = false;

  void Start() {
    body = GetComponent<Rigidbody2D>();
    audioSource = GetComponent<AudioSource>();
    animator = GetComponentInChildren<Animator>();
    position = transform.position;

    selectorRenderer = Instantiate(selector, Vector3.zero, Quaternion.identity).GetComponent<SpriteRenderer>();
    selectorRenderer.gameObject.SetActive(false);
  }

  void Update() {
    if (Controller.current.paused) return;

    Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, infoDistance, (pickupLayer | infoLayer));
    foreach (Collider2D coll in colls) {
      if (!inRange.ContainsKey(coll) && Vector2.Distance(coll.bounds.center, transform.position) <= infoDistance) {
        bool isInfo = infoLayer == (infoLayer | (1 << coll.gameObject.layer));
        inRange.Add(coll, Instantiate(isInfo ? infoIcon : pickupIcon, coll.bounds.center, Quaternion.identity));
      }
    }

    bool pickup = false;
    Dictionary<Collider2D, GameObject> toRemove = new Dictionary<Collider2D, GameObject>();
    Dictionary<Collider2D, GameObject> toPickup = new Dictionary<Collider2D, GameObject>();
    foreach (KeyValuePair<Collider2D, GameObject> coll in inRange) {
      float distance = Vector2.Distance(coll.Key.bounds.center, transform.position);

      if (!pickup && Input.GetMouseButtonDown(0) && coll.Key.bounds.Contains(new Vector3(viewport.mouse.x, viewport.mouse.y, coll.Value.transform.position.z))) {
        if (pickupLayer == (pickupLayer | (1 << coll.Key.gameObject.layer)) && distance <= pickupDistance && inventory.Count <= 0) {
          Item item = coll.Key.gameObject.GetComponent<Pickup>().item;
          Debug.Log("Pickup " + item.name);
          inventory.Add(item);

          audioSource.PlayOneShot(item.sound, Random.Range(2.5f, 3f));

          toPickup.Add(coll.Key, coll.Value);
          pickup = true;
        }

        if (infoLayer == (infoLayer | (1 << coll.Key.gameObject.layer)) && distance <= infoDistance) {
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

    if (!pickup && !fleeing && Input.GetMouseButtonDown(0)) {
      Navigate(viewport.mouse);
      audioSource.PlayOneShot(click, Random.Range(2f, 2.5f));
    }

    if (inventory.Count > 0 && carryingRenderer.sprite != inventory[0].sprite) {
      carryingRenderer.sprite = inventory[0].sprite;
    } else if (inventory.Count <= 0 && carryingRenderer.sprite != null) {
      carryingRenderer.sprite = null;
    }
  }

  void FixedUpdate() {
    Vector2 velocity = (Vector2)(lastPosition - body.position);
    body.MovePosition(position);

    float change = Mathf.Max(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y));
    if (footstepTimer <= 0) {
      audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)], Random.Range(0.25f, 0.5f));
      footstepTimer = footstepFrequency;
    } else if (change > 0) {
      footstepTimer -= Time.fixedDeltaTime;
    }

    int direction = 0;

    if (Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) {
      if (velocity.y > 0) {
        direction = 1;
      } else if (velocity.y < 0) {
        direction = 2;
      }
    } else {
      if (velocity.x > 0) {
        direction = 3;
      } else if (velocity.x < 0) {
        direction = 4;
      }
    }

    animator.SetInteger("Direction", direction);
    animator.SetFloat("Speed", animationSpeed * (speed / 0.1f));
    animator.SetBool("Carrying", inventory.Count > 0);

    lastPosition = body.position;
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

  /// <summary>
  /// Move player gradually to some other location, not taking collisions into accord.
  /// </summary>
  /// <param name="point">The position you want the player to move to.</param>
  public void Move(Vector2 point) {
    progress = 0;
    path = new Vector2[] { transform.position, point };

    StopCoroutine("Follow");
    StartCoroutine("Follow");
  }

  /// <summary>
  /// Flee to a point. When player is fleeing, no control from the user is applied.
  /// </summary>
  /// <param name="point">The position you want the player to flee to.</param>
  public void Flee(Vector2 point) {
    if (fleeing) return;
    fleeing = true;
    Move(point);

    Debug.Log("Flee to " + point);
  }

  /// <summary>
  /// Pathfind the player to a certain location.
  /// </summary>
  /// <param name="to">The position you want the player to move to.</param>
  private void Navigate(Vector2 point) {
    Vector2[] path = router.Find(transform.position, point);
    if (path != null && path.Length > 10) path = null;

    Tile click = router.grid.Get(point);
    // Don't do anything if clicked outside the map
    if (click == null) return;

    selectorRenderer.transform.position = click.position;
    selectorRenderer.sprite = (path == null) ? failure : move;

    StopCoroutine("Flash");
    StartCoroutine("Flash");

    if (path == null) return;

    progress = 0;
    this.path = path;

    StopCoroutine("Follow");
    StartCoroutine("Follow");
  }

  /// <summary>
  /// Follow a path from the Router class.
  /// </summary>
  private IEnumerator Follow() {
    if (path.Length > 0) {
      Vector2 current = path[0];

      while (true) {
        if (Vector2.Distance(transform.position, current + correction) < 0.5f) {
          progress++;

          if (progress >= path.Length) {
            fleeing = false;
            yield break;
          }

          current = path[progress];
        }

        position = Vector2.MoveTowards(position, current + correction, speed);

        yield return null;
      }
    }
  }

  /// <summary>
  /// Flash the cursor indicating where the player is moving to.
  /// </summary>
  private IEnumerator Flash() {
    for (int i = 0; i < 3; i++) {
      selectorRenderer.gameObject.SetActive(true);
      yield return new WaitForSeconds(0.5f);
      selectorRenderer.gameObject.SetActive(false);
      yield return new WaitForSeconds(0.5f);
    }
  }
}
