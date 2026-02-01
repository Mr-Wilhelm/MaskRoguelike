using UnityEngine;

public class SpawnpointLogic : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int enemyAmount = 0;
    private float timeStamp;
    private float lowerSpawnTimer = 4f;
    private float upperSpawnTimer = 12f;
    public void spawnTimer(int amount)
    {
        enemyAmount = amount;
        timeStamp = Time.time + UnityEngine.Random.Range(lowerSpawnTimer, upperSpawnTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAmount > 0)
        {
            if (timeStamp <= Time.time)
            {
                timeStamp = Time.time + UnityEngine.Random.Range(lowerSpawnTimer, upperSpawnTimer);
                enemyAmount -= 1;
                Instantiate(enemyPrefab, transform);
            }
            
        }
    }
}
