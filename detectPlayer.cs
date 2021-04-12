using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectPlayer : MonoBehaviour
{
    //check if player in range (not for flyingeye as it is considered a harmless monster

    public enemyAI enemyAI;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Goblin")
        {
            enemyAI.enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();

            enemyAI.newPosition();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Goblin")
        {
            enemyAI.enemyRB = null;

            enemyAI.newPosition();
            enemyAI.Wander();
        }
    }
}
