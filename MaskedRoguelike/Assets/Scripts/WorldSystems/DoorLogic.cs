using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorLogic : MonoBehaviour
{
    public bool locked = true;
    public bool vendorDoor = false;
    public Light2D exitLight;

    public void UnlockDoor()
    {
        if (locked)
        {
            locked = false;
            exitLight.enabled = true;
        }
    }

    public void LockDoor()
    {
        locked = true;
        exitLight.enabled = false;
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
