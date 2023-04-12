using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Player : MonoBehaviour
{
    public string playerName;
    public int playerID;
    public List<CardSO> playerCards = new List<CardSO>();

    public TextMeshProUGUI playerNameText;

    public TextMeshProUGUI playerStatusText; // Beta implementation for Player status
    public string playeStatus;  // Beta implementation for Player status

    Photon.Realtime.Player MyPlayer;

    public List<string> cardIds = new List<string>();

    public List<CardUI> cardsUI = new List<CardUI>();
    public void InitialiseNetworkPlayer(Photon.Realtime.Player player)
    {
        MyPlayer = player;
        this.playerName = MyPlayer.NickName;
        this.playerID = MyPlayer.ActorNumber;
        playerNameText.text = this.playerName;
    }


    public void InitializeCards (List<CardSO> playercards, int actorNumber)
    {
        if(this.playerID == actorNumber)
        {
            this.playerCards = playercards;
        }        
    }

    public void ReceiveCards(string c1, string c2, string c3, string c4, string c5)
    {
        cardIds.Add(c1);
        cardIds.Add(c2);
        cardIds.Add(c3);
        cardIds.Add(c4);
        cardIds.Add(c5);

        playerCards = CardManager.instance.GetCardListBasedOnIds(cardIds);

        if (cardsUI.Count > 0)
        {
            cardsUI[0].Initialize(playerCards[0]);
            cardsUI[1].Initialize(playerCards[1]);
            cardsUI[2].Initialize(playerCards[2]);
            cardsUI[3].Initialize(playerCards[3]);
            cardsUI[4].Initialize(playerCards[4]);


            CardManager.instance.UpdateDiscardedCards(playerCards[0]);
            CardManager.instance.UpdateDiscardedCards(playerCards[1]);
            CardManager.instance.UpdateDiscardedCards(playerCards[2]);
            CardManager.instance.UpdateDiscardedCards(playerCards[3]);
            CardManager.instance.UpdateDiscardedCards(playerCards[4]);

            CardManager.instance.UpdateDiscardedDeckUI();
        }

    }




}
