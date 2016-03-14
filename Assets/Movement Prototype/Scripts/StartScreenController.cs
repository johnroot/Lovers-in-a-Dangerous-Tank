using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreenController : MonoBehaviour
{
    public Toggle player1Toggle;
    public Toggle player2Toggle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for the existance of joysticks.
        int numJoysticks = Input.GetJoystickNames().Length;
        player1Toggle.isOn = numJoysticks >= 1 ? true : false;
        player2Toggle.isOn = numJoysticks >= 2 ? true : false;
    }

    public void StartGame(int levelNumber)
    {
        if (player1Toggle.isOn && player2Toggle.isOn)
        {
            SceneManager.LoadScene(levelNumber);
        }
    }
}
