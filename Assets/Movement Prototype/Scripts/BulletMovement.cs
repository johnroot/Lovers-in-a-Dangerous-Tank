using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    int life;

    // Use this for initialization
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(0, speed));
        life = 0;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == gameObject.tag)
        {
            Destroy(coll.gameObject);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        life++;
        if (life > 200)
        {
            Destroy(gameObject);
        }
    }
}
