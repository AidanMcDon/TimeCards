using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameObject, cardDescriptionObject, cardCostObject;
    [SerializeField] private Image cardImageObject;
    public Card card;

    public void Build(Card card){
        this.card = card;
        cardNameObject.text = card.cardName;
        cardDescriptionObject.text = card.description;
        cardCostObject.text = card.cost.ToString();
        if (card is UnitCard unitCard){
            if(unitCard.visuals.cardImage != null){
                cardImageObject.sprite = unitCard.visuals.cardImage;
            }
            if(unitCard.visuals.unitSprite != null){
                cardImageObject.sprite = unitCard.visuals.unitSprite;
            }
            if(unitCard.visuals.unitColor != null){
                cardImageObject.color = unitCard.visuals.unitColor;
            }
        }
        else{
            Debug.LogError("Card is not a UnitCard");
        }
    }
    
}
