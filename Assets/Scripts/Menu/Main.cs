using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu {
  public class Main : MonoBehaviour {

    public Button buttonPlay;
    public Button buttonCredits;
    public Button buttonQuit;

    void Start() {
      buttonPlay = GameObject.Find("Play").GetComponent<Button>();
      buttonCredits = GameObject.Find("Credits").GetComponent<Button>();
      buttonQuit = GameObject.Find("Quit").GetComponent<Button>();
      buttonPlay.onClick.AddListener(() => Controller.current.LoadScene ("Forest"));
      buttonCredits.onClick.AddListener(() => Controller.current.LoadScene ("EndCredits"));
      buttonQuit.onClick.AddListener(() => Application.Quit());	

      buttonQuit.gameObject.SetActive(false);
      #if UNITY_STANDALONE_WIN
      buttonQuit.gameObject.SetActive(true);
      #endif
    }
  }
}
