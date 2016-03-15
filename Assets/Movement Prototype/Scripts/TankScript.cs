using UnityEngine;
using System.Collections;

public class TankScript : MonoBehaviour
{

    public enum State { Null, Move, Turret, MachineGun, SpawnDrone };

    public GameObject enemy;
    public float speed;
    public float rotationSpeed;
    public int turretFireRate;
    public int machineGunFireRate;

    public GameObject turretBullet;
    public GameObject machineGunBullet;
    public GameObject OperatorL;
    public GameObject OperatorR;

    int turretReload;
    int machineGunReload;

    GunScript turret;
    GunScript machineGun;
    DroneSpawnerScript droneSpawner;
    ControllerScript controller;
    Rigidbody2D rb;

    // Axes for agents 1 and 2
    float tankHorizontal;
    float tankVertical;
    float agent2Horizontal;
    float agent2Vertical;

    State agent1;
    State agent2;

	float friction = 1f;
	Vector2 tankCurrentVel = Vector2.zero;

    AudioSource audioSource;

    // State switching cooldown for agents 1 and 2
    int agentSwitchTimer;
    // int agent2SwitchTimer;
    public int agentSwitchCooldown;

    bool inSwitchMode = false;
    int agentToSwitch = 0;

    void Start()
    {
        turret = transform.Find("Turret").gameObject.GetComponent<GunScript>();
        machineGun = transform.Find("MachineGun").gameObject.GetComponent<GunScript>();
        droneSpawner = gameObject.GetComponent<DroneSpawnerScript>();
        controller = GetComponent<ControllerScript>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        tankHorizontal = 0;
        tankVertical = 0;
        agent2Horizontal = 0;
        agent2Vertical = 0;

        agent1 = State.Move;
        OperatorL.transform.localPosition = new Vector3(-25, 110);
        agent2 = State.MachineGun;
        OperatorR.transform.localPosition = new Vector3(35, 110);

        agentSwitchTimer = agentSwitchCooldown;
    }

    void FixedUpdate() {
        if (agent1 == State.Move || agent2 == State.Move) {
			tankCurrentVel = Vector2.Lerp(tankCurrentVel, new Vector2 (tankVertical * speed, 0), friction*friction);
			rb.AddRelativeForce(tankCurrentVel);
			rb.AddTorque(-tankHorizontal * rotationSpeed);
        }

        audioSource.volume = rb.velocity.magnitude / (speed * 2) + Mathf.Abs(rb.angularVelocity) / 1000;
    }

    void Update()
    {
        if (controller.tankHorizontal != 0 || controller.tankVertical != 0 ||
            controller.turretTrigger > 0.25f || controller.machineGunTrigger > 0.25f ||
            controller.turretHorizontal != 0) {
            inSwitchMode = false;
            agentToSwitch = 0;
        }
        // Handle the input for agent1
        switch (agent1)
        {
            case State.Null:
                break;
            case State.Move:
                // Debug.Log("Agent 1 is moving");
                Steer(controller.tankHorizontal);
                Accelerate(controller.tankVertical);
                break;
            case State.Turret:
                RotateTurret(controller.turretHorizontal);
                FireGun(controller.turretTrigger, true);
                // ReloadGun(controller.turretReload);
                break;
            case State.MachineGun:
                FireGun(controller.machineGunTrigger, false);
                // ReloadGun(controller.agent1Reload, 1);
                break;
            case State.SpawnDrone:
                SpawnDrone(true); // TODO: find a button for to spawn the drone
                break;
        }

        // Handle the input for agent2
        switch (agent2)
        {
            case State.Null:
                break;
            case State.Move:
                // Debug.Log("Agent 2 is moving");
                Steer(controller.tankHorizontal);
                Accelerate(controller.tankVertical);
                break;
            case State.Turret:
                RotateTurret(controller.turretHorizontal);
                FireGun(controller.turretTrigger, true);
                break;
            case State.MachineGun:
                FireGun(controller.machineGunTrigger, false);
                break;
            case State.SpawnDrone:
                SpawnDrone(true); // TODO: find a button for to spawn the drone
                break;
        }
        SwitchRoles();
    }

    public void Steer(float horizontal)
    {
        tankHorizontal = horizontal;
    }

    public void Accelerate(float vertical)
    {
        tankVertical = vertical;
    }

    public void RotateTurret(float horizontal)
    {
        if (agent1 == State.Turret || agent2 == State.Turret)
        {
            turret.Rotate(horizontal);
        }
        // if (horizontal != 0)
        // {
        //     Debug.Log("Rotating " + agent + " for agent: " + agentIndex);
        // }
    }

    public void FireGun(float trigger, bool isTurret)
    {
        if (trigger > 0.75f)
        {
            // Debug.Log("Trigger detected: " + trigger + " for agent: " + agentIndex);
            if (isTurret)
            {
                // Debug.Log("Agent detected");
                GameObject bullet = turret.Fire();
                setLayerOfFiredBullet(bullet);
            }
            else {
                GameObject bullet = machineGun.Fire();
                setLayerOfFiredBullet(bullet);
            }
        }
    }

    // public void ReloadGun(bool reloadTriggered)
    // {
    //     if (reloadTriggered)
    //     {
    //         if (agent == State.Turret)
    //         {
    //             turret.Reload();
    //         }
    //         else if (agent == State.MachineGun)
    //         {
    //             machineGun.Reload();
    //         }
    //     }
    // }

    public void SpawnDrone(bool spawningTriggered)
    {
        if (enemy != null)
        {
            if (spawningTriggered)
            {
                if (agent1 == State.SpawnDrone || agent2 == State.SpawnDrone)
                {
                    GameObject drone = droneSpawner.SpawnDrone();
                    if (drone != null)
                    {
                        Debug.Log("Drone actually spawned!");
                        drone.layer = LayerMask.NameToLayer("Player" + controller.playerIndex + "Drone"); // e.g. Player1Drone
                        drone.GetComponent<DroneScript>().target = enemy;
                    }
                }
            }
        }
    }

    public void SwitchRoles()
    {
        // If not in switch mode, and timer has passed
        // This is the switch for both
        if (!inSwitchMode && agentSwitchTimer > agentSwitchCooldown) {
            // If buttons pressed, we turn in switch mode to be true
            if (controller.agentSwitchHorizontal == 1.0f) {
                inSwitchMode = true;
                agentToSwitch = 2;
                agentSwitchTimer = 0;
            } else if (controller.agentSwitchHorizontal == -1.0f) {
                inSwitchMode = true;
                agentToSwitch = 1;
                agentSwitchTimer = 0;
            }
        } else {
            // Increase timer passively
            agentSwitchTimer++;
        }


        // Handle role switching for left hand
        if (inSwitchMode)
        {
            if (agentToSwitch == 1) {

                if (controller.turretSelect && agent2 != State.Turret)
                {
                    Debug.Log("1.Turret");
                    agent1 = State.Turret;
                    OperatorL.transform.localPosition = new Vector3(0, 15);
                    inSwitchMode = false;
                }
                else if (controller.machineGunSelect && agent2 != State.MachineGun)
                {
                    Debug.Log("1.MachineGun");
                    agent1 = State.MachineGun;
                    OperatorL.transform.localPosition = new Vector3(35, 110);
                    inSwitchMode = false;
                }
                else if (controller.driveSelect && agent2 != State.Move)
                {
                    Debug.Log("1.Move");
                    agent1 = State.Move;
                    OperatorL.transform.localPosition = new Vector3(-25, 110);
                    inSwitchMode = false;
                }
                else if (controller.droneSpawnSelect && agent2 != State.SpawnDrone)
                {
                    Debug.Log("1.SpawnDrone");
                    agent1 = State.SpawnDrone;
                    OperatorL.transform.localPosition = new Vector3(0, -145); // TODO(denis): Marshall figure where this should actually be
                    inSwitchMode = false;
                }
            }

            if (agentToSwitch == 2) {
                if (controller.turretSelect && agent1 != State.Turret)
                {
                    Debug.Log("2.Turret");
                    agent2 = State.Turret;
                    OperatorR.transform.localPosition = new Vector3(0, 15);
                    inSwitchMode = false;
                }
                else if (controller.machineGunSelect && agent1 != State.MachineGun)
                {
                    Debug.Log("2.MachineGun");
                    agent2 = State.MachineGun;
                    OperatorR.transform.localPosition = new Vector3(35, 110);
                    inSwitchMode = false;
                }
                else if (controller.driveSelect && agent1 != State.Move)
                {
                    Debug.Log("2.Move");
                    agent2 = State.Move;
                    OperatorR.transform.localPosition = new Vector3(-25, 110);
                    inSwitchMode = false;
                }
                else if (controller.droneSpawnSelect && agent1 != State.SpawnDrone)
                {
                    Debug.Log("2.SpawnDrone");
                    agent2 = State.SpawnDrone;
                    OperatorR.transform.localPosition = new Vector3(0, -145); // TODO(denis): Marshall figure where this should actually be
                    inSwitchMode = false;
                }
            }

        }

    }

    private void setLayerOfFiredBullet(GameObject bullet)
    {
        if (bullet != null)
        {
            bullet.layer = gameObject.layer;
        }
    }

    private State getAgent(int agentIndex)
    {
        if (agentIndex == 1)
        {
            return agent1;
        }
        else if (agentIndex == 2)
        {
            return agent2;
        }
        return State.Null;
    }

	//for handling friciton on different surfaces
	private void OnTriggerEnter2D(Collider2D other) {
		friction = other.sharedMaterial.friction;
	}
	private void OnTriggerExit2D(Collider2D other) {
		friction = 1f;
	}
}
