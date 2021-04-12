using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour
{
    //tiles that can be destoyed in game

    public float maxHealth = 10f;
    public float currentHealth = 10f;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void damage(float dmg)
    {
        currentHealth -= dmg;
    }

    public void Update()
    {
        if (currentHealth <= 0) Destroy(gameObject);
    }
}
