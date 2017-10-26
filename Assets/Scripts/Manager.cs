using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager {
  public static int scale = 1;
  public static int width = 144 * scale;
  public static int height = 256 * scale;
  public static int ppu = 8;

  public static float ratio = (float)width / (float)height;
  public static float size = ((float)height / 2f) / ppu;

  public static float Snap(float value) {
    return Mathf.Round(value * (float)ppu) / (float)ppu;
  }
}
