using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/List", order = 1)]
public class Item : ScriptableObject {
  public new string name = "New Game Item";
  public Sprite sprite;
  public AudioClip sound;
  public bool collectable = false;
}