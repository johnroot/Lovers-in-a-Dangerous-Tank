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
	}

    void OnCollisionEnter2D(Collision2D coll) {
        HealthScript healthScript = coll.gameObject.GetComponent<HealthScript>();
        if (healthScript != null) {
            healthScript.DecreaseHealth(damage);
        }
        Destroy (gameObject);
    }

	// Update is called once per frame
	void Update () {
	}
}
