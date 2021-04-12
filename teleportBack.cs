using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportBack : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "FlyingEye" || collision.tag == "Goblin")
        {
            var player = collision.gameObject.GetComponent<playerController>();
            player.TeleportBack();
        }
    }
}
