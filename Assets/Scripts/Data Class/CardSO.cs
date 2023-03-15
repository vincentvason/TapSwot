using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardSO
{
    public int cardId;
    public string cardTitle;
    public string cardDescription;
    public string cardBrief;
    public string cardCategory;
    public int cardRank;

    public CardSO(CardSO card)
    {
        this.cardId = card.cardId;
        this.cardTitle = card.cardTitle;
        this.cardDescription = card.cardDescription;
        this.cardBrief = card.cardBrief;
        this.cardCategory = card.cardCategory;
        this.cardRank = card.cardRank;
    }
}

