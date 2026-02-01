using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{

    [SerializeField]
    private CardUpgrade[] cardUpgrades;

    public void EnterShop()
    {
        GetComponent<Canvas>().enabled = true;

        foreach (CardUpgrade upgrade in cardUpgrades) { upgrade.RandomiseUpgrade(); }
    }

    public void LeaveShop()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
