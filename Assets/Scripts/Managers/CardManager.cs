using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;

public class CardManager : MonoBehaviour
{
    public CardDatabase cardDatabase;
    public static CardManager instance;

    [SerializeField]private List<CardSO> mainDeck = new List<CardSO>();
    [SerializeField]private List<CardSO> remainingCards = new List<CardSO>();

    public RectTransform mainDeckRect,discardedDeckRect;
    public GameObject card;

    private bool shuffleCheck = false;

    [SerializeField] private List<CardSO> discardedCards = new List<CardSO>();

    [SerializeField] private List<CardSO> VotingRoundDiscardedCards = new List<CardSO>();


    private void Start()
    {
        mainDeck = cardDatabase.cards.Select(c => new CardSO(c)).ToList();
        remainingCards = cardDatabase.cards.Select(c => new CardSO(c)).ToList();
    }

    public void AddCardToDiscardedCards(string cardID)
    {
        CardSO co = GetCardBasedOnId(cardID);
        CardSO findCard = cardDatabase.cards.FirstOrDefault(c => c.cardId.ToString().Equals(co.cardId));
        if (findCard == null)
        {
            discardedCards.Add(co);
            Debug.Log("[AddCardToDiscardedCards] remainingCards length" + remainingCards.Count);
        }
    }

    public void RemoveCardFromDiscardedCards(string cardID)
    {
        CardSO c = GetCardBasedOnId(cardID);
        foreach (CardSO so in remainingCards.ToList())
        {
            if (so.cardId == c.cardId)
            {
                discardedCards.Remove(so);
                Debug.Log("[RemoveCardFromDiscardedCards] remainingCards length" + remainingCards.Count);
            }
        }
    }

    public void AddCardToRemainingCards(string cardID)
    {
        CardSO co = GetCardBasedOnId(cardID);
        CardSO findCard = cardDatabase.cards.FirstOrDefault(c => c.cardId.ToString().Equals(co.cardId));
        if (findCard == null)
        {
            remainingCards.Add(co);
            Debug.Log("[AddCardToRemainingCards] remainingCards length" + remainingCards.Count);
        }
    }

    public void RemoveCardFromRemainingDeck(CardSO c)
    {
        foreach(CardSO so in remainingCards.ToList())
        {
            if(so.cardId == c.cardId)
            {
                remainingCards.Remove(so);
                Debug.Log("[UpdateDiscardedCards] remainingCards length" + remainingCards.Count);
            }
        }        
    }

    public void UpdateDeckFromData(List<CardSO> cards)
    {
        remainingCards.Clear();
        remainingCards = cards;
    }

    public void UpdateDiscardedDeckUI()
    {
        CleanDiscarded();
        DisplayDiscardedCard();
        CardGameManagerUI.instance.UpdateDiscardedScrollText(discardedCards.Count.ToString());

        //if remainging cards.count <=0 and all players has taken turn atlest once... change game state to stage 2
        if (remainingCards.Count <= 0)
        {
            CardGameManager.instance.CheckAllPlayersAndUpdateGameStage();
        }
    }

    public void CleanDiscarded()
    {
        //delete all child in discarded deck content transform
        if (discardedDeckRect.transform.childCount > 0)
        {
            for (int i = 0; i < discardedDeckRect.transform.childCount; i++)
            {
                Destroy(discardedDeckRect.transform.GetChild(i).gameObject);
            }
        }
    }

    public void DisplayDiscardedCard()
    {
        Debug.Log("[DisplayRemainingCard] discardedCards length" + discardedCards.Count);

        for (int i = 0; i < discardedCards.Count; i++)
        {
            GameObject a = GameObject.Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            a.transform.SetParent(discardedDeckRect);
            a.GetComponent<CardUI>().Initialize(discardedCards[i]);
        }
    }

    public void CreateCardsForVotingDiscard()
    {
        List<CardSO> RankOne = new List<CardSO>();
        List<CardSO> RankTwo = new List<CardSO>();
        List<CardSO> RankThree = new List<CardSO>();
        List<CardSO> RankFour = new List<CardSO>();
        List<CardSO> RankFive = new List<CardSO>();

        for (int i = 0; i < PlayerManager.instance.ReceivedCardsFromAllPlayersAfterRanking.Count; i++)
        {
            switch (PlayerManager.instance.ReceivedCardsFromAllPlayersAfterRanking[i].cardRank)
            {
                case 1:
                    RankOne.Add(PlayerManager.instance.ReceivedCardsFromAllPlayersAfterRanking[i]);
                    break;
                case 2:
                    RankTwo.Add(PlayerManager.instance.ReceivedCardsFromAllPlayersAfterRanking[i]);
                    break;
                case 3:
                    RankThree.Add(PlayerManager.instance.ReceivedCardsFromAllPlayersAfterRanking[i]);
                    break;
                case 4:
                    RankFour.Add(PlayerManager.instance.ReceivedCardsFromAllPlayersAfterRanking[i]);
                    break;
                case 5:
                    RankFive.Add(PlayerManager.instance.ReceivedCardsFromAllPlayersAfterRanking[i]);
                    break;
            }

        }

        for (int x = 0; x < CardGameManagerUI.instance.VotingCardHolders.Count; x++)
        {
            if (CardGameManagerUI.instance.VotingCardHolders[x].childCount > 0)
            {
                Destroy(CardGameManagerUI.instance.VotingCardHolders[x].GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < RankOne.Count; i++)
        {
            GameObject a = GameObject.Instantiate(CardManager.instance.card, new Vector3(0, 0, 0), Quaternion.identity);
            a.SetActive(true);
            a.transform.SetParent(CardGameManagerUI.instance.VotingCardHolders[i]);

            a.GetComponent<CardUI>().Initialize(RankOne[i]);
            a.GetComponent<CardUI>().DisableBackCard();
            a.GetComponent<CardUI>().ShowRanking();
        }

        for (int i = 0; i < RankTwo.Count; i++)
        {
            GameObject a = GameObject.Instantiate(CardManager.instance.card, new Vector3(0, 0, 0), Quaternion.identity);
            a.SetActive(true);
            a.transform.SetParent(CardGameManagerUI.instance.VotingCardHolders[i]);

            a.GetComponent<CardUI>().Initialize(RankTwo[i]);
            a.GetComponent<CardUI>().DisableBackCard();
            a.GetComponent<CardUI>().ShowRanking();
        }


        for (int i = 0; i < RankThree.Count; i++)
        {
            GameObject a = GameObject.Instantiate(CardManager.instance.card, new Vector3(0, 0, 0), Quaternion.identity);
            a.SetActive(true);
            a.transform.SetParent(CardGameManagerUI.instance.VotingCardHolders[i]);

            a.GetComponent<CardUI>().Initialize(RankThree[i]);
            a.GetComponent<CardUI>().DisableBackCard();
            a.GetComponent<CardUI>().ShowRanking();
        }


        for (int i = 0; i < RankFour.Count; i++)
        {
            GameObject a = GameObject.Instantiate(CardManager.instance.card, new Vector3(0, 0, 0), Quaternion.identity);
            a.SetActive(true);
            a.transform.SetParent(CardGameManagerUI.instance.VotingCardHolders[i]);

            a.GetComponent<CardUI>().Initialize(RankFour[i]);
            a.GetComponent<CardUI>().DisableBackCard();
            a.GetComponent<CardUI>().ShowRanking();
        }


        for (int i = 0; i < RankFive.Count; i++)
        {
            GameObject a = GameObject.Instantiate(CardManager.instance.card, new Vector3(0, 0, 0), Quaternion.identity);
            a.SetActive(true);
            a.transform.SetParent(CardGameManagerUI.instance.VotingCardHolders[i]);

            a.GetComponent<CardUI>().Initialize(RankFive[i]);
            a.GetComponent<CardUI>().DisableBackCard();
            a.GetComponent<CardUI>().ShowRanking();
        }


    }

    public void UpdateRemainingDeckUI()
    {
        CleanRemaining();
        DisplayRemainingCard();
        CardGameManagerUI.instance.UpdateRemainingScrollText(remainingCards.Count.ToString());

        //if remainging cards.count <=0 and all players has taken turn atlest once... change game state to stage 2
        if(remainingCards.Count <=0)
        {
            Debug.Log("CheckAllPlayersAndUpdateGameStage");
            CardGameManager.instance.CheckAllPlayersAndUpdateGameStage();
        }
    }

    public void CleanRemaining()
    {
        //delete all child in discarded deck content transform
        if (mainDeckRect.transform.childCount > 0)
        {
            for(int i=0;i< mainDeckRect.transform.childCount; i++)
            {
                Destroy(mainDeckRect.transform.GetChild(i).gameObject);
            }
        }
    }

    public void DisplayRemainingCard()
    {
        Debug.Log("[DisplayRemainingCard] remainingCards length" + remainingCards.Count);

        for (int i = 0; i < remainingCards.Count; i++)
        {
            GameObject a = GameObject.Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            a.transform.SetParent(mainDeckRect);
            a.GetComponent<CardUI>().Initialize(remainingCards[i]);
        }
    }

    public List<CardSO> GetCardListBasedOnIds(List<string> ids)
    {
        return System.Linq.Enumerable.ToList(cardDatabase.cards.Where(c => ids.Contains(c.cardId.ToString())));
    }

    public CardSO GetCardBasedOnId(string id)
    {
        return cardDatabase.cards.FirstOrDefault(c => c.cardId.ToString().Equals(id));
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
        return remainingCards;
    }

    public List<CardSO> GetRemaingCards()
    {
        return remainingCards;
    }
    //to shuffle the cards before distributing to players
    public void ShuffleCards()
    {
        remainingCards.Shuffle();
        shuffleCheck = true;
    }
}
