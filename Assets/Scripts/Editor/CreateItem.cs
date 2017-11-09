using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateItem {
  [MenuItem("Assets/Create/Game Item")]
  public static void Create() {
    Item asset = ScriptableObject.CreateInstance<Item>();

    AssetDatabase.CreateAsset(asset, "Assets/Items/New Game Item.asset");
    AssetDatabase.SaveAssets();

    EditorUtility.FocusProjectWindow();

    Selection.activeObject = asset;
  }
}