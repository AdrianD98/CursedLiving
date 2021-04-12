using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    //target (either player or ground position)
    Vector2 targetPosition;

    public LayerMask enemyLayer;

    //movement
    Rigidbody2D myRB;
    public float speed = 1f;
    public Rigidbody2D enemyRB = null;

    //how close before attack
    float distance;
    public bool inRangeOfAttack = false;

    //animation
    Animator myAnim;
    bool facingRight = true;

    //attack
    float attackTime;
    public float attackSpeed = 1f;
    public int myDMG = 1;
    public int myKnockBack = 100;
    public Transform attackPoint;

    //health
    public float maxHealth = 10f;
    public float currentHealth = 10f;

    //health checkers
    bool alive = true;
    bool hit = false;

    //sound
    AudioSource audioSource;
    public AudioClip swordClip;

    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();

        myRB = GetComponent<Rigidbody2D>();
        Wander();

        attackTime = Time.time;
    }

    public void damage(float dmg)
    {
        if (!alive) return;

        hit = true;
        Invoke("recoverHit", 0.6f);
        currentHealth -= dmg;
        myAnim.SetTrigger("Hit");
        if (currentHealth <= 0)
        {
            myAnim.SetBool("Dead", true);
            alive = false;
        }
    }

    void recoverHit()
    {
        hit = false;
        CancelInvoke();
    }

    public void newPosition()
    {
        if (enemyRB == null)
        {
            speed = 1f;
            targetPosition = new Vector2((transform.position.x + Random.Range(-2, 2)), transform.position.y);
        }
        else CancelInvoke();
    }

    private void TargetPlayer()
    {
        speed = 2f;
        if (myRB.transform.localScale.x > 0) distance = 2.06f;
        else distance = -2.06f;
        targetPosition = new Vector2((enemyRB.position.x - distance), transform.position.y);
    }

    public void Wander()
    {
        InvokeRepeating("newPosition", 0f, 2f);
    }

    void flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void checkTargetFlip()
    {
        if (enemyRB != null)
        {
            if (targetPosition.x + distance > transform.position.x && !facingRight) flip();
            else if (targetPosition.x + distance < transform.position.x && facingRight) flip();
        }

        else
        {
            if (targetPosition.x > transform.position.x && !facingRight) flip();
            else if (targetPosition.x < transform.position.x && facingRight) flip();
        }
    }

    // Update is called once per frame


    private void Attack()
    {
        attackTime = Time.time + attackSpeed;
        myAnim.SetTrigger("Attack");
        Invoke("CastAttack", 0.5f);
    }

    //delay between the attack so that the player has time to respond
    //does not interfeer with other Invokes/CancelInvokes
    private void CastAttack()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, 1.6f, enemyLayer);

        foreach (Collider2D enemy in hitObjects)
        {
            if (!alive || hit) return;
            enemy.GetComponent<playerController>().damaged(myDMG,myKnockBack, transform.position.x);
        }
        CancelInvoke();
    }

    void AttackSound()
    {
        audioSource.PlayOneShot(swordClip, 2f);
    }

    //for editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, 1.6f);
    }

    void FootStep()
    {
        audioSource.Play();
    }

    void Update()
    {
        if (currentHealth <= 0) Destroy(gameObject, 1.2f);
        if (!alive || hit) return;


        if (transform.position.x != targetPosition.x && !inRangeOfAttack)
        {
            myAnim.SetFloat("Speed", 1);
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else 
        { 
            myAnim.SetFloat("Speed", 0);
            if (!audioSource.isPlaying) audioSource.Pause();
        }
    }

    private void FixedUpdate()
    {
        if (!alive || hit) return;
        checkTargetFlip();

        if (enemyRB != null) TargetPlayer();

        if (inRangeOfAttack == true && enemyRB != null && !hit)
        {
            if (attackTime < Time.time)
            {
                Attack();
            }
        }
        else myRB.MovePosition(Vector2.MoveTowards(myRB.position, targetPosition, speed * Time.deltaTime));

    }

    
}
