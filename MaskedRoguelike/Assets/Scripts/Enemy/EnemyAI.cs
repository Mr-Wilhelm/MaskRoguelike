using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private GameObject player;
    private bool canAttack = true;
    private float timeStamp;
    private bool stunned = false;
    private Rigidbody2D rb2d;

    [Header("Enemy Stats")]
    public float health = 5;
    public float damage = 1;
    public float moveSpeed = 10;
    public int enemyLevel = 1; // Level 1 = Blue, Level 2 = Red
    public float hitStunTime = 0.8f;

    [Header("NavMesh Variables")]
    private NavMeshAgent agent;
    public Vector3 navMeshTarget = new Vector3(10, 10, 0);
    private NavMeshPath path;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        rb2d = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");
        navMeshTarget = player.transform.position;

        if (enemyLevel == 2)
        {
            // spriteRenderer.sprite = SET TO RED SLIME SPRITE
        }
        health = health * enemyLevel;
        damage = damage * enemyLevel;
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        agent.CalculatePath(navMeshTarget, path);
        agent.SetPath(path);
        agent.isStopped = true;

    }

    // Update is called once per frame
    void Update()
    {

        
        

        spriteRenderer.transform.eulerAngles = Vector3.zero;
        
        AnimatorClipInfo[ ] animationClip = animator.GetCurrentAnimatorClipInfo(0);
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        int currentFrame = (int) (animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (animationClip [0].clip.length * animationClip[0].clip.frameRate));
        if (currentFrame%10 > 4)
        {
            rb2d.linearDamping = 0.2f;
            if (!stunned)
            {
                if (agent.isStopped == true)
            {
                agent.isStopped = false;
                canAttack = true;
            }
            }
            
        }
        else
        {
            rb2d.linearDamping = 5f;
            if (stunned)
            {
                if (timeStamp <= Time.time)
                {
                    agent.ResetPath();
                    agent.nextPosition = transform.position;
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    stunned = false;
                    
                    
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (health <= 0)
                {
                    Destroy(gameObject);
                    return;
                }


                navMeshTarget = player.transform.position;
                if (navMeshTarget.x > transform.position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
                agent.CalculatePath(navMeshTarget, path);
                agent.SetPath(path);
                agent.isStopped = true;
            }
            
        }
    }

    public void TakeDamage(float amount, Vector3 knockbackSource, float knockbackStrength)
    {
        
        agent.isStopped = true;
        agent.ResetPath();
        agent.updatePosition = false;
        agent.updateRotation = false;
        

        timeStamp = Time.time + hitStunTime;

        Vector3 force = (transform.position - knockbackSource).normalized * knockbackStrength;
        if (!stunned){GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x, force.y));}
        
        stunned = true;

        health -= amount;
        if (health <= 0)
        {
            // PLAY DEATH ANIMATION
        }
        
    }

    public void AttackPlayer(GameObject player)
    {  
        if (canAttack)
        {
            TakeDamage(1, player.transform.position, 1000);
            Debug.Log("test");
            // REPLACE WITH SOME FUNCTION CALL TO THE PLAYER THAT HANDLES DAMAGE, SHOULD INCLUDE DAMAGE AMOUNT AND POSITION OF DAMAGE SOURCE FOR KNOCKBACK CALCULATIONS
            // player.GetComponent<DamageHandlerScript>().TakeDamage(damage, transform.position)

            canAttack = false;
        }

    }
}
