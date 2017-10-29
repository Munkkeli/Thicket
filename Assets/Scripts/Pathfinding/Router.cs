using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
  [RequireComponent(typeof(Grid))]
  public class Router : MonoBehaviour {
    [HideInInspector]
    public Grid grid;

    void Awake() {
      grid = GetComponent<Grid>();
    }

    public Vector2[] Find(Vector2 start, Vector2 stop) {
      bool success = false;

      Tile first = grid.Get(start);
      Tile last = grid.Get(stop);

      Heap<Tile> open = new Heap<Tile>(grid.max);
      HashSet<Tile> closed = new HashSet<Tile>();
      open.Add(first);

      // If target is not walkable, return null
      if (!last.walkable) return null;

      while(open.Count > 0) {
        Tile current = open.RemoveFirst();
        closed.Add(current);

        // Path complete, break
        if (current == last) {
          success = true;
          break;
        }

        // Go through the tiles in an A* manner
        foreach (Tile neighbor in grid.Neighbors(current)) {
          if (!neighbor.walkable || closed.Contains(neighbor)) continue;

          int cost = current.gCost + grid.Distance(current, neighbor);
          if (cost < neighbor.gCost || !open.Contains(neighbor)) {
            neighbor.gCost = cost;
            neighbor.hCost = grid.Distance(neighbor, last);
            neighbor.parent = current;

            if (!open.Contains(neighbor)) {
              open.Add(neighbor);
              open.Update(neighbor);
            }
          }
        }
      }

      // If path was found, retrace & convert to Vector2[]
      if (success) return Retrace(first, last);

      // If path was not found, return null
      return null;
    }

    private Vector2[] Retrace(Tile first, Tile last) {
      List<Tile> path = new List<Tile>();
      Tile current = last;

      // Retrace the path
      while (current != first) {
        path.Add(current);
        current = current.parent;
      }

      // Flip it the right way
      path.Reverse();

      // Simplify & return Vector2[]
      return Simplify(path);
    }

    public Vector2[] Simplify(List<Tile> path) {
      List<Vector2> waypoints = new List<Vector2>();
      Vector2 direction = Vector2.zero;

      // Loop through the path & only keep points if the direction has changed
      for (int i = 1; i < path.Count; i++) {
        Vector2 current = new Vector2(path[i - 1].x - path[i].x, path[i - 1].y - path[i].y);
        //if (current != direction) {
          waypoints.Add(path[i].position);
          direction = current;
        //}
      }

      return waypoints.ToArray();
    }
  }
}
