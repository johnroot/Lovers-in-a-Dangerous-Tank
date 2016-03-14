using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageController : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
    }

    public void ReturnToStart() {
        // 0 is the home scene
        SceneManager.LoadScene(0);
    }
}
