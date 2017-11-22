using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiled2Unity;
using View;

namespace Pathfinding {
  [RequireComponent(typeof(TiledMap))]
  public class Grid : MonoBehaviour {
    public bool visualize = true;

    public LayerMask collision;
    public TiledMap map;

    public Vector2 size { get { return new Vector2(map.NumTilesWide, map.NumTilesHigh) * (map.TileHeight / Render.ppu); } }
    public Rect rect { get { return new Rect(transform.position.x, transform.position.y, size.x, -size.y); } }
    public int max { get { return width * height; } }

    private Tile[,] grid;
    private int width;
    private int height;
    private float square;

    void Awake() {
      if (map == null) map = GetComponent<TiledMap>();

      width = map.MapWidthInPixels / map.TileWidth;
      height = map.MapHeightInPixels / map.TileHeight;
      square = size.x / width;

      Create();
    }

    void OnDrawGizmos() {
      if (!visualize || map == null) return;
      Gizmos.DrawWireCube(rect.center, size);

      if (grid == null) return;

      // Draw grid & non walkable tiles
      foreach (Tile tile in grid) {
        if (!tile.walkable) {
          Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
          Gizmos.DrawCube(tile.position, new Vector2(square, square));
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(tile.position, new Vector2(square, square));
      }
    }

    /// <summary>
    /// Gets a tile at a world coordinate and returns it.
    /// </summary>
    /// <param name="point">The position you want to get the tile at.</param>
    /// <returns>A grid tile.</returns>
    public Tile Get(Vector2 point) {
      Vector2 local = transform.InverseTransformPoint(point - new Vector2(0, square / 2));

      int x = Mathf.RoundToInt((width - 1) * Mathf.Clamp01(local.x / size.x));
      int y = Mathf.RoundToInt((height - 1) * ((size.y + local.y) / size.y));
      return grid[x, y];
    }

    /// <summary>
    /// Modifies the collision value of a tile.
    /// </summary>
    /// <param name="point">The position you want to modify the tile at.</param>
    public void Set(Vector2 point, bool value) {
      Tile tile = Get(point);
      tile.walkable = value;
    }

    /// <summary>
    /// Gets all neighboring tiles for a tile.
    /// </summary>
    /// <param name="tile">The tile you want to get the neighbors for.</param>
    /// <returns>An array of tiles.</returns>
    public List<Tile> Neighbors(Tile tile) {
      List<Tile> neighbors = new List<Tile>();
      for (int x = -1; x <= 1; x++) {
        for (int y = -1; y <= 1; y++) {
          if (x == 0 && y == 0) continue;

          int cx = tile.x + x;
          int cy = tile.y + y;

          // Make sure the tile is inside grid bounds
          if (cx >= 0 && cx < width && cy >= 0 && cy < height) {
            neighbors.Add(grid[cx, cy]);
          }
        }
      }
      return neighbors;
    }

    /// <summary>
    /// Calculates a distance between two tiles.
    /// </summary>
    /// <param name="a">Start tile.</param>
    /// <param name="b">End tile.</param>
    /// <returns>The calculated distance in tiles as an int.</returns>
    public int Distance(Tile a, Tile b) {
      int x = Mathf.Abs(a.x - b.x);
      int y = Mathf.Abs(a.y - b.y);
      if (x > y) return (14 * y) + (10 * (x - y));
      return (14 * x) + (10 * (y - x));
    }

    /// <summary>
    /// Creates the grid and checks the collisions for tiles.
    /// </summary>
    private void Create() {
      grid = new Tile[width, height];

      // Calculate the bottom right corner of the grid
      Vector2 corner = rect.position + new Vector2(0, -size.y);

      for (int x = 0; x < width; x++) {
        for (int y = 0; y < height; y++) {
          // Calculate the tiles position in world space
          Vector2 position = corner + new Vector2(x * square, y * square) + new Vector2(square / 2, square / 2);

          // Check if tile has something non walkable on top of it
          bool walkable = !Physics2D.OverlapCircle(position, 0.25f, collision);

          grid[x, y] = new Tile(x, y, position, walkable);
        }
      }
    }
  }
}
