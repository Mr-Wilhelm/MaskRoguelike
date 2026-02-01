using System.Linq;
using Unity.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    public GameObject [] spawnPoints;

    

    public void spawnEnemyWave(int amount)
    {
        foreach (var x in spawnPoints)
        {
            x.GetComponent<SpawnpointLogic>().spawnTimer(amount/spawnPoints.Count());
        }
    }

}
