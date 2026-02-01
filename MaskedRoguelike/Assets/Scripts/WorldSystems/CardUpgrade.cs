using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardUpgrade : MonoBehaviour
{
    [Header("Card Attributes")]
    [SerializeField]
    private Sprite[] cardSprites;

    [SerializeField]
    private string[] cardTexts;

    public enum CardTypes
    {
        None,
        FrostStrength,
        FrostDuration,
        Heal,
        MaxHealth,
        BurnDamage,
        BurnDuration
    }

    [Header("Card Components")]
    [SerializeField]
    private TextMeshProUGUI cardText;

    public void RandomiseUpgrade()
    {
        Debug.Log(gameObject.name + "Randomising Upgrades");
    }
}
