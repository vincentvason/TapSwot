using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardController : MonoBehaviour
{
    public Card card;
    public TextMeshProUGUI cardTitle;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardRank;
    public TextMeshProUGUI cardCategory;

    void Start()
    {
        
    }

    public void Initialize(Card card)
    {
        cardTitle.text = card.cardTitle;
        cardDescription.text = card.cardDescription;
        cardRank.text = card.cardRank.ToString();
        cardCategory.text = card.cardCategory;

    }
    private void Awake()
    {
        Initialize(card);  

    }

    void Update()
    {
        
    }
}
