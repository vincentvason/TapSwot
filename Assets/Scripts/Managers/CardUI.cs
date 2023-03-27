using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
{
    public CardSO card;
    public TextMeshProUGUI cardTitle;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardRank;
    public TextMeshProUGUI cardCategory;

    public void Initialize(CardSO card)
    {
        cardTitle.text = card.cardTitle;
        cardDescription.text = card.cardDescription;
        cardRank.text = card.cardRank.ToString();
        cardCategory.text = card.cardCategory;        
    }
}
