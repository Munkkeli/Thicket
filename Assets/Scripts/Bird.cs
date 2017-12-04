using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bird : MonoBehaviour {
  public float noticeRadius = 6f;
  public float chaseRadius = 8f;
  public float speed = 1;
  public Vector2 fleePosition;

  public Animator[] chicks;
  public Item accepts;
  public GameObject[] enableOnFirstContact;

  private bool firstTime = true;
  private bool mad { get { return status < chicks.Length; } }
  private int status = 0;
  private Player player;
  private Animator animator;
  private Vector2 nest;
  private Vector2 velocity;
  private bool scared = false;
  private bool stealing = false;

  void Awake() {
    animator = GetComponent<Animator>();
    nest = transform.position;
  }

  void Start() {
    player = Controller.current.player.GetComponent<Player>();
  }

  void Update() {
    float distance = Vector2.Distance(player.transform.position, nest);
    if (mad && !scared && distance < noticeRadius) {
      if (player.inventory.Contains(accepts)) {
        Controller.current.paused = true;
        player.Move(player.transform.position);
        player.inventory.Remove(accepts);
        chicks[status].SetBool("Fed", true);
        status++;
        stealing = true;
        scared = true;
      } else {
        player.Flee(fleePosition);
        scared = true;
      }

      if (firstTime) {
        firstTime = false;
        foreach (GameObject obj in enableOnFirstContact) {
          obj.SetActive(true);
        }
      }
    } else if (scared && distance < chaseRadius) {
      transform.position = Vector2.SmoothDamp(transform.position, player.transform.position, ref velocity, speed, 10f, Time.deltaTime);
    } else if (scared && distance > chaseRadius) {
      scared = false;
    } else {
      transform.position = Vector2.SmoothDamp(transform.position, nest, ref velocity, speed, 10f, Time.deltaTime);
    }

    if (stealing && Vector2.Distance(player.transform.position, transform.position) < 0.1f) {
      Controller.current.paused = false;
      player.Flee(fleePosition);
      stealing = false;
    }

    bool flying = Vector2.Distance(transform.position, nest) > 0.1f;
    animator.SetBool("Flying", flying);
    GetComponent<SpriteRenderer>().sortingOrder = flying ? 2 : 0;
  }

  void OnDrawGizmos() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere((nest != Vector2.zero) ? nest : (Vector2)transform.position, noticeRadius);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere((nest != Vector2.zero) ? nest : (Vector2)transform.position, chaseRadius);
  }
}
