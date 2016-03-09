using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public float maxHealth = 100f;
    public float health = 100f;
    public GameObject healthBar;
	public GameObject deathAnimation;

    public void DecreaseHealth(float damage)
    {
        health -= damage;
        if (healthBar != null)
        {
            UpdateHealthBar();
        }
        if (health <= 0)
        {
			Instantiate (deathAnimation, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3(health / maxHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

}
