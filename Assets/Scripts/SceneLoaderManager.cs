using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoaderManager : MonoBehaviour {

	public Image loadingBar;

	private AsyncOperation asyncLoad;

	// Use this for initialization
	void Start () {
		StartCoroutine("LoadSceneAsync");
	}
	
	// Update is called once per frame
	void Update () {
		if(asyncLoad != null)
		{
			loadingBar.fillAmount = asyncLoad.progress;
		}
	}

	IEnumerator LoadSceneAsync()
	{
		asyncLoad = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("SceneToLoadID"));
		yield return asyncLoad;
	}
}
