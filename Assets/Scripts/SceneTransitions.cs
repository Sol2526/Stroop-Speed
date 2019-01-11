using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour {

    public Animator transitionAnim;
    public string sceneName;
    public float loadTime;

    public void LoadSce () {
        StartCoroutine(LoadScene());
	}

    IEnumerator LoadScene() {
        //transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(sceneName);
    }
}
