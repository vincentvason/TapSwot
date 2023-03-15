using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public AllCards cardDatabase;
    public static CardManager instance;

    [SerializeField]private List<CardSO> mainDeck = new List<CardSO>();
    [SerializeField]private List<CardSO> discardedDeck = new List<CardSO>();

    //Bool to Initialize the Main card deck copy before allocating the individual Player decks  
    public Action<bool> cardInitilaized;

    //To check if the cards have been shuffled
    private bool shuffleCheck = false;

    private void Start()
    {
        mainDeck = cardDatabase.cards.Select(c => new CardSO(c)).ToList();
        cardInitilaized?.Invoke(true);
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
        // Shuffle cards pending

        shuffleCheck = true;

    }
}
