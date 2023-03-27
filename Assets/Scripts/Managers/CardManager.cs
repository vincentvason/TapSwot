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

    //Temporary container deck to shuffle but can use discardedDeck instead to save memory unless client requirements for it change
    // [SerializeField]private List<CardSO> tempDeck= new List<CardSO>(); 

    //To check if the cards have been shuffled
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
        //Initialise here in discarded deck content transform
    }

    public void CleanDiscarded()
    {
        //delete all child in discarded deck content transform
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
