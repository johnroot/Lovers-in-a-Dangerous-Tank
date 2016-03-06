using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public float maxHealth = 100f;
    public float health = 100f;
    public GameObject healthBar;

    public void DecreaseHealth(float damage)
    {
        health = System.Math.Max(0, health - damage);
    }

    public void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3(health / maxHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

}
