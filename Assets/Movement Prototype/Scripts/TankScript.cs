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
    float agent1Horizontal;
    float agent1Vertical;
    float agent2Horizontal;
    float agent2Vertical;

    State agent1;
    State agent2;

	float friction = 1f;
	Vector2 agent1CurrentVel = Vector2.zero;
	Vector2 agent2CurrentVel = Vector2.zero;

    AudioSource audioSource;

    // State switching cooldown for agents 1 and 2
    int agent1SwitchTimer;
    int agent2SwitchTimer;
    public int agentSwitchCooldown;

    void Start()
    {
        turret = transform.Find("Turret").gameObject.GetComponent<GunScript>();
        machineGun = transform.Find("MachineGun").gameObject.GetComponent<GunScript>();
        droneSpawner = gameObject.GetComponent<DroneSpawnerScript>();
        controller = GetComponent<ControllerScript>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        agent1Horizontal = 0;
        agent1Vertical = 0;
        agent2Horizontal = 0;
        agent2Vertical = 0;

        agent1 = State.Move;
        agent2 = State.MachineGun;

        agent1SwitchTimer = agentSwitchCooldown;
        agent2SwitchTimer = agentSwitchCooldown;
    }

    void FixedUpdate() {
        if (agent1 == State.Move) {
			agent1CurrentVel = Vector2.Lerp(agent1CurrentVel, new Vector2 (agent1Vertical * speed, 0), friction*friction);
			rb.AddRelativeForce(agent1CurrentVel);
			rb.AddTorque(-agent1Horizontal * rotationSpeed);
        }

        if (agent2 == State.Move) {
			agent2CurrentVel = Vector2.Lerp(agent2CurrentVel, new Vector2 (agent2Vertical * speed, 0), friction*friction);
			rb.AddRelativeForce(agent2CurrentVel);
			rb.AddTorque(-agent2Horizontal * rotationSpeed * 1/friction);
        }

        audioSource.volume = rb.velocity.magnitude / (speed * 2) + Mathf.Abs(rb.angularVelocity) / 1000;
    }

    void Update()
    {
        // Handle the input for agent1
        switch (agent1)
        {
            case State.Null:
                break;
            case State.Move:
                // Debug.Log("Agent 1 is moving");
                Steer(controller.agent1Horizontal, 1);
                Accelerate(controller.agent1Vertical, 1);
                break;
            case State.Turret:
            case State.MachineGun:
                RotateTurret(controller.agent1Horizontal, 1);
                FireGun(controller.agent1Trigger, 1);
                ReloadGun(controller.agent1Reload, 1);
                break;
            case State.SpawnDrone:
                SpawnDrone(true, 1); // TODO: find a button for to spawn the drone
                break;
        }

        // Handle the input for agent2
        switch (agent2)
        {
            case State.Null:
                break;
            case State.Move:
                // Debug.Log("Agent 2 is moving");
                Steer(controller.agent2Horizontal, 2);
                Accelerate(controller.agent2Vertical, 2);
                break;
            case State.Turret:
            case State.MachineGun:
                RotateTurret(controller.agent2Horizontal, 2);
                FireGun(controller.agent2Trigger, 2);
                ReloadGun(controller.agent2Reload, 2);
                break;
        }
        SwitchRoles();
    }

    public void Steer(float horizontal, int agentIndex)
    {
        if (agentIndex == 1)
        {
            if (agent1 == State.Move)
            {
                agent1Horizontal = horizontal;
            }
        }
        else if (agentIndex == 2)
        {
            if (agent2 == State.Move)
            {
                agent2Horizontal = horizontal;
            }
        }
    }

    public void Accelerate(float vertical, int agentIndex)
    {
        if (agentIndex == 1)
        {
            if (agent1 == State.Move)
            {
                agent1Vertical = vertical;
            }
        }
        else if (agentIndex == 2)
        {
            if (agent2 == State.Move)
            {
                agent2Vertical = vertical;
            }
        }
    }

    public void RotateTurret(float horizontal, int agentIndex)
    {
        State agent = getAgent(agentIndex);
        if (agent == State.Turret)
        {
            turret.Rotate(horizontal);
        }
        else if (agent == State.MachineGun)
        {
            machineGun.Rotate(horizontal);
        }
        if (horizontal != 0)
        {
            Debug.Log("Rotating " + agent + " for agent: " + agentIndex);
        }
    }

    public void FireGun(float trigger, int agentIndex)
    {
        State agent = getAgent(agentIndex);
        if (trigger > 0.75f)
        {
            Debug.Log("Trigger detected: " + trigger + " for agent: " + agentIndex);
            if (agent == State.Turret)
            {
                Debug.Log("Agent detected");
                GameObject bullet = turret.Fire();
                setLayerOfFiredBullet(bullet);
            }
            else if (agent == State.MachineGun)
            {
                GameObject bullet = machineGun.Fire();
                setLayerOfFiredBullet(bullet);
            }
        }
    }

    public void ReloadGun(bool reloadTriggered, int agentIndex)
    {
        if (reloadTriggered)
        {
            State agent = getAgent(agentIndex);
            if (agent == State.Turret)
            {
                turret.Reload();
            }
            else if (agent == State.MachineGun)
            {
                machineGun.Reload();
            }
        }
    }

    public void SpawnDrone(bool spawningTriggered, int agentIndex)
    {
        if (spawningTriggered)
        {
            State agent = getAgent(agentIndex);
            if (agent == State.SpawnDrone)
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

    public void SwitchRoles()
    {
        // Handle role switching for left hand
        if (agent1SwitchTimer > agentSwitchCooldown)
        {
            if (controller.agent1SwitchHorizontal == 1.0f && agent2 != State.Turret)
            {
                Debug.Log("1.Turret");
                agent1 = State.Turret;
                OperatorL.transform.localPosition = new Vector3(-15, 2);
                agent1SwitchTimer = 0;
            }
            else if (controller.agent1SwitchHorizontal == -1.0f && agent2 != State.MachineGun)
            {
                Debug.Log("1.MachineGun");
                agent1 = State.MachineGun;
                OperatorL.transform.localPosition = new Vector3(-95, 50);
                agent1SwitchTimer = 0;
            }
            else if (controller.agent1SwitchVertical == 1.0f && agent2 != State.Move)
            {
                Debug.Log("1.Move");
                agent1 = State.Move;
                OperatorL.transform.localPosition = new Vector3(-95, -35);
                agent1SwitchTimer = 0;
            }
            else if (controller.agent1SwitchVertical == -1.0f && agent2 != State.SpawnDrone)
            {
                Debug.Log("1.SpawnDrone");
                agent1 = State.SpawnDrone;
                OperatorL.transform.localPosition = new Vector3(-55, -50); // TODO(denis): Marshall figure where this should actually be
            }
        }
        else
        {
            agent1SwitchTimer++;
        }

        if (agent2SwitchTimer > agentSwitchCooldown)
        {
            // Handle role switching for right hand
            if (controller.agent2TurretSelect && agent1 != State.Turret)
            {
                Debug.Log("2.Turret");
                agent2 = State.Turret;
                OperatorR.transform.localPosition = new Vector3(-15, 2);
                agent2SwitchTimer = 0;
            }
            else if (controller.agent2MachineGunSelect && agent1 != State.MachineGun)
            {
                Debug.Log("2.MachineGun");
                agent2 = State.MachineGun;
                OperatorR.transform.localPosition = new Vector3(-95, 50);
                agent2SwitchTimer = 0;
            }
            else if (controller.agent2MoveSelect && agent1 != State.Move)
            {
                Debug.Log("2.Move");
                agent2 = State.Move;
                OperatorR.transform.localPosition = new Vector3(-95, -35);
                agent2SwitchTimer = 0;
            }
        }
        else
        {
            agent2SwitchTimer++;
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
