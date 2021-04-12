using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtableDamage : MonoBehaviour
{
    //for tiles that can hurt player on collision (i.e. spikes)

    public int Damage = 5;
    float attackTimer = 0.5f;
    float nextAttack;
    public LayerMask player;

    playerController playerController;

    private void Start()
    {
        nextAttack = 0f;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("playerLayer") && nextAttack < Time.time)
        {
            
            nextAttack = Time.time + attackTimer;
            playerController = collision.GetComponent<playerController>();
            playerController.damaged(Damage); 
        }

    }
}
