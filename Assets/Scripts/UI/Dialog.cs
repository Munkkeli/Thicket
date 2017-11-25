using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

namespace UI {
  public class Dialog : MonoBehaviour {
    [TextArea]
    public string text;
    public bool visible = false;
    public float speed = 0.1f;
    public float stop = 0.8f;
    public GameObject panel;
    public RectTransform more;
    public RectTransform content;

    private int index = 0;
    private int scroll = 0;
    private float delay = 0;
    private float top = 0;
    private int line = 0;
    private Character[] currentText;
    private string currentTextString;
    private Text textComponent;
    private TextGenerationSettings settings;
    private TextGenerator generator;
    private UILineInfo[] lines;
    private float width = 0;
    private bool waiting = false;
    private bool finished = false;

    void Awake() {
      textComponent = GetComponentInChildren<Text>();
      generator = new TextGenerator();
      top = textComponent.rectTransform.offsetMax.y;
    }

    void Update () {
      if (panel.activeInHierarchy != visible) panel.SetActive(visible);

      if (!visible) return;

      if (width != textComponent.rectTransform.rect.width) {
        width = textComponent.rectTransform.rect.width;
        Set(text);
      }

      if (delay > 0) delay -= Time.deltaTime;

      if (line < lines.Length - 1 && (index + 1) > lines[line + 1].startCharIdx) {
        line++;
      }

      int currentScroll = (int)(line / 3);
      waiting = currentScroll > scroll;

      if (!waiting && delay <= 0 && index < text.Length) {
        currentText[index].Reveal();
        switch (text[index]) {
          case ',':
            delay = stop * 0.75f;
            break;

          case '.':
            bool multi = index == text.Length - 1 || text[index + 1] == '.';
            delay = multi ? speed : stop;
            break;

          default:
            delay = speed;
            break;
        }

        index++;
      }

      finished = index >= text.Length;

      currentTextString = "";
      for (int i = 0; i < text.Length; i++) {
        currentText[i].Update(Time.deltaTime);
        currentTextString += currentText[i].Draw();
      }

      textComponent.text = currentTextString;

      textComponent.rectTransform.offsetMax = new Vector2(textComponent.rectTransform.offsetMax.x, (16 * scroll * 3));

      more.anchoredPosition = new Vector2(-4, 8 + (Mathf.Sin(Time.time * 6) * 4f));

      if (more.gameObject.activeInHierarchy != waiting) more.gameObject.SetActive(waiting);
    }

    public void Set(string text) {
      visible = true;
      this.text = text;

      textComponent.text = text;

      currentText = new Character[text.Length];
      for (int i = 0; i < text.Length; i++) {
        currentText[i] = new Character(text[i], textComponent);
      }

      Canvas.ForceUpdateCanvases();
      settings = textComponent.GetGenerationSettings(textComponent.rectTransform.rect.size);
      settings.verticalOverflow = VerticalWrapMode.Overflow;
      generator.Populate(text, settings);
      lines = generator.lines.ToArray();

      index = 0;
      line = 0;
      delay = 0;
      scroll = 0;
    }

    public void Scroll() {
      if (finished) {
        visible = false;
        return;
      }

      if (!waiting) return;
      scroll++;
    }
  }
}