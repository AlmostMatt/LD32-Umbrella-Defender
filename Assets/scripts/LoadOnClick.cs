using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	
	//public GameObject loadingImage;
	public Button play;

	void Start() {
		play.onClick.AddListener(() => { Application.LoadLevel("2d"); });
	}
}