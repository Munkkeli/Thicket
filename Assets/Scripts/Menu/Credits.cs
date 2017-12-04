using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu {
	public class Credits : MonoBehaviour {

		public Button buttonBack;


		void Start(){
			buttonBack = GameObject.Find("Back").GetComponent<Button>();
			buttonBack.onClick.AddListener(() => SceneManager.LoadScene ("Menu"));

		}
	}
}