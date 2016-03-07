using UnityEngine;
using System.Collections;

public class MissileScript: MonoBehaviour
{
    public float speed;
	int life;
	public GameObject explosionAnimation;


    // Use this for initialization
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.AddRelativeForce(new Vector2(-speed, 0));
        life = 0;
		transform.Rotate (0, 0, 90);
    }

    void OnCollisionEnter2D(Collision2D coll)
	{
        if (coll.gameObject.tag == gameObject.tag)
        {
            Destroy(coll.gameObject);
        }
		SpawnDeathAnimation ();
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

	public void SpawnDeathAnimation()
	{
		Instantiate (explosionAnimation, transform.position, Quaternion.identity);
	}
}
