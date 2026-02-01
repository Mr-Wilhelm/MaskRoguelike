using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CardUpgrade : MonoBehaviour
{
    [Header("Card Attributes")]
    [SerializeField]
    private Sprite[] cardSprites;

    [SerializeField]
    private string[] cardTexts;

    [SerializeField]
    private int cardCost;

    [Header("Card Components")]
    [SerializeField]
    private TextMeshProUGUI cardText;

    [SerializeField]
    private Image cardSprite;

    [SerializeField]
    private CardTypes cardType;

    [Header("Player Script References")]
    [SerializeField]
    private PlayerAttack playerAttackScript;

    [SerializeField]
    private PlayerHealth playerHealthScript;

    [SerializeField]
    private PlayerMovement playerMoveScript;

    [SerializeField]
    private PlayerMaskCount playerMaskCountScript;

    public enum CardTypes
    {
        None,
        FrostStrength,
        FrostDuration,
        Heal,
        MaxHealth,
        BurnDamage,
        BurnDuration,
        MoveSpeed,
        KnockbackStrength
    }

    private void Awake()
    {
        cardSprite = GetComponent<Image>();
        cardText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void RandomiseUpgrade()
    {
        cardType = (CardTypes)Random.Range(1, 8);

        print("card " + gameObject.name + " : " + cardType);

        switch(cardType)
        {
            case CardTypes.FrostStrength:
                cardSprite.sprite = cardSprites[0];
                cardText.text = cardTexts[0];
                break;
            case CardTypes.FrostDuration:
                cardSprite.sprite = cardSprites[0];
                cardText.text = cardTexts[1];
                break;
            case CardTypes.Heal:
                cardSprite.sprite = cardSprites[1];
                cardText.text = cardTexts[2];
                break;
            case CardTypes.MaxHealth:
                cardSprite.sprite = cardSprites[1];
                cardText.text = cardTexts[3];
                break;
            case CardTypes.BurnDamage:
                cardSprite.sprite = cardSprites[2];
                cardText.text = cardTexts[4];
                break;
            case CardTypes.BurnDuration:
                cardSprite.sprite = cardSprites[2];
                cardText.text = cardTexts[5];
                break;
            case CardTypes.MoveSpeed:
                cardSprite.sprite = cardSprites[3];
                cardText.text = cardTexts[6];
                break;
            case CardTypes.KnockbackStrength:
                cardSprite.sprite = cardSprites[4];
                cardText.text = cardTexts[7];
                break;
        }
    }

    public void BuyUpgrade()
    {
        if (playerMaskCountScript.RemoveMasks(cardCost))
        {
            switch (cardType)
            {

                case CardTypes.FrostStrength:
                    playerAttackScript.frostMovementSpeedReduction += 1;
                    break;
                case CardTypes.FrostDuration:
                    playerAttackScript.frostDuration += 1;
                    break;
                case CardTypes.Heal:
                    playerHealthScript.Heal(10.0f);
                    break;
                case CardTypes.MaxHealth:
                    playerHealthScript.AddMaxHealthUpgrade(1);
                break;
                case CardTypes.BurnDamage:
                    playerAttackScript.burnDamageOverTime += 1;
                    break;
                case CardTypes.BurnDuration:
                    playerAttackScript.burnDuration += 1;
                    break;
                case CardTypes.MoveSpeed:
                    playerMoveScript.moveUpgrades += 1;
                    break;
                case CardTypes.KnockbackStrength:
                    playerAttackScript.knockbackUpgrades += 1;
                    break;
            }
        }
        else
        {
            return;
        }
    }
}
