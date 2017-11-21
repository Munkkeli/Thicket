using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public Button buttonPlay;
	public Button buttonCredits;
	public Button buttonQuit;

	void Start() {
		buttonPlay = GameObject.Find("Play").GetComponent<Button>();
		buttonCredits = GameObject.Find("Credits").GetComponent<Button>();
		buttonQuit = GameObject.Find("Quit").GetComponent<Button>();
		buttonPlay.onClick.AddListener(() => SceneManager.LoadScene ("Forest"));
		buttonCredits.onClick.AddListener(() => SceneManager.LoadScene ("Credits"));
		//buttonQuit.onClick.AddListener(() => Application.Quit);	
	}
}
