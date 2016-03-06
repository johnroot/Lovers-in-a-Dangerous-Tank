using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour
{
    public float agent1Horizontal;
    public float agent1Vertical;
    public float agent1Trigger;

    public float agent2Horizontal;
    public float agent2Vertical;
    public float agent2Trigger;

    // Keeping it this way for now so we can still support xbox 360 controller.
    public float agent1SwitchHorizontal;
    public float agent1SwitchVertical;

    public bool agent2TurretSelect;
    public bool agent2MachineGunSelect;
    public bool agent2MoveSelect;
    public bool agent2NullSelect;

    void Start() {
    }

    void Update() {
        agent1Horizontal = Input.GetAxis("L_XAxis_1");
        agent1Vertical = Input.GetAxis("L_YAxis_1");
        agent1Trigger = Input.GetAxis("TriggersL_1");

        agent2Horizontal = Input.GetAxis("R_XAxis_1");
        agent2Vertical = Input.GetAxis("R_YAxis_1");
        agent2Trigger = Input.GetAxis("TriggersR_1");

        agent1SwitchHorizontal = Input.GetAxisRaw("DPad_XAxis_1"); // RL
        agent1SwitchVertical = Input.GetAxisRaw("DPad_YAxis_1"); // UD

        agent2TurretSelect = Input.GetButton("B_1"); // Right
        agent2MachineGunSelect = Input.GetButton("X_1"); // Left
        agent2MoveSelect = Input.GetButton("Y_1"); // Up
        agent2NullSelect = Input.GetButton("A_1");
    }
}