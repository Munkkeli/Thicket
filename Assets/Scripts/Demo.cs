using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using View;

public class Demo : MonoBehaviour {
  public Router router;
  public Viewport viewport;
  public Curtain curtain;
  public float range = 16;
  public Vector2 position;
  public Transform follow;
  public float duration = 10;
  public bool done = false;

  private float timer = 0;

  void Update () {
    if (done) {
      curtain.visible = true;
    }

    if (timer <= 0 && !done) {
      timer = duration;

      Vector2 current;
      bool valid = false;
      do {
        current = router.grid.rect.center + new Vector2(Random.Range(-router.grid.size.x / 2, router.grid.size.x / 2), Random.Range(-router.grid.size.y / 2, router.grid.size.y / 2));
        valid = Physics2D.OverlapCircle(current, 0.5f) != null && router.grid.Get(current) != null && router.grid.Get(current).walkable;
      } while(!valid);

      position = current;
      viewport.Jump(position);

      Vector2 next;
      do {
        next = position + Random.insideUnitCircle.normalized * range;
        valid = Physics2D.OverlapCircle(next, 0.5f) != null && router.grid.Get(next) != null && router.grid.Get(next).walkable;
      } while(!valid);

      follow.transform.position = next;

      curtain.visible = false;
    }

    if (!curtain.visible && timer < 2.5f) {
      curtain.visible = true;
    }

    timer -= Time.deltaTime;
  }

  void OnDrawGizmos() {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(position, range);
  }
}
