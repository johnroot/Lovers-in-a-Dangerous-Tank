using UnityEngine;
using System.Collections;

public class DroneScript : MonoBehaviour {

    public float damage;
    public float speed;
    public GameObject target;

    Vector2 dronePosition;
    Vector2 targetPosition;
    Rigidbody2D rb;
	
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update () {
        dronePosition = rb.position;
        targetPosition = target.transform.gameObject.GetComponent<Rigidbody2D>().position;
        rb.velocity = speed * (targetPosition - dronePosition).normalized;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Collided with an object!");
        HealthScript healthScript = coll.gameObject.GetComponent<HealthScript>();
        if (healthScript != null)
        {
            healthScript.DecreaseHealth(damage);
        }
        Destroy(gameObject);
    }

}
