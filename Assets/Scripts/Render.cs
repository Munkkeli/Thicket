using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Render {
  public static int scale = 1;
  public static int width = 144 * scale;
  public static int height = 256 * scale;
  public static int ppu = 8;

  public static float ratio = (float)width / (float)height;
  public static float size = ((float)height / 2f) / ppu;

  public static float Snap(float value) {
    return Mathf.Round(value * (float)ppu) / (float)ppu;
  }

  public static Vector3 Snap(Vector3 value, bool centered = false) {
    value = new Vector3(Mathf.Round(value.x * Render.ppu) / Render.ppu, Mathf.Round(value.y * Render.ppu) / Render.ppu, value.z);
    if (centered) value += new Vector3((1f / Render.ppu) / 2f, (1f / Render.ppu) / 2f, 0);
    return value;
  }
}
