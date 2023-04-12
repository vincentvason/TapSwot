using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardSO
{
    public int cardId; // Manually assigned card ID to identify each card in any future use case when list is shuffled
    public string cardTitle; // The main card title to be displayed in bold

    [TextAreaAttribute(3,5)]
    public string cardDescription; // The subtitle card discription

    [TextAreaAttribute(3, 10)]
    public string cardBrief; // The brief content of the card

    public string cardCategory; // The category of the card (Strength - S, Weakness - W, Opportunity - O, Threat - T ) used with first letter in capital
    public int cardRank; // The unqiue rank which the player allocates for each card in the Ranking stage

    public CardSO(CardSO card) // Constructor to create a copy of the original AllcardsSO 
    {
        this.cardId = card.cardId;
        this.cardTitle = card.cardTitle;
        this.cardDescription = card.cardDescription;
        this.cardBrief = card.cardBrief;
        this.cardCategory = card.cardCategory;
        this.cardRank = card.cardRank;
    }
}

