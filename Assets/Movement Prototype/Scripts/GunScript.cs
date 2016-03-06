using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {

    public float rotationSpeed;
    public float fireRate;
    public GameObject turretBullet;
    public int maxAmmo;
    public int reloadAmount;
    int currentAmmo;

    int timeElapsedSinceFire = 0;

	// Use this for initialization
	void Start () {
        currentAmmo = maxAmmo;
	}

	// Update is called once per frame
	void Update () {
        if (timeElapsedSinceFire < fireRate) {
            timeElapsedSinceFire++;
        }
	}

    /**
     * TODO: finish doc
     * Rotate turret based on the horizontal
     * @param horizontal - horizontal axis position
     */
    public void Rotate (float horizontal) {
        transform.Rotate(0, 0, rotationSpeed * horizontal * Time.deltaTime);
    }

    public GameObject Fire() {
        if (timeElapsedSinceFire == fireRate && currentAmmo > 0) {
            GameObject bullet = (GameObject) Instantiate(turretBullet, transform.position, transform.rotation);
            timeElapsedSinceFire = 0;
            return bullet;
        }
        return null;
    }

    public void Reload() {
        currentAmmo += reloadAmount;
        if (currentAmmo > maxAmmo) {
            currentAmmo = maxAmmo;
        }
    }
}
