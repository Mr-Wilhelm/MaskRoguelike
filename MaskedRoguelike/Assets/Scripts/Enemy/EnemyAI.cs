using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

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
        if (enemyLevel == 2)
        {
            // GetComponentInChildren<SpriteRenderer>().sprite = SET TO RED SLIME SPRITE
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
        
    }
}
