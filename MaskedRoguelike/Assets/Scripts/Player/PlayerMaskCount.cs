using UnityEngine;

public class PlayerMaskCount : MonoBehaviour
{
    [SerializeField]
    private int maskCount;

    public void AddMasks(int amount)
    {
        maskCount += amount;
    }

    public void RemoveMasks(int amount)
    {
        if (maskCount < amount) { return; }

        maskCount -= amount;
    }
}
