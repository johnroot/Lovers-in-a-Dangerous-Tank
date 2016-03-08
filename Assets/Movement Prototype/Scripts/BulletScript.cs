using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float speed;
    public float damage;
    public GameObject owner;
	public GameObject explosionAnimation;

	// Use this for initialization
	void Start () {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(-speed, 0));
		transform.Rotate (0, 0, 90);
	}

    void OnCollisionEnter2D(Collision2D coll) {
        HealthScript healthScript = coll.gameObject.GetComponent<HealthScript>();
        if (healthScript != null) {
            healthScript.DecreaseHealth(damage);
        }
		Instantiate (explosionAnimation, transform.position, transform.rotation);
        Destroy (gameObject);
    }

	// Update is called once per frame
	void Update () {
	}
}
