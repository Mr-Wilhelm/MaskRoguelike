using TMPro;
using UnityEngine;

public class PlayerMaskCount : MonoBehaviour
{
    [SerializeField]
    private int maskCount;
    public TextMeshProUGUI counterText;

    public void AddMasks(int amount)
    {
        maskCount += amount;
        counterText.text = "x " + maskCount;
    }

    public bool RemoveMasks(int amount)
    {
        if(maskCount < amount)
        {
            return false;
        }
        else if(maskCount >= amount)
        {
            maskCount -= amount;
            return true;
        }
        counterText.text = "x " + maskCount;
        return false;
    }
}
