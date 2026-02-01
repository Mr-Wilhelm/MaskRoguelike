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

    [Header("Card Components")]
    [SerializeField]
    private TextMeshProUGUI cardText;

    [SerializeField]
    private Image cardSprite;

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

    private void Awake()
    {
        cardSprite = GetComponent<Image>();
        cardText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void RandomiseUpgrade()
    {
        CardTypes cardType = (CardTypes)Random.Range(1, 6);

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
        }
    }
}
