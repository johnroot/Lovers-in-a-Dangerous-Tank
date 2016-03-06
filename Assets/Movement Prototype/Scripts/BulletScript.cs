using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(0, speed));
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == gameObject.tag)
        {
            Destroy(coll.gameObject);
        }

        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
	}
}
