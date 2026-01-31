using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [Header("ExternalScripts")]
    [SerializeField]
    private PlayerAttack playerAttackScript;

    [SerializeField]
    private PlayerMaskCount playerMaskCount;

    [SerializeField]
    private PlayerHealth playerHealth;

    public void BuyIceStrengthUpgrade()
    {
        int upgradeCost = 10;
        if (playerMaskCount.RemoveMasks(upgradeCost))
            playerAttackScript.frostSlowUpgrades += 1;
        else
            return;
    }
}
