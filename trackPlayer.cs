using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackPlayer : MonoBehaviour
{
    //fireball homing

    public GameObject explosion;

    public float speed = 6f;
    Rigidbody2D myRB;
    Vector3 target;

    playerController playerController;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        findPlayer();

        InvokeRepeating("Track", 0f, 0.1f);
    }

    private void findPlayer()
    {
        playerController = FindObjectOfType<playerController>();
    }

    void Track()
    {
        if (playerController == null || !playerController.alive) { findPlayer(); }
        target = playerController.transform.position;

        if (Mathf.Abs(transform.position.x - target.x) < 4 && Mathf.Abs(transform.position.y - target.y) < 4)
        {
            
            CancelInvoke();
        }
    }

    public void Detonate()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position,transform.rotation);
    }

    private void FixedUpdate()
    {
        myRB.MovePosition(Vector2.MoveTowards(myRB.position, target, speed * Time.deltaTime));

        if (transform.position.x == target.x && Mathf.Abs(transform.position.y - target.y) < 0.1) Detonate();
    }
}
