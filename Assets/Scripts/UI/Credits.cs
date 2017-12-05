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

    public Font headerFont;
    public Font nameFont;
    public int headerSize = 32;
    public int nameSize = 24;
    public float speed = 1f;

    private RectTransform rect;

    void Start() {
      rect = GetComponent<RectTransform>();

      foreach(KeyValuePair<string, string[]> section in credits) {
        GameObject header = new GameObject(section.Key, new System.Type[] { typeof(Text) });
        header.transform.SetParent(transform);
        SetText(header, section.Key, headerSize, headerFont);

        foreach(string name in section.Value) {
          CreateSpacer(16);

          GameObject text = new GameObject(name, new System.Type[] { typeof(Text), typeof(ContentSizeFitter) });
          text.transform.SetParent(transform);
          text.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
          SetText(text, name, nameSize, nameFont);
        }

        CreateSpacer(64);
      }
    }

    void Update() {
      rect.localPosition = new Vector2(0, rect.localPosition.y + (speed * Time.deltaTime));
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
