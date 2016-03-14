using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartGame : MonoBehaviour {

    public float waitTime;

	// Use this for initialization
	void Start () {
        StartCoroutine(RestartGameAfter(waitTime));
	}
	
    IEnumerator RestartGameAfter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(0);
    }
}
