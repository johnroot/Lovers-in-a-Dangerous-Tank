using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float speed;
    public float damage;
    public GameObject owner;

	// Use this for initialization
	void Start () {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(0, speed));
        // Parent of transform gives the weapon. The parent of that is the owner of the bullet.
        owner = gameObject.transform.parent.transform.parent.gameObject;
	}

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject != owner) {
            HealthScript healthScript = coll.gameObject.GetComponent<HealthScript>();
            if (healthScript != null) {
                healthScript.DecreaseHealth(damage);
            }
        }
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
	}
}
