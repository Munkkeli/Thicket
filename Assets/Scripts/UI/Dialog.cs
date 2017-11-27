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
    public AudioClip characterSound;
    public AudioClip openSound;

    private int index = 0;
    private int scroll = 0;
    private float delay = 0;
    private float top = 0;
    private int line = 0;
    private Character[] currentText;
    private string currentTextString;
    private Text textComponent;
    private RectTransform rectComponent;
    private TextGenerationSettings settings;
    private TextGenerator generator;
    private UILineInfo[] lines;
    private float width = 0;
    private bool waiting = false;
    private bool finished = false;
    private bool active = true;
    private AudioSource audioSource;
    private float positionAnimation = 0;
    private float positionAnimationVel = 0;

    void Awake() {
      textComponent = GetComponentInChildren<Text>();
      audioSource = GetComponent<AudioSource>();
      rectComponent = GetComponent<RectTransform>();
      generator = new TextGenerator();
      top = textComponent.rectTransform.offsetMax.y;
    }

    void Update () {
      if (panel.activeInHierarchy != active) panel.SetActive(active);

      rectComponent.anchoredPosition = new Vector3(0, 8 - ((64 + 32) * (1 - positionAnimation)));
      rectComponent.localRotation = Quaternion.Euler(0, 0, (1 - positionAnimation) * 8f);
      positionAnimation = Mathf.SmoothDamp(positionAnimation, visible ? 1 : 0, ref positionAnimationVel, 0.25f);

      active = positionAnimation > 0.01f;

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

      if (!waiting && positionAnimation > 0.8f && delay <= 0 && index < text.Length) {
        currentText[index].Reveal();
        audioSource.PlayOneShot(characterSound, Random.Range(0.4f, 0.8f));
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

      audioSource.PlayOneShot(openSound, Random.Range(1.8f, 2.2f));
    }

    public void Scroll() {
      if (finished) {
        visible = false;
        audioSource.PlayOneShot(openSound, Random.Range(1.8f, 2.2f));
        return;
      }

      if (!waiting) return;
      scroll++;
    }
  }
}