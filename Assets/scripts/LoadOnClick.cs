using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {
	
	//public GameObject loadingImage;
	public Button play;

	void Start() {
		play.onClick.AddListener(() => { SceneManager.LoadScene("2d"); });
	}
}