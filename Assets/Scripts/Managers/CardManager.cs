using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public CardDatabase cardDatabase;
    public static CardManager instance;

    [SerializeField]private List<CardSO> mainDeck = new List<CardSO>();
    [SerializeField]private List<CardSO> discardedDeck = new List<CardSO>();

    public RectTransform mainDeckRect;
    public GameObject card;

  
    private bool shuffleCheck = false;

    private void Start()
    {
        mainDeck = cardDatabase.cards.Select(c => new CardSO(c)).ToList();

    }

    public void UpdateDiscardedCards(CardSO c)
    {
        discardedDeck.Remove(c);      
    }

    public void UpdateDiscardedDeckUI()
    {
        CleanDiscarded();
        DisplayRemainingCard();
    }

    public void CleanDiscarded()
    {
        //delete all child in discarded deck content transform
    }

    public void DisplayRemainingCard()
    {
        for (int i = 0; i < mainDeck.Count; i++)
        {
            GameObject a = GameObject.Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            a.transform.SetParent(mainDeckRect);
            a.GetComponent<CardUI>().Initialize(mainDeck[i]);

        }

    }

    public List<CardSO> GetCardListBasedOnIds(List<string> ids)
    {
        return System.Linq.Enumerable.ToList(cardDatabase.cards.Where(c => ids.Contains(c.cardId.ToString())));
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public List<CardSO> GetShuffledCards()
    {
        if (!shuffleCheck)
        {
           ShuffleCards();
        }
        return mainDeck;
    }
    //to shuffle the cards before distributing to players
    public void ShuffleCards()
    {
        // Shuffle the cards using the discardedDeck list as a temporary container
        for (int i=0;i <mainDeck.Count;i++)
        {
            discardedDeck[0]=mainDeck[i];
            int randomValue = UnityEngine.Random.Range(i, mainDeck.Count);
            mainDeck[i]=mainDeck[randomValue];
            mainDeck[randomValue]= discardedDeck[0];
            
        }
        discardedDeck.Clear();
        shuffleCheck = true;

    }
}
