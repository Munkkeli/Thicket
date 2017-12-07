using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI {
  public class Credits : MonoBehaviour {
    public Dictionary<string, string[]> credits = new Dictionary<string, string[]>() {
      { "The Team", new string[] {
        "Neea Oja",
        "Tony Holmberg",
        "Tuomas Pöyry"
      } },
      { "Textures", new string[] {
        "Sara Suviranta",
        "Tuomas Pöyry",
        "Zelda-like tilesets and sprites by ArMM1998"
      } },
      { "Audio", new string[] {
        "Forest ambiance by dobroide",
        "Frogs by oyez",
        "Master of the Feast by Kevin MacLeod",
        "RPG Audio by Kenney",
        "Soft wind & crickets by Leandros.Ntounis",
        "UI Audio by Kenney",
        "Water flows over a rock by volivieri"
      } },
      { "Special thanks", new string[] {
        "Santeri Sorjonen",
        "Sebastian Lague",
        "You!"
      } }
    };

    public RectTransform canvas;
    public Title title;
    public Demo demo;
    public Font headerFont;
    public Font nameFont;
    public int headerSize = 32;
    public int nameSize = 24;
    public float speed = 1f;
    public float logoTime = 5;
    public bool scroll = true;

    private RectTransform rect;

    void Start() {
      rect = GetComponent<RectTransform>();
      title.visible = false;

      foreach(KeyValuePair<string, string[]> section in credits) {
        GameObject header = new GameObject(section.Key, new System.Type[] { typeof(Text) });
        header.transform.SetParent(transform, false);
        header.transform.localScale = new Vector3(1, 1, 1);
        header.layer = gameObject.layer;
        SetText(header, section.Key, headerSize, headerFont);

        foreach(string name in section.Value) {
          CreateSpacer(6);

          GameObject text = new GameObject(name, new System.Type[] { typeof(Text), typeof(ContentSizeFitter) });
          text.transform.SetParent(transform, false);
          text.transform.localScale = new Vector3(1, 1, 1);
          text.layer = gameObject.layer;
          text.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
          SetText(text, name, nameSize, nameFont);
        }

        CreateSpacer(64);
      }
    }

    void Update() {
      bool done = rect.localPosition.y > (rect.sizeDelta.y + canvas.sizeDelta.y / 2);

      if (scroll && !done) rect.localPosition = new Vector2(0, rect.localPosition.y + (speed * (Input.GetMouseButton(0) ? 10 : 1) * Time.deltaTime));

      if (!demo.done && rect.localPosition.y > (rect.sizeDelta.y + canvas.sizeDelta.y / 2) - 100) {
        demo.done = true;
      }

      if (done) logoTime -= Time.deltaTime;

      if (done && logoTime <= 0) {
        Controller.current.LoadScene("Menu", 0);
      }

      if (!title.visible && done) {
        title.visible = true;
      }
    }

    public void CreateSpacer(int size) {
      GameObject spacer = new GameObject("Space", new System.Type[] { typeof(RectTransform), typeof(LayoutElement) });
      spacer.transform.SetParent(transform);
      spacer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, size);
      spacer.GetComponent<LayoutElement>().minHeight = size;
    }

    public void SetText(GameObject obj, string value, int size, Font font) {
      Text text = obj.GetComponent<Text>();
      text.text = value;
      text.font = font;
      text.fontSize = size;
      text.horizontalOverflow = HorizontalWrapMode.Wrap;
      text.verticalOverflow = VerticalWrapMode.Overflow;
      text.alignment = TextAnchor.MiddleCenter;

      obj.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 48);
    }
  }
}
