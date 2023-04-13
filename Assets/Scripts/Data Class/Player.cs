using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI.Extensions;

public class Player : MonoBehaviour
{
    public string playerName;
    public int playerID;

    public TextMeshProUGUI playerNameText;

    public TextMeshProUGUI playerStatusText; // Beta implementation for Player status
    public string playeStatus;  // Beta implementation for Player status

    Photon.Realtime.Player MyPlayer;

    public List<string> cardIds = new List<string>();

    public List<CardUI> cardsUI = new List<CardUI>();

    public List<ReorderableList> playerDraggableCards = new List<ReorderableList>();
    public void InitialiseNetworkPlayer(Photon.Realtime.Player player)
    {
        MyPlayer = player;
        this.playerName = MyPlayer.NickName;
        this.playerID = MyPlayer.ActorNumber;
        playerNameText.text = this.playerName;

        if (this == PlayerManager.instance.myPlayer)
        {
            cardsUI = CardGameManagerUI.instance.clientCardsUI;
            playerDraggableCards = CardGameManagerUI.instance.playerDraggableCards;
        }
    }

    private void ElementDropped0(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
        Debug.Log("card dropped in slot 1, card id :" + id);
        PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(),"0", id);
    }
    private void ElementDropped1(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
        Debug.Log("card dropped in slot 2, card id :" + id);
        PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "1", id);
    }
    private void ElementDropped2(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
        Debug.Log("card dropped in slot 3, card id :" + id);
        PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "2", id);
    }
    private void ElementDropped3(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
        Debug.Log("card dropped in slot 4, card id :" + id);
        PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "3", id);
    }
    private void ElementDropped4(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
        Debug.Log("card dropped in slot 5, card id :" + id);
        PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "4", id);
    }

    public void ReceiveUpdatedCardInSlot(string cardSlot, string cardId)
    {
        CardSO card = CardManager.instance.GetCardBasedOnId(cardId);
        int slot = 0;
        int.TryParse(cardSlot, out slot);

        cardIds[slot] = cardId;
        cardsUI[slot].card = card;
        cardsUI[slot].Initialize(card);
    }

    public void ReceiveShuffledCards(string c1, string c2, string c3, string c4, string c5)
    {
        cardIds.Add(c1);
        cardIds.Add(c2);
        cardIds.Add(c3);
        cardIds.Add(c4);
        cardIds.Add(c5);

        if (this == PlayerManager.instance.myPlayer)
        {
            cardsUI = CardGameManagerUI.instance.clientCardsUI;

            foreach (CardUI c in cardsUI)
            {
                c.gameObject.SetActive(true);
            }
            
            if (cardsUI.Count > 0)
            {
                cardsUI[0].Initialize(CardManager.instance.GetCardBasedOnId(cardIds[0]));
                cardsUI[1].Initialize(CardManager.instance.GetCardBasedOnId(cardIds[1]));
                cardsUI[2].Initialize(CardManager.instance.GetCardBasedOnId(cardIds[2]));
                cardsUI[3].Initialize(CardManager.instance.GetCardBasedOnId(cardIds[3]));
                cardsUI[4].Initialize(CardManager.instance.GetCardBasedOnId(cardIds[4]));
            }

            playerDraggableCards = CardGameManagerUI.instance.playerDraggableCards;

            //add listeners to check if player cards is changed by replcing from remaining cards
            playerDraggableCards[0].OnElementDropped.AddListener(ElementDropped0);
            playerDraggableCards[1].OnElementDropped.AddListener(ElementDropped1);
            playerDraggableCards[2].OnElementDropped.AddListener(ElementDropped2);
            playerDraggableCards[3].OnElementDropped.AddListener(ElementDropped3);
            playerDraggableCards[4].OnElementDropped.AddListener(ElementDropped4);
        }
    }

    public void DisableMyPlayerUI()
    {

    }
    public void EnableMyPlayerUI()
    {

    }




}
