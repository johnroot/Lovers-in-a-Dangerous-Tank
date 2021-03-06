using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {

    public float rotationSpeed;
    public float fireRate;
    public GameObject bullet;
    public int maxAmmo;
    public int reloadAmount;
    public float barrelLength;

    int currentAmmo;
    int timeElapsedSinceFire = 0;

    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
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
			Vector3 barrelEnd = transform.position +
			                    (Quaternion.Euler (transform.eulerAngles) * new Vector3 (-barrelLength, 0, 0));
			GameObject bulletInstance = (GameObject) Instantiate(bullet, barrelEnd, transform.rotation);
			timeElapsedSinceFire = 0;
            audioSource.Play();
			return bulletInstance;
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
