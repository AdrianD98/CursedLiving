using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wizardAI : MonoBehaviour
{
    //hp bar
    public Slider health;

    //death fx
    public GameObject explosion;

    //face target
    bool facingRight = false;
    Vector2 playerPosition;
    GameObject player;

    //move location
    Vector2 targetPosition;
    Rigidbody2D myRB;
    public float speed = 3f;

    //hp
    public float maxHealth = 40f;
    public float currentHealth;
    
    //attack
    public float attackSpeed = 5f;
    float timer = 5f;
    public Transform attackPoint;
    public GameObject fireball;
    bool decreseForm = true;

    Animator myAnim;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        health.value = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();

        myAnim = GetComponent<Animator>();
        FindPLayer();
        currentHealth = maxHealth;
        myRB = GetComponent<Rigidbody2D>();

        InvokeRepeating("Wander", 5f, 4f);
    }

    public void damage(float damage)
    {
        currentHealth -= damage;
        health.value = currentHealth;
        StartCoroutine(changeColor());
    }

    //flash red when damaged
    IEnumerator changeColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.color = Color.white;
    }

    private void FindPLayer()
    {
        player = FindObjectOfType<playerController>().gameObject;
    }

    void Wander()
    {
        StartCoroutine(teleportAnimation());
    }

    IEnumerator teleportAnimation()
    {
        float scale = transform.localScale.x;
        while (scale > 0 && decreseForm)
        {
            scale -= 0.5f;
            transform.localScale = new Vector2(scale, scale);
            yield return new WaitForSeconds(0.01f);
        }
        decreseForm = false;

        targetPosition = new Vector2(Mathf.Clamp((transform.position.x + Random.Range(-42, 42)), -28, 14), Mathf.Clamp((transform.position.y + Random.Range(-4, 4)), 0, 10));

        while (scale < 9 && !decreseForm)
        {
            scale += 0.5f;
            transform.localScale = new Vector2(scale, scale);
            yield return new WaitForSeconds(0.01f);
        }
        decreseForm = true;

    }

    void Dead()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void checkFlip()
    {
        checkAlive();
        playerPosition = player.transform.position;

        if (playerPosition.x > transform.position.x && !facingRight) flip();
        else if (playerPosition.x < transform.position.x && facingRight) flip();
    }

    void Attack()
    {
        if (timer < Time.time)
        {
            myAnim.SetTrigger("Attack");
            timer = Time.time + attackSpeed;
        }
    }

    void Fireball()
    {
        Instantiate(fireball, attackPoint.position, attackPoint.rotation);
    }

    void checkAlive()
    {
        if (player == null)
        {
            FindPLayer();
        }
    }

    private void FixedUpdate()
    {
        checkFlip();
        Attack();
        myRB.MovePosition(Vector2.MoveTowards(myRB.position, targetPosition, speed * Time.deltaTime));

        if (currentHealth <= 0) Dead();
    }

}
