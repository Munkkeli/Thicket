﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View {
  public static class Render {
    public static int scale = 3;
    public static int ppu = 8;

    public static float dpi { get { return ((Screen.dpi <= 0 ? 258 : Screen.dpi) / 258); } }
    public static int width { get { return ((int)((Screen.width / dpi) / 16) * scale); } }
    public static int height { get { return ((int)((Screen.height / dpi) / 16) * scale); } }

    public static float ratio { get { return (float)width / (float)height; } }
    public static float size { get { return ((float)height / 2f) / ppu; } }

    /// <summary>
    /// Snaps a position into pixel coordinates, thus eliminating pixels rendering wrongly.
    /// </summary>
    /// <param name="value">A vector you want to snap into the pixel grid.</param>
    /// <param name="centered">Should the vector be snapped into the center or the edge of a pixel.</param>
    public static Vector3 Snap(Vector3 value, bool centered = false) {
      value = new Vector3(Mathf.Round(value.x * Render.ppu) / Render.ppu, Mathf.Round(value.y * Render.ppu) / Render.ppu, value.z);
      if (centered) value += new Vector3((1f / Render.ppu) / 2f, (1f / Render.ppu) / 2f, 0);
      return value;
    }
  }
}
