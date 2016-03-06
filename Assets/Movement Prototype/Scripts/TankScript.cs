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
    Rigidbody2D rb;

    float left_horizontal;
    float left_vertical;
    float right_horizontal;
    float right_vertical;

    State actor1;
    State actor2;

    void Start()
    {
        turret = transform.GetChild(0).gameObject.GetComponent<GunScript>();
        machineGun = transform.GetChild(1).gameObject.GetComponent<GunScript>();
        rb = GetComponent<Rigidbody2D>();

        left_horizontal = 0;
        left_vertical = 0;
        right_horizontal = 0;
        right_vertical = 0;

        actor1 = State.Move;
        actor2 = State.Null;
    }

    void FixedUpdate()
    {
        if (actor1 == State.Move)
        {
            Debug.Log("I am here!");
            rb.AddRelativeForce(new Vector2(0, -left_vertical * speed));
            rb.MoveRotation(rb.rotation - left_horizontal * rotationSpeed * Time.fixedDeltaTime);
        }

        if (actor2 == State.Move)
        {
            rb.AddRelativeForce(new Vector2(0, -right_vertical * speed));
            rb.MoveRotation(rb.rotation - right_horizontal * rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        ControllerScript controller = GetComponent<ControllerScript>();
        // Handle the input for actor1
        switch (actor1)
        {
            case State.Null: break;
            case State.Move: Steer(controller.actor1Horizontal, 1); Accelerate(controller.actor1Vertical, 1); break;
            case State.Turret: RotateTurret(controller.actor1Horizontal, 1); FireGun(controller.actor1Trigger, 1); break;
            case State.MachineGun: RotateTurret(controller.actor1Horizontal, 1); FireGun(controller.actor1Trigger, 1); break;
        }

        // Handle the input for actor2
        switch (actor2)
        {
            case State.Null: break;
            case State.Move: Steer(controller.actor2Horizontal, 2); Accelerate(controller.actor2Vertical, 2); break;
            case State.Turret: RotateTurret(controller.actor2Horizontal, 2); FireGun(controller.actor2Trigger, 2); break; ;
            case State.MachineGun: RotateTurret(controller.actor2Horizontal, 2); FireGun(controller.actor2Trigger, 2); break;
        }
    }

    public void Steer(float horizontal, int agentIndex)
    {
        if (agentIndex == 1)
        {
            if (actor1 == State.Move)
            {
                left_horizontal = horizontal;
            }
        }
        else if (agentIndex == 2)
        {
            if (actor2 == State.Move)
            {
                right_horizontal = horizontal;
            }
        }
    }

    public void Accelerate(float vertical, int agentIndex)
    {
        if (agentIndex == 1)
        {
            if (actor1 == State.Move)
            {
                left_vertical = vertical;
            }
        }
        else if (agentIndex == 2)
        {
            if (actor2 == State.Move)
            {
                right_vertical = vertical;
            }
        }
    }

    public void RotateTurret(float horizontal, int agentIndex)
    {
        if (agentIndex == 1)
        {
            if (actor1 == State.Turret)
            {
                turret.Rotate(horizontal);
            }
            else if (actor1 == State.MachineGun)
            {
                machineGun.Rotate(horizontal);
            }
        }
        else if (agentIndex == 2)
        {
            if (actor2 == State.Turret)
            {
                turret.Rotate(horizontal);
            }
            else if (actor2 == State.MachineGun)
            {
                machineGun.Rotate(horizontal);
            }
        }
    }

    public void FireGun(float trigger, int agentIndex)
    {
        if (agentIndex == 1)
        {
            if (trigger > 0.75f)
            {
                if (actor1 == State.Turret)
                {
                    turret.Fire();
                }
                else if (actor1 == State.MachineGun)
                {
                    machineGun.Fire();
                }
            }
        }
        else if (agentIndex == 2)
        {
            if (trigger > 0.75f)
            {
                if (actor2 == State.Turret)
                {
                    turret.Fire();
                }
                else if (actor2 == State.MachineGun)
                {
                    machineGun.Fire();
                }
            }
        }
    }

    public void SwitchRoles(State newState, int agentIndex)
    {
        if (agentIndex == 1)
        {
            actor1 = newState;
        }
        else if (agentIndex == 2)
        {
            actor2 = newState;
        } 
    }

}
