using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //cameraUpdate
    public CameraFollow cameraFollow;

    //movement
    public float maxSpeed;
    Vector2 lastPosition;

    //characters
    public playersList PlayersList;

    //jump
    bool grounded = false;
    float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;

    Rigidbody2D myRB;
    Animator myAnim;
    bool facingRight;

    //attacking
    public float fireSpeed = 0.5f;
    float nextFire = 0f;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public float myDMG = 10f;
    float attackRange = 1.5f;

    //hit
    int enemyKnockback;
    float enemyPositionX;
    float timer;
    bool knockedAway = false;
    float timeSinceLastKnock;

    //hp
    public int maxHp = 10;
    int currentHp;
    public bool alive = true;

    //effects
    public GameObject blood;

    //sound
    AudioSource audioSource;
    public AudioClip swordClip;
    public AudioClip diamondClip;
    public AudioClip deathClip;

    private void Awake()
    {
        cameraFollow = Object.FindObjectOfType<CameraFollow>();
        cameraFollow.setNewTarget(transform);
        PlayersList = Object.FindObjectOfType<playersList>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckLastPosition());

        audioSource = GetComponent<AudioSource>();

        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        facingRight = true;

        currentHp = maxHp;
    }

    IEnumerator CheckLastPosition()
    {
        while (true)
        {
            if (grounded)
            {
                lastPosition = new Vector2(transform.position.x, transform.position.y);
                
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void TeleportBack()
    {
        transform.position = lastPosition;
    }

    public void damaged(int damage, int knockback, float enemyPosition)
    {
        enemyPositionX = enemyPosition;
        enemyKnockback = knockback;
        timer = Time.time + 0.2f;
        knockedAway = true;
        timeSinceLastKnock = Time.time;
        InvokeRepeating("KnockBack", 0, 0.01f);
        
        myAnim.SetTrigger("Hit");
        Instantiate(blood, transform, false);
        currentHp -= damage;
    }

    void KnockBack()
    {
        if (Time.time <= timer)
            myRB.AddForce(new Vector2(transform.position.x - enemyPositionX, 0.2f) * enemyKnockback);
        else
        {
            knockedAway = false;
            CancelInvoke();
        }
    }

    public void damaged(int damage)
    {
        myAnim.SetTrigger("Hit");
        Instantiate(blood, transform, false);
        currentHp -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Evil_object" && gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(diamondClip,2f);
            PlayersList.increaseCounter();
            Destroy(collision.gameObject);
            Death();
            return;
        }

        if(collision.tag == "Good_object")
        {
            Destroy(collision.gameObject);
            return;
        }

        if(collision.tag == "mana_crystal" && gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(diamondClip, 2f);
            FindObjectOfType<Morph>().changeMana(1) ;
            Destroy(collision.gameObject);
            return;
        }

        if (collision.tag == "fireball")
        {
            FindObjectOfType<Morph>().Spelllock();
            nextFire = Time.time + 2f;

            collision.GetComponent<trackPlayer>().Detonate();
            return;
        }
    }


    private void Attack()
    {
        nextFire = Time.time + fireSpeed;
        myAnim.SetTrigger("Attack1");
        StartCoroutine("castAttack");
    }

    IEnumerator castAttack()
    {
        yield return new WaitForSeconds(0.2f);

        audioSource.PlayOneShot(swordClip,3f);


        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitObjects)
        {
            if (enemy.GetComponent<Destructables>())
                enemy.GetComponent<Destructables>().damage(myDMG);

            else if (enemy.GetComponent<enemyAI>())
            {
                enemy.GetComponent<enemyAI>().damage(myDMG);
            }

            else if (enemy.GetComponent<wizardAI>())
            {
                enemy.GetComponent<wizardAI>().damage(myDMG);
            }
        }

        yield return null;
    }

    // Update is called once per frame
    private void CheckGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Death()
    {   
        var clipVolume = 2f;
        if (gameObject.tag == "Goblin") clipVolume = 4f; 
        audioSource.PlayOneShot(deathClip, clipVolume);

        alive = false;
        myAnim.SetTrigger("Dead");
        if(gameObject.tag == "FlyingEye") { var rb = GetComponent<Rigidbody2D>(); rb.gravityScale = 5; }

        PlayersList.setCounter();

        Instantiate(PlayersList.players[PlayersList.counter], transform.position, transform.rotation);

        
        this.enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        Destroy(gameObject, 2f);
    }
    void Update()
    {
       if((Input.GetAxis("Jump") > 0 || Input.GetKey(KeyCode.W)) && (grounded  || gameObject.tag == "FlyingEye"))
        {
            grounded = false;
            myRB.velocity = new Vector2(0, jumpHeight);

        }

       if (Input.GetKey(KeyCode.S) == true && gameObject.tag == "FlyingEye")
            myRB.velocity = new Vector2(0, -jumpHeight);

        //attacker
        if (Input.GetAxisRaw("Fire1") > 0 && Time.time > nextFire && gameObject.tag == "Goblin")
        {
            Attack();

        }

        if (currentHp <= 0) Death();
    }

    void Footstep()
    {
        if(grounded || gameObject.tag =="FlyingEye")
        audioSource.Play();
    }


    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");

        //check grounded
        if (gameObject.tag != "FlyingEye")
        {
            CheckGrounded();
            myAnim.SetFloat("Speed", Mathf.Abs(move));
            myAnim.SetFloat("vSpeed", Mathf.Abs(myRB.velocity.y));
        }


        if (!knockedAway)
        { 

            myRB.velocity = new Vector2(move * maxSpeed, myRB.velocity.y);
        }

        if (move > 0 && !facingRight) flip();
        else if (move < 0 && facingRight) flip();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
