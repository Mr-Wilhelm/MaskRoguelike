using System.Linq;
using UnityEngine;

public class RoomManagerScript : MonoBehaviour
{

    public GameObject [] playerSpawnPoints;
    public GameObject [] exitDoors;
    public GameObject [] roomEnemySpawnManagers;
    private GameObject player;
    public bool justLeftVendor = true;
    public bool inVendorRoom = false;
    public int currentRoom = -1;
    public float roomsCleared = 0;
    public float enemiesLeft = 0;
    public GameObject cameraFadeObject;

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
            return;
        }

        if (enemiesLeft <= 0)
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
            Debug.Log("Leaving Vendor Room");
            Debug.Log("Before: " + currentRoom);
            currentRoom = UnityEngine.Random.Range(1, 4);
            Debug.Log("After : " + currentRoom);
            inVendorRoom = false;
            justLeftVendor = false;
            roomsCleared += 1;
            cameraFadeObject.GetComponent<CameraFadeScrip>().teleportPos = playerSpawnPoints[currentRoom].transform.position;
            if (roomsCleared == 1)
            {
                StartCoroutine(cameraFadeObject.GetComponent<CameraFadeScrip>().StartGameFade());
            }
            else
            {
                StartCoroutine(cameraFadeObject.GetComponent<CameraFadeScrip>().FadeOutIn());
            }
            
            //player.transform.position = playerSpawnPoints[currentRoom].transform.position;
            
        }
        else
        {
            exitDoors[3].GetComponent<DoorLogic>().UnlockDoor();
            Debug.Log("Leaving Enemy Room");
            currentRoom = -1;
            inVendorRoom = true;
            justLeftVendor = true;
            cameraFadeObject.GetComponent<CameraFadeScrip>().teleportPos = playerSpawnPoints[0].transform.position;
            StartCoroutine(cameraFadeObject.GetComponent<CameraFadeScrip>().FadeOutIn());
            //player.transform.position = playerSpawnPoints[0].transform.position;
            exitDoors[0].GetComponent<DoorLogic>().LockDoor();
            exitDoors[1].GetComponent<DoorLogic>().LockDoor();
            exitDoors[2].GetComponent<DoorLogic>().LockDoor();
            
        }
        
        int numberOfEnemies;
        if (currentRoom == 1)
        {
            Debug.Log("Populating Room 1");
            // Change magic number to be the same as the number of spawnpoints in the room
            numberOfEnemies = 5 * (Mathf.FloorToInt(roomsCleared * 0.5f)+1);
            enemiesLeft = numberOfEnemies;
            roomEnemySpawnManagers[0].GetComponent<EnemySpawnManager>().spawnEnemyWave(numberOfEnemies);
        }
        else if (currentRoom == 2)
        {
            Debug.Log("Populating Room 2");
            // Change magic number to be the same as the number of spawnpoints in the room
            numberOfEnemies = 5 * (Mathf.FloorToInt(roomsCleared * 0.5f)+1);
            enemiesLeft = numberOfEnemies;
            roomEnemySpawnManagers[1].GetComponent<EnemySpawnManager>().spawnEnemyWave(numberOfEnemies);
        }
        else if (currentRoom == 3)
        {
            Debug.Log("Populating Room 3");
            // Change magic number to be the same as the number of spawnpoints in the room
            numberOfEnemies = 7 * (Mathf.FloorToInt(roomsCleared * 0.3f)+1);
            enemiesLeft = numberOfEnemies;
            roomEnemySpawnManagers[2].GetComponent<EnemySpawnManager>().spawnEnemyWave(numberOfEnemies);
        }
    }
}
