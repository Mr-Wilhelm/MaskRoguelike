using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    public bool locked = true;
    public bool vendorDoor = false;

    public void UnlockDoor()
    {
        if (locked)
        {
            locked = false;
            // TURN THE LIGHT ON
        }
    }

    public void LockDoor()
    {
        locked = true;
        // TURN THE LIGHT OFF
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!locked)
        {
            if (other.gameObject.tag == "Player")
            {
                GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManagerScript>().NextRoom();
                locked = true;
                
            }
        }
        
        
    }

}
