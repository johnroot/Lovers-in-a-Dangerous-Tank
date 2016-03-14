using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour
{
    public int playerIndex;
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
    public bool agent2SpawnDroneSelect;
    public bool agent2NullSelect;

    public bool agent1Reload;
    public bool agent2Reload;

    void Start() {
    }

    void Update() {
        agent1Horizontal = Input.GetAxis("L_XAxis_" + playerIndex);
        agent1Vertical = Input.GetAxis("L_YAxis_" + playerIndex);
        agent1Trigger = Input.GetAxis("TriggersL_" + playerIndex);

        agent2Horizontal = Input.GetAxis("R_XAxis_" + playerIndex);
        agent2Vertical = Input.GetAxis("R_YAxis_" + playerIndex);
        agent2Trigger = Input.GetAxis("TriggersR_" + playerIndex);

        agent1SwitchHorizontal = Input.GetAxisRaw("DPad_XAxis_" + playerIndex); // RL
        agent1SwitchVertical = Input.GetAxisRaw("DPad_YAxis_" + playerIndex); // UD

        agent2TurretSelect = Input.GetButton("B_" + playerIndex); // Right
        agent2MachineGunSelect = Input.GetButton("X_" + playerIndex); // Left
        agent2MoveSelect = Input.GetButton("Y_" + playerIndex); // Up
        agent2SpawnDroneSelect = Input.GetButton("A_" + playerIndex);

        agent1Reload = Input.GetButton("LB_" + playerIndex);
        agent2Reload = Input.GetButton("RB_" + playerIndex);
    }
}