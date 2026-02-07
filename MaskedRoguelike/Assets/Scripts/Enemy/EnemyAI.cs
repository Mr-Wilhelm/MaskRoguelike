using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private GameObject player;
    private NavMeshAgent navMeshAgent;
    private bool canAttack = true;
    private float timeStamp;
    private bool stunned = false;
    private Rigidbody2D rb2d;
    private bool dead = false;


    [Header("Enemy Stats")]
    public float health = 5;
    public float damage = 1;
    public float moveSpeed;
    public int enemyLevel = 1; // Level 1 = Blue, Level 2 = Red
    public float hitStunTime = 0.8f;
    public int lowerBoundMaskDrop = 1;
    public int upperBoundMaskDrop = 3;

    [Header("Enemy Status Conditions")]
    [SerializeField] private bool isBurning = false;
    [SerializeField] private bool isFrosty = false;

    [Header("NavMesh Variables")]
    private NavMeshAgent agent;
    public Vector3 navMeshTarget = new Vector3(10, 10, 0);
    private NavMeshPath path;

    private PlayerAttack playerAttackScript;
    public enum DamageType
    {
        Default,
        Fire,
        Ice
    }

    private float burnEffectTime, frostEffectTime = 0.0f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        rb2d = GetComponent<Rigidbody2D>();

        // Targets the player
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackScript = player.GetComponent<PlayerAttack>();
        navMeshTarget = player.transform.position;

        // Sets the spritesheet to be the correct one according to the level of the enemy
        if (enemyLevel == 2)
        {
            // spriteRenderer.sprite = SET TO RED SLIME SPRITE
            // PROBABLY ALSO NEED TO SWAP OUT THE ANIMATION FOR THE RED ANIMATION SHEET
        }

        // Increases stats if the enemy is a higher level
        health = health * enemyLevel;
        damage = damage * enemyLevel;
        lowerBoundMaskDrop = lowerBoundMaskDrop * enemyLevel;
        upperBoundMaskDrop = upperBoundMaskDrop * enemyLevel;

        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        agent.CalculatePath(navMeshTarget, path);
        agent.SetPath(path);
        agent.isStopped = true;

        moveSpeed = navMeshAgent.speed;

    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.transform.eulerAngles = Vector3.zero;
        
        AnimatorClipInfo[ ] animationClip = animator.GetCurrentAnimatorClipInfo(0);

        if(animationClip.Count() == 0) { return; }

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
            rb2d.linearDamping = 3f;
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
                    int MASK_DROP_VAL = UnityEngine.Random.Range(lowerBoundMaskDrop, upperBoundMaskDrop);
                    player.GetComponent<PlayerMaskCount>().AddMasks(MASK_DROP_VAL);
                    GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManagerScript>().enemiesLeft -= 1;

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

        //status condition checks for when the player attacks
        if (isBurning)
        {
            if(burnEffectTime > playerAttackScript.GetBurnDuration())
            {
                animator.SetBool("isBurning", false);
                isBurning = false;
                burnEffectTime = 0.0f;
                return;
            }
            
            burnEffectTime += Time.fixedDeltaTime;
            health -= playerAttackScript.GetBurnDamageOverTime() * Time.fixedDeltaTime;
            animator.SetBool("isBurning", true);
        }

        if (isFrosty)
        {
            if (frostEffectTime > playerAttackScript.GetFrostSlowDuration())
            {
                navMeshAgent.speed = moveSpeed;
                animator.SetBool("isFrozen", false);
                isFrosty = false;
                frostEffectTime = 0.0f;
                return;
            }

            frostEffectTime += Time.fixedDeltaTime;
            navMeshAgent.speed = playerAttackScript.GetFrostReductionAmount();
            animator.SetBool("isFrozen", true);

        }

        else if (!isFrosty) 
        { 
            animator.SetBool("isFrozen", false); 
        }
    }

    // Example Usage: TakeDamage(1, player.transform.position);
    public void TakeDamage(float amount, Vector3 knockbackSource, DamageType damageType, float knockbackModifier = 1f)
    {
        if (dead){return;}

        // Turning off the NavMeshAgent so that it doesnt overwrite the position or teleport the enemy immediately back to the player when the stun is over
        agent.isStopped = true;
        agent.ResetPath();
        agent.updatePosition = false;
        agent.updateRotation = false;
        
        // Sets the stun cooldown
        timeStamp = Time.time + hitStunTime;

        //status condition check
        if (damageType == DamageType.Fire)
        {
            isBurning = true; 
            animator.SetBool("isBurning", true);
            animator.Play("SlimeBurn", 0, 0f);
        }

         
        else if (damageType == DamageType.Ice)
        {
            isFrosty = true; 
            animator.SetBool("isFrozen", true);
            animator.Play("SlimeFreeze", 0, 0f);
        }


        // Knockback calculation and implementation
        Vector3 force = (transform.position - knockbackSource).normalized * (knockbackModifier * 1000);
        if (!stunned){GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x, force.y));}
        
        // I wonder what this does
        stunned = true;



        // Deals with damage and death checks
        health -= amount;
        if (health <= 0)
        {
            if (!dead)
            {
                dead = true;
                animator.SetBool("isDead", true);
            }
            
        }
        
    }

    public void AttackPlayer(GameObject player)
    {  
        if (canAttack && !stunned && health > 0)
        {
            
            // REPLACE WITH SOME FUNCTION CALL TO THE PLAYER THAT HANDLES DAMAGE, SHOULD INCLUDE DAMAGE AMOUNT AND POSITION OF DAMAGE SOURCE FOR KNOCKBACK CALCULATIONS
            
            player.GetComponent<PlayerHealth>().TakeDamage(damage, transform.position, 1);


            canAttack = false;
        }

    }
}
