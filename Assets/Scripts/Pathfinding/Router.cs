using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

      // Return path as Vector2[]
      return path.Select(x => x.position).ToArray();
    }
  }
}
