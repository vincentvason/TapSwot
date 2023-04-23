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

    private bool cardRemovedFromSlot = false;
    private bool cardAddedBackToSlot = false;

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

            CardGameManagerUI.instance.DiscardedDeckScroll.OnElementAdded.AddListener(OnDiscardedElementAdded);
            CardGameManagerUI.instance.DiscardedDeckScroll.OnElementDropped.AddListener(OnDiscardedElementDropped);

            CardGameManagerUI.instance.DiscardedDeckScroll.OnElementGrabbed.AddListener(OnElementDraggedFromDiscarded);

            CardGameManagerUI.instance.RemainingDeckScroll.OnElementAdded.AddListener(OnRemainingElementAdded);
            CardGameManagerUI.instance.RemainingDeckScroll.OnElementDropped.AddListener(OnRemainingElementDropped);
        }
        //to-do     //update when something is added to remaining cards or removed from remaining cards
        CardGameManager.OnGameStateChanged += Instance_OnGameStateChanged;
    }

    private void Instance_OnGameStateChanged(GameStateEnum gameState)
    {
        if (this == PlayerManager.instance.myPlayer)
        {
            foreach(CardUI c in cardsUI)
            {
                c.EnableRankDropdown();
            }
        }
    }


    /// <summary>
    /// on drag started from discared scroll, then disable drop in remaining scroll
    /// </summary>
    /// <param name="grabbedStruct"></param>
    private void OnElementDraggedFromDiscarded(ReorderableList.ReorderableListEventStruct grabbedStruct)
    {
        CardGameManagerUI.instance.RemainingDeckScroll.IsDropable = false;
    }

    /// <summary>
    /// on drag started from slot, enable drop in remaining scroll
    /// </summary>
    /// <param name="grabbedStruct"></param>
    private void OnElementDraggedFromSlot(ReorderableList.ReorderableListEventStruct grabbedStruct)
    {
        CardGameManagerUI.instance.RemainingDeckScroll.IsDropable = true;

        //disable drag of other elements unitl slot is filled
        DisableDragOnAllCardSlots();
    }

    private void DisableDragOnAllCardSlots()
    {
        //playerDraggableCards[0].IsDraggable = false;
        //playerDraggableCards[1].IsDraggable = false;
        //playerDraggableCards[2].IsDraggable = false;
        //playerDraggableCards[3].IsDraggable = false;
        //playerDraggableCards[4].IsDraggable = false;
    }

    private void EnableDragOnAllCardSlots()
    {
        playerDraggableCards[0].IsDraggable = true;
        playerDraggableCards[1].IsDraggable = true;
        playerDraggableCards[2].IsDraggable = true;
        playerDraggableCards[3].IsDraggable = true;
        playerDraggableCards[4].IsDraggable = true;
    }

    public void OnTurnReceived()
    {
        Debug.Log("[OnTurnReceived int]:" + "[playerID]:" + playerID);
        if (this == PlayerManager.instance.myPlayer)
        {
            //show our turn received and update UI from CardGameManager based on that.
            EnableMyPlayerUI();
            CardGameManagerUI.instance.ShowItsYourTurn();
        }
    }

    public void OtherPlayerTurn()
    {
        if (this == PlayerManager.instance.myPlayer)
        {
            Debug.Log("[OtherPlayerTurn int]:" + "[playerID]:" + playerID);

            DisableMyPlayerUI();
            CardGameManagerUI.instance.ShowWaitForTurn();
        }
    }

    public void CheckAndSendRPC(ReorderableList.ReorderableListEventStruct droppedStruct, string cardSlot)
    {
        if (droppedStruct.FromList.name == "DiscardedCardsScroll" && droppedStruct.ToList.name == cardSlot)
        {
            PlayerManager.instance.Send_RemoveFromDiscardedCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
        }
        if (droppedStruct.FromList.name == "RemainingCardsScroll" && droppedStruct.ToList.name == cardSlot)
        {
            PlayerManager.instance.Send_RemoveFromRemainingCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
        }
    }

    private void ElementDropped0(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if(droppedStruct.ToList!=null)
        {
            if (droppedStruct.FromList.name != "Card1" && droppedStruct.ToList.name == "Card1")
            {
                CardGameManagerUI.instance.ShowConfirmReplace();
                DisableMyPlayerUI();
                cardAddedBackToSlot = true;
                string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
                droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();
                Debug.Log("card dropped in slot 1, card id :" + id);
                PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "0", id); //if card is added to slot, turn is complete
            }
            CheckAndSendRPC(droppedStruct, "Card1");
        }
    }
    private void ElementDropped1(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if (droppedStruct.ToList != null)
        {
            if (droppedStruct.FromList.name != "Card2" && droppedStruct.ToList.name == "Card2")
            {
                CardGameManagerUI.instance.ShowConfirmReplace();
                DisableMyPlayerUI();
                cardAddedBackToSlot = true;
                string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
                droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();
                Debug.Log("card dropped in slot 2, card id :" + id);
                PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "1", id); //if card is added to slot, turn is complete
            }
            CheckAndSendRPC(droppedStruct, "Card2");
        }
    }
    private void ElementDropped2(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if (droppedStruct.ToList != null)
        {
            if (droppedStruct.FromList.name != "Card3" && droppedStruct.ToList.name == "Card3")
            {
                CardGameManagerUI.instance.ShowConfirmReplace();
                DisableMyPlayerUI();
                cardAddedBackToSlot = true;
                string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
                droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();
                Debug.Log("card dropped in slot 3, card id :" + id);
                PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "2", id); //if card is added to slot, turn is complete
            }
            CheckAndSendRPC(droppedStruct, "Card3");
        }
    }
    private void ElementDropped3(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if (droppedStruct.ToList != null)
        {
            if (droppedStruct.FromList.name != "Card4" && droppedStruct.ToList.name == "Card4")
            {
                CardGameManagerUI.instance.ShowConfirmReplace();
                DisableMyPlayerUI();
                cardAddedBackToSlot = true;
                string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
                droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();
                Debug.Log("card dropped in slot 4, card id :" + id);
                PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "3", id); //if card is added to slot, turn is complete
            }
            CheckAndSendRPC(droppedStruct, "Card4");
        }
    }
    private void ElementDropped4(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if (droppedStruct.ToList != null)
        {
            if(droppedStruct.FromList.name != "Card5" && droppedStruct.ToList.name == "Card5")
            {
                CardGameManagerUI.instance.ShowConfirmReplace();
                DisableMyPlayerUI();
                cardAddedBackToSlot = true;
                string id = droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString();
                droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();
                Debug.Log("card dropped in slot 5, card id :" + id);
                PlayerManager.instance.SendPlayerCardChanged(this.playerID.ToString(), "4", id); //if card is added to slot, turn is complete
            }
            CheckAndSendRPC(droppedStruct, "Card5");
        }
    }

    private void OnDiscardedElementDropped(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if (droppedStruct.ToList != null && droppedStruct.ToList.name == "DiscardedCardsScroll") //added in discarded scroll
        {
            droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();

            //added from players cards slot
            if (droppedStruct.FromList != null && droppedStruct.FromList.name == "Card1" || droppedStruct.FromList.name == "Card2" || droppedStruct.FromList.name == "Card3"
                || droppedStruct.FromList.name == "Card4" || droppedStruct.FromList.name == "Card5")
            {
                PlayerManager.instance.Send_AddToDiscardedCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
                cardRemovedFromSlot = true;
                Debug.Log("dropped to discarded from card slot");
                CardGameManagerUI.instance.ShowSelectFromRemaining();
            }
            //added from remaining cards scroll
            else if (droppedStruct.FromList != null && droppedStruct.FromList.name == "RemainingCardsScroll")
            {
                PlayerManager.instance.Send_AddToDiscardedCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
                PlayerManager.instance.Send_RemoveFromRemainingCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
                Debug.Log("dropped to discarded from RemainingCardsScroll");
            }
        }
    }

    private void OnDiscardedElementAdded(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if (droppedStruct.ToList != null && droppedStruct.ToList.name == "DiscardedCardsScroll") //added in discarded scroll
        {
            droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();

            //added from players cards slot
            if (droppedStruct.FromList!=null && droppedStruct.FromList.name == "Card1" || droppedStruct.FromList.name == "Card2" || droppedStruct.FromList.name == "Card3"
                || droppedStruct.FromList.name == "Card4" || droppedStruct.FromList.name == "Card5")
            {
                PlayerManager.instance.Send_AddToDiscardedCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
                cardRemovedFromSlot = true;
                CardGameManagerUI.instance.RemainingDeckScroll.IsDropable = true;
                CardGameManagerUI.instance.ShowSelectFromRemaining();
                Debug.Log("added to discarded from card slot");
            }
            //added from remaining cards scroll
            else if (droppedStruct.FromList != null && droppedStruct.FromList.name == "RemainingCardsScroll")
            {
                PlayerManager.instance.Send_AddToDiscardedCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
                PlayerManager.instance.Send_RemoveFromRemainingCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
                Debug.Log("added to discarded from RemainingCardsScroll");
            }
        }
    }

    private void OnRemainingElementDropped(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if (droppedStruct.ToList != null && droppedStruct.ToList.name == "RemainingCardsScroll") //added in discarded scroll
        {
            droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();

            //added from players cards slot
            if (droppedStruct.FromList != null && droppedStruct.FromList.name == "Card1" || droppedStruct.FromList.name == "Card2" || droppedStruct.FromList.name == "Card3"
                || droppedStruct.FromList.name == "Card4" || droppedStruct.FromList.name == "Card5")
            {
                PlayerManager.instance.Send_AddToRemainingCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
                cardRemovedFromSlot = true;
                CardGameManagerUI.instance.RemainingDeckScroll.IsDropable = true;
                CardGameManagerUI.instance.ShowSelectFromRemaining();
                Debug.Log("dropped to remaining from card slot");
            }
            //added from remaining cards scroll
            else if (droppedStruct.FromList != null && droppedStruct.FromList.name == "DiscardedCardsScroll")
            {
                Debug.Log("dropped to remaining from DiscardedCardsScroll");
            }
        }
    }

    private void OnRemainingElementAdded(ReorderableList.ReorderableListEventStruct droppedStruct)
    {
        if (droppedStruct.ToList != null && droppedStruct.ToList.name == "RemainingCardsScroll") //added in discarded scroll
        {
            droppedStruct.DroppedObject.GetComponent<CardUI>().CheckParentAndSetBackCard();

            //added from players cards slot
            if (droppedStruct.FromList != null && droppedStruct.FromList.name == "Card1" || droppedStruct.FromList.name == "Card2" || droppedStruct.FromList.name == "Card3"
                || droppedStruct.FromList.name == "Card4" || droppedStruct.FromList.name == "Card5")
            {
                PlayerManager.instance.Send_AddToRemainingCards(droppedStruct.DroppedObject.GetComponent<CardUI>().card.cardId.ToString());
                cardRemovedFromSlot = true;
                CardGameManagerUI.instance.RemainingDeckScroll.IsDropable = true;
                CardGameManagerUI.instance.ShowSelectFromRemaining();
                Debug.Log("added to remaining from card slot");
            }
            //added from remaining cards scroll
            else if (droppedStruct.FromList != null && droppedStruct.FromList.name == "DiscardedCardsScroll")
            {
                Debug.Log("added to remaining from DiscardedCardsScroll");
            }
        }
    }


    public void ReceiveUpdatedCardInSlot(string cardSlot, string cardId)
    {
        CardSO card = CardManager.instance.GetCardBasedOnId(cardId);
        int slot = 0;
        int.TryParse(cardSlot, out slot);

        cardIds[slot] = cardId;
        if (this == PlayerManager.instance.myPlayer)
        {
            if(cardsUI[slot] != null)
            {
                cardsUI[slot].card = card;
                cardsUI[slot].Initialize(card);
            }
        }
    }

    public Dictionary<int, CardUI> SetRanks = new Dictionary<int, CardUI>();
    public void SetCardRank(CardUI cardUI, int value)
    {
        if (!SetRanks.ContainsKey(value))
        {
            SetRanks.Add(value, cardUI);
        }

        foreach(CardUI c in cardsUI)
        {
            if(c!= cardUI)
            {
                c.EnableAllRanks();
                c.DisableOneRank(value);
            }
        }
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
            PlayerManager.instance.ShuffleAnimation.SetActive(false);

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
            playerDraggableCards[0].OnElementAdded.AddListener(ElementDropped0);
            playerDraggableCards[1].OnElementAdded.AddListener(ElementDropped1);
            playerDraggableCards[2].OnElementAdded.AddListener(ElementDropped2);
            playerDraggableCards[3].OnElementAdded.AddListener(ElementDropped3);
            playerDraggableCards[4].OnElementAdded.AddListener(ElementDropped4);

            playerDraggableCards[0].OnElementGrabbed.AddListener(OnElementDraggedFromSlot);
            playerDraggableCards[1].OnElementGrabbed.AddListener(OnElementDraggedFromSlot);
            playerDraggableCards[2].OnElementGrabbed.AddListener(OnElementDraggedFromSlot);
            playerDraggableCards[3].OnElementGrabbed.AddListener(OnElementDraggedFromSlot);
            playerDraggableCards[4].OnElementGrabbed.AddListener(OnElementDraggedFromSlot);

            DisableMyPlayerUI(); //we disable interactions until it is our turn to play

            PlayerManager.instance.ShuffleAnimation.SetActive(true);
            cardsUI[0].gameObject.SetActive(false);
            cardsUI[1].gameObject.SetActive(false);
            cardsUI[2].gameObject.SetActive(false);
            cardsUI[3].gameObject.SetActive(false);
            cardsUI[4].gameObject.SetActive(false);
            PlayerManager.instance.ShuffleAnimation.GetComponent<CardShuffling>().StartShuffleAnimation();
        }
    }

    public void DisableMyPlayerUI()
    {
        if (this == PlayerManager.instance.myPlayer)
        {
            DisableDragOnAllCardSlots();
            CardGameManagerUI.instance.RemainingDeckScroll.IsDraggable = false;
            CardGameManagerUI.instance.DiscardedDeckScroll.IsDraggable = false;
        }
    }
    public void EnableMyPlayerUI()
    {
        if (this == PlayerManager.instance.myPlayer)
        {
            EnableDragOnAllCardSlots(); //only enable drag on slots when it is my turn
            CardGameManagerUI.instance.RemainingDeckScroll.IsDraggable = true;
            CardGameManagerUI.instance.DiscardedDeckScroll.IsDraggable = true;
        }
    }




}
