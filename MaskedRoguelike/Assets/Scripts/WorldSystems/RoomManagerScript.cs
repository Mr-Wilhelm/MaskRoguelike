using System.Linq;
using UnityEngine;

public class RoomManagerScript : MonoBehaviour
{

    public GameObject [] playerSpawnPoints;
    public GameObject [] exitDoors;
    public GameObject [] roomEnemySpawnManagers;
    private GameObject player;
    private bool justLeftVendor = false;
    private bool inVendorRoom = false;
    private int currentRoom = -1;
    private float roomsCleared = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        NextRoom();
    }

    void Update()
    {
        if (inVendorRoom)
        {
            exitDoors[0].GetComponent<DoorLogic>().LockDoor();
            exitDoors[1].GetComponent<DoorLogic>().LockDoor();
            exitDoors[2].GetComponent<DoorLogic>().LockDoor();
            return;
        }

        if (GameObject.FindGameObjectsWithTag("Enemy").Count() <= 0)
        {
            if (currentRoom == 1)
            {
                exitDoors[0].GetComponent<DoorLogic>().UnlockDoor();
            }
            else if (currentRoom == 2)
            {
                exitDoors[1].GetComponent<DoorLogic>().UnlockDoor();
            }
            else if (currentRoom == 3)
            {
                exitDoors[2].GetComponent<DoorLogic>().UnlockDoor();
            }
        }
    }

    public void NextRoom()
    {
        if (justLeftVendor)
        {
            currentRoom = UnityEngine.Random.Range(1, 3);
            player.transform.position = playerSpawnPoints[currentRoom].transform.position;
            justLeftVendor = false;
            roomsCleared += 1;
        }
        else
        {
            player.transform.position = playerSpawnPoints[0].transform.position;
            justLeftVendor = true;
        }
        
        int numberOfEnemies;
        if (currentRoom == 1)
        {
            // Change magic number to be the same as the number of spawnpoints in the room
            numberOfEnemies = 5 * (Mathf.FloorToInt(roomsCleared * 0.5f)+1);
            roomEnemySpawnManagers[0].GetComponent<EnemySpawnManager>().spawnEnemyWave(numberOfEnemies);
        }
        else if (currentRoom == 2)
        {
            // Change magic number to be the same as the number of spawnpoints in the room
            numberOfEnemies = 5 * (Mathf.FloorToInt(roomsCleared * 0.5f)+1);
            roomEnemySpawnManagers[1].GetComponent<EnemySpawnManager>().spawnEnemyWave(numberOfEnemies);
        }
        else if (currentRoom == 3)
        {
            // Change magic number to be the same as the number of spawnpoints in the room
            numberOfEnemies = 7 * (Mathf.FloorToInt(roomsCleared * 0.3f)+1);
            roomEnemySpawnManagers[2].GetComponent<EnemySpawnManager>().spawnEnemyWave(numberOfEnemies);
        }
    }
}
