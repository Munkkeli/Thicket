using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace UI {
  public class Character {
    public char character;
    public Text text;

    private bool visible = false;
    private float size = 1f;

    public Character(char character, Text text) {
      this.character = character;
      this.text = text;
    }

    public void Reveal() {
      this.visible = true;
      size = 0.75f;
    }

    public void Update(float delta) {
      size = Mathf.Min(1, size + (4 * delta));
    }

    public string Draw() {
      bool large = size != 1;
      return (large ? "<size=" + (text.fontSize * size) + ">" : "")
        + (!visible ? "<color=#FFFFFF00>" : "") + character + (!visible ? "</color>" : "")
        + (large ? "</size>" : "");
    }
  }
}