using UnityEngine;
using System.Collections;

public class TankMovementScript : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public int turretFireRate;
    public int machineGunFireRate;

    public GameObject turretBullet;
    public GameObject machineGunBullet;

    int turretReload;
    int machineGunReload;

    GameObject turret;
    GameObject machineGun;
    Rigidbody2D rb;

    float left_horizontal;
    float left_vertical;
    float right_horizontal;
    float right_vertical;

    enum State { Null, Move, Turret, MachineGun };
    State actor1;
    State actor2;

    // Use this for initialization
    void Start()
    {
        turret = transform.GetChild(0).gameObject;
        machineGun = transform.GetChild(1).gameObject;
        rb = GetComponent<Rigidbody2D>();

        left_horizontal = 0;
        left_vertical = 0;
        right_horizontal = 0;
        right_vertical = 0;

        actor1 = State.Move;
        actor2 = State.Null;
    }

    // FixedUpdate accesses analog input from the sticks for movement
    void FixedUpdate()
    {
        if (actor1 == State.Move)
        {
            rb.AddRelativeForce(new Vector2(0, -left_vertical * speed));
            rb.MoveRotation(rb.rotation - left_horizontal * rotationSpeed * Time.fixedDeltaTime);
        }

        if (actor2 == State.Move)
        {
            rb.AddRelativeForce(new Vector2(0, -right_vertical * speed));
            rb.MoveRotation(rb.rotation - right_horizontal * rotationSpeed * Time.fixedDeltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        left_horizontal = Input.GetAxis("L_XAxis_1");
        left_vertical = Input.GetAxis("L_YAxis_1");

        right_horizontal = Input.GetAxis("R_XAxis_1");
        right_vertical = Input.GetAxis("R_YAxis_1");

        SwitchRoles();
        RotateGuns();
        FireGuns();
    }

    void SwitchRoles()
    {
        // Handle role switching for left hand
        float horizontal = Input.GetAxisRaw("DPad_XAxis_1");
        float vertical = Input.GetAxisRaw("DPad_YAxis_1");

        if (horizontal != 0.0f)
        {
            if (horizontal == 1.0f && actor2 != State.Turret) // Right
            {
                actor1 = State.Turret;
            }
            else if (horizontal == -1.0f && actor2 != State.MachineGun) // Left
            {
                actor1 = State.MachineGun;
            }
        }
        else
        {
            if (vertical == 1.0f && actor2 != State.Move) // Up
            {
                actor1 = State.Move;
            }
            else if (vertical == -1.0f) // Down
            {
                actor1 = State.Null;
            }
        }

        // Handle role switching for right hand
        if (Input.GetButton("B_1") && actor1 != State.Turret) // Right
        {
            actor2 = State.Turret;
        }
        else if (Input.GetButton("X_1") && actor1 != State.MachineGun) // Left
        {
            actor2 = State.MachineGun;
        }
        else if (Input.GetButton("Y_1") && actor1 != State.Move) // Up
        {
            actor2 = State.Move;
        }
        else if (Input.GetButton("A_1")) // Down
        {
            actor2 = State.Null;
        }
    }

    void RotateGuns()
    {
        if (actor1 == State.Turret || actor1 == State.MachineGun)
        {
            Transform transformToRotate = null;

            if (actor1 == State.Turret)
            {
                transformToRotate = turret.transform;
            }
            else if (actor1 == State.MachineGun)
            {
                transformToRotate = machineGun.transform;
            }

            if (transformToRotate != null)
            {
                transformToRotate.Rotate(0, 0, rotationSpeed * -left_horizontal * Time.deltaTime);
            }
        }

        if (actor2 == State.Turret || actor2 == State.MachineGun)
        {
            Transform transformToRotate = null;

            if (actor2 == State.Turret)
            {
                transformToRotate = turret.transform;
            }
            else if (actor2 == State.MachineGun)
            {
                transformToRotate = machineGun.transform;
            }

            if (transformToRotate != null)
            {
                transformToRotate.Rotate(0, 0, rotationSpeed * -right_horizontal * Time.deltaTime);
            }
        }
    }

    void FireGuns()
    {
        if (turretReload < turretFireRate)
        {
            turretReload++;
        }

        if (machineGunReload < machineGunFireRate)
        {
            machineGunReload++;
        }

        float leftTrigger = Input.GetAxis("TriggersL_1");
        float rightTrigger = Input.GetAxis("TriggersR_1");

        if (leftTrigger > 0.75f)
        {
            if (actor1 == State.Turret && turretReload == turretFireRate)
            {
                Instantiate(turretBullet, turret.transform.position, turret.transform.rotation);
                turretReload = 0;
            }
            if (actor1 == State.MachineGun && machineGunReload == machineGunFireRate)
            {
                Instantiate(machineGunBullet, machineGun.transform.position, machineGun.transform.rotation);
                machineGunReload = 0;
            }
        }

        if (rightTrigger > 0.75f)
        {
            if (actor2 == State.Turret && turretReload == turretFireRate)
            {
                Instantiate(turretBullet, turret.transform.position, turret.transform.rotation);
                turretReload = 0;
            }
            if (actor2 == State.MachineGun && machineGunReload == machineGunFireRate)
            {
                Instantiate(machineGunBullet, machineGun.transform.position, machineGun.transform.rotation);
                machineGunReload = 0;
            }
        }
    }
}
