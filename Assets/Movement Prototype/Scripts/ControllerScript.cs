using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour
{
    public float actor1Horizontal;
    public float actor1Vertical;

    public float actor2Horizontal;
    public float actor2Vertical;

    public float actor1Trigger;
    public float actor2Trigger;

    void Start() {

    }

    void Update() {
        actor1Horizontal = Input.GetAxis("L_XAxis_1");
        actor1Vertical = Input.GetAxis("L_YAxis_1");

        actor2Horizontal = Input.GetAxis("R_XAxis_1");
        actor2Vertical = Input.GetAxis("R_YAxis_1");

        actor1Trigger = Input.GetAxis("TriggersL_1");
        actor2Trigger = Input.GetAxis("TriggersR_1");
    }
}