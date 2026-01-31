using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Enemy Stats")]
    public float health = 5;
    public float damage = 1;
    public float moveSpeed = 10;
    public int enemyLevel = 1; // Level 1 = Blue, Level 2 = Red

    [Header("NavMesh Variables")]
    private NavMeshAgent agent;
    public Vector3 navMeshTarget = new Vector3(10, 10, 0);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (enemyLevel == 2)
        {
            // spriteRenderer.sprite = SET TO RED SLIME SPRITE
        }
        health = health * enemyLevel;
        damage = damage * enemyLevel;
        agent = GetComponent<NavMeshAgent>();
        var path = new NavMeshPath();
        agent.CalculatePath(navMeshTarget, path);
        agent.SetPath(path);

    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.transform.eulerAngles = Vector3.zero;
    }
}
