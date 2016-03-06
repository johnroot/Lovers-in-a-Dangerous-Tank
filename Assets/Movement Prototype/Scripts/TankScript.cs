using UnityEngine;
using System.Collections;

public class TankScript : MonoBehaviour {

    public enum State { Null, Move, Turret, MachineGun };

    public float speed;
    public float rotationSpeed;
    public int turretFireRate;
    public int machineGunFireRate;

    public GameObject turretBullet;
    public GameObject machineGunBullet;

    int turretReload;
    int machineGunReload;

    GunScript turret;
    GunScript machineGun;
    ControllerScript controller;
    Rigidbody2D rb;

    // Axes for agents 1 and 2
    float agent1Horizontal;
    float agent1Vertical;
    float agent2Horizontal;
    float agent2Vertical;

    State agent1;
    State agent2;

    void Start() {
        turret = transform.GetChild(0).gameObject.GetComponent<GunScript>();
        machineGun = transform.GetChild(1).gameObject.GetComponent<GunScript>();
        controller = GetComponent<ControllerScript>();
        rb = GetComponent<Rigidbody2D>();

        agent1Horizontal = 0;
        agent1Vertical = 0;
        agent2Horizontal = 0;
        agent2Vertical = 0;

        agent1 = State.Move;
        agent2 = State.Null;
    }

    void FixedUpdate() {
        if (agent1 == State.Move) {
            rb.AddRelativeForce(new Vector2(0, -agent1Vertical * speed));
            rb.MoveRotation(rb.rotation - agent1Horizontal * rotationSpeed * Time.fixedDeltaTime);
        }

        if (agent2 == State.Move) {
            rb.AddRelativeForce(new Vector2(0, -agent2Vertical * speed));
            rb.MoveRotation(rb.rotation - agent2Horizontal * rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void Update() {
        // Handle the input for agent1
        switch (agent1) {
            case State.Null:
                break;
            case State.Move:
                Debug.Log("Agent 1 is moving");
                Steer(controller.agent1Horizontal, 1);
                Accelerate(controller.agent1Vertical, 1);
                break;
            case State.Turret:
            case State.MachineGun:
                RotateTurret(controller.agent1Horizontal, 1);
                FireGun(controller.agent1Trigger, 1);
                ReloadGun(controller.agent1Reload, 1);
                break;
        }

        // Handle the input for agent2
        switch (agent2) {
            case State.Null:
                break;
            case State.Move:
                Debug.Log("Agent 2 is moving");
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

    public void Steer(float horizontal, int agentIndex) {
        if (agentIndex == 1) {
            if (agent1 == State.Move) {
                agent1Horizontal = horizontal;
            }
        } else if (agentIndex == 2) {
            if (agent2 == State.Move) {
                agent2Horizontal = horizontal;
            }
        }
    }

    public void Accelerate(float vertical, int agentIndex) {
        if (agentIndex == 1) {
            if (agent1 == State.Move) {
                agent1Vertical = vertical;
            }
        }
        else if (agentIndex == 2) {
            if (agent2 == State.Move) {
                agent2Vertical = vertical;
            }
        }
    }

    public void RotateTurret(float horizontal, int agentIndex) {
        State agent = getAgent(agentIndex);
        if (agent == State.Turret) {
            Debug.Log("Rotating TURRET for agent: " + agentIndex);
            turret.Rotate(horizontal);
        } else if (agent == State.MachineGun) {
            Debug.Log("Rotating MACHINEGUN for agent: " + agentIndex);
            machineGun.Rotate(horizontal);
        }
    }

    public void FireGun(float trigger, int agentIndex) {
        State agent = getAgent(agentIndex);
        if (trigger > 0.75f) {
            Debug.Log("Trigger detected: " + trigger + " for agent: " + agentIndex);
            if (agent == State.Turret) {
                Debug.Log("Agent detected");
                GameObject bullet = turret.Fire();
                setLayerOfFiredBullet(bullet);
            } else if (agent == State.MachineGun) {
                GameObject bullet = machineGun.Fire();
                setLayerOfFiredBullet(bullet);
            }
        }
    }

    public void ReloadGun(bool reloadTriggered, int agentIndex) {
        if (reloadTriggered) {
            State agent = getAgent(agentIndex);
            if (agent == State.Turret) {
                turret.Reload();
            } else if (agent == State.MachineGun) {
                machineGun.Reload();
            }
        }
    }

    public void SwitchRoles() {
        // Handle role switching for left hand
        Debug.Log("horizontal" + controller.agent1SwitchHorizontal);
        Debug.Log("Vertical" + controller.agent1SwitchVertical);
        if (controller.agent1SwitchHorizontal == 1.0f && agent2 != State.Turret) {
            Debug.Log("1.Turret");
            agent1 = State.Turret;
        } else if (controller.agent1SwitchHorizontal == -1.0f && agent2 != State.MachineGun) {
            Debug.Log("1.MachineGun");
            agent1 = State.MachineGun;
        } else if (controller.agent1SwitchVertical == 1.0f && agent2 != State.Move) {
            Debug.Log("1.Move");
            agent1 = State.Move;
        }

        // Handle role switching for right hand
        if (controller.agent2TurretSelect && agent1 != State.Turret) {
            Debug.Log("2.Turret");
            agent2 = State.Turret;
        } else if (controller.agent2MachineGunSelect && agent1 != State.MachineGun) {
            Debug.Log("2.MachineGun");
            agent2 = State.MachineGun;
        } else if (controller.agent2MoveSelect && agent1 != State.Move) {
            Debug.Log("2.Move");
            agent2 = State.Move;
        }
    }

    // public void SwitchRoles(State newState, int agentIndex) {
    //     if (agentIndex == 1) {
    //         agent1 = newState;
    //     } else if (agentIndex == 2) {
    //         agent2 = newState;
    //     }
    // }

        private void setLayerOfFiredBullet(GameObject bullet)
    {
        if (bullet != null)
        {
            bullet.layer = gameObject.layer;
        }
    }

    private State getAgent(int agentIndex) {
        if (agentIndex == 1) {
            return agent1;
        } else if (agentIndex == 2) {
            return agent2;
        }
        return State.Null;
    }

}
