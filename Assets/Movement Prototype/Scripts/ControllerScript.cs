using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour
{
    public int playerIndex;
    public float tankHorizontal;
    public float tankVertical;
    public float turretTrigger;

    public float turretHorizontal;
    public float turretVertical;
    public float machineGunTrigger;

    // Keeping it this way for now so we can still support xbox 360 controller.
    public float agentSwitchHorizontal;
    // public float agent1SwitchVertical;

    public bool turretSelect;
    public bool machineGunSelect;
    public bool droneSpawnSelect;
    public bool driveSelect;

    public bool agent1Reload;
    public bool agent2Reload;

    void Start() {
    }

    void Update() {
        tankHorizontal = Input.GetAxis("L_XAxis_" + playerIndex);
        tankVertical = Input.GetAxis("L_YAxis_" + playerIndex);
        turretTrigger = Input.GetAxis("TriggersL_" + playerIndex);

        turretHorizontal = Input.GetAxis("R_XAxis_" + playerIndex);
        turretVertical = Input.GetAxis("R_YAxis_" + playerIndex);
        machineGunTrigger = Input.GetAxis("TriggersR_" + playerIndex);

        agentSwitchHorizontal = Input.GetAxisRaw("DPad_XAxis_" + playerIndex); // RL
        // agent1SwitchVertical = Input.GetAxisRaw("DPad_YAxis_" + playerIndex); // UD

        machineGunSelect = Input.GetButton("B_" + playerIndex); // Right
        turretSelect = Input.GetButton("X_" + playerIndex); // Left
        droneSpawnSelect = Input.GetButton("Y_" + playerIndex); // Up
        driveSelect = Input.GetButton("A_" + playerIndex);

        agent1Reload = Input.GetButton("LB_" + playerIndex);
        agent2Reload = Input.GetButton("RB_" + playerIndex);
    }
}