using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    public string currentScene;
    public bool OnTV;

	// Use this for initialization
	void Awake () {

        StartCoroutine(WaitForReady());
	}
	
	IEnumerator WaitForReady() {

        // Singletons are not created until they are called, so create them all here.
        yield return new WaitUntil(delegate { return GameManager.Instance != null; });
        yield return new WaitUntil(delegate { return EventManager.Instance != null; });
        yield return new WaitUntil(delegate { return TouchManager.Instance != null; });
        Debug.Log("Application Ready!");
        SceneManager.LoadScene("Main");
    }
}
