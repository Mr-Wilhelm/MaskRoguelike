using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Enemy Stats")]
    public float health = 5;
    public float damage = 1;
    public float moveSpeed = 10;
    public int enemyLevel = 1; // Level 1 = Blue, Level 2 = Red

    [Header("NavMesh Variables")]
    private NavMeshAgent agent;
    public Vector3 navMeshTarget = new Vector3(10, 10, 0);
    private NavMeshPath path;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();


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
        Debug.Log(currentFrame%10);
        if (currentFrame%10 > 4)
        {
            if (agent.isStopped == true)
            {
                agent.isStopped = false;
            }
        }
        else
        {
            agent.CalculatePath(navMeshTarget, path);
            agent.SetPath(path);
            agent.isStopped = true;
        }
    }
}
