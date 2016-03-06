using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {

    public float rotationSpeed;
    public float fireRate;
    public GameObject turretBullet;
    public int ammo;

    int timeElapsedSinceFire = 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        timeElapsedSinceFire++;
	}

    /**
     * TODO: finish doc
     * Rotate turret based on the horizontal
     * @param horizontal - horizontal axis position
     */
    public void Rotate (float horizontal) {
        transform.Rotate(0, 0, rotationSpeed * horizontal * Time.deltaTime);
    }

    public void Fire() {
        if (timeElapsedSinceFire < fireRate) {
            Instantiate(turretBullet, transform.position, transform.rotation);
            timeElapsedSinceFire = 0;
        }
    }
}
