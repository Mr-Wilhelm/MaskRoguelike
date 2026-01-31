using UnityEngine;

public class PlayerMaskCount : MonoBehaviour
{
    [SerializeField]
    private int maskCount;

    public void AddMasks(int amount)
    {
        maskCount += amount;
    }

    public bool RemoveMasks(int amount)
    {
        if (maskCount < amount) { return false; }

        maskCount -= amount;
        return true;
    }
}
