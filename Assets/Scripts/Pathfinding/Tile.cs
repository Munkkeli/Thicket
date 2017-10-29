using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
  public class Tile : IHeapItem<Tile> {
    public bool walkable;
    public Vector2 position;

    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost { get { return gCost + hCost; } }

    public Tile parent;

    private int index;

    public Tile(int x, int y, Vector2 position, bool walkable) {
      this.x = x; this.y = y;
      this.position = position;
      this.walkable = walkable;
    }

    public int Index {
      get { return index; }
      set { index = value; }
    }

    public int CompareTo(Tile tile) {
      int compare = fCost.CompareTo(tile.fCost);
      if (compare == 0) compare = hCost.CompareTo(tile.hCost);
      return -compare;
    }
  }
}
