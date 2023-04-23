using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<Player> currentPlayersList = new List<Player>();
    public Player myPlayer;

    public RectTransform[] allPlayersGameobject = new RectTransform[4];
    public GameObject myPlayerPrefab, otherPlayerPrefab;

    public RectTransform Otherplayer_content,Myplayer_content;
    

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
        DontDestroyOnLoad(gameObject);

        PlayerManager.instance.ShuffleAnimation.SetActive(false);
    }

    public int CurrentPlayerTurn()
    {
        return CardGameManager.instance.CurrentTurn();
    }

    public void InitializeNetworkPlayers(Dictionary<int, Photon.Realtime.Player> players)
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Players.Count);
        foreach (KeyValuePair<int, Photon.Realtime.Player> kvp in (PhotonNetwork.CurrentRoom.Players))
        {
            if(kvp.Value != PhotonNetwork.LocalPlayer)
            {
                Debug.Log("not local");

                GameObject a = GameObject.Instantiate(otherPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                a.transform.SetParent(Otherplayer_content);


                //GameObject a = GameObject.Instantiate(otherPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                //a.transform.SetParent(allPlayersGameobject[kvp.Value.ActorNumber - 1]);
                //a.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                //a.GetComponent<RectTransform>().transform.localPosition = new Vector3(0, 0, 0);

                a.GetComponent<Player>().InitialiseNetworkPlayer(kvp.Value);
                currentPlayersList.Add(a.GetComponent<Player>());
            }
            else
            {
                Debug.Log("local");
                GameObject localPlayer = GameObject.Instantiate(myPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                localPlayer.transform.SetParent(Myplayer_content);



                //GameObject localPlayer = GameObject.Instantiate(myPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                //localPlayer.transform.SetParent(allPlayersGameobject[kvp.Value.ActorNumber - 1]);
                //localPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                //localPlayer.GetComponent<RectTransform>().transform.localPosition = new Vector3(0, 0, 0);
                myPlayer = localPlayer.GetComponent<Player>();
                localPlayer.GetComponent<Player>().InitialiseNetworkPlayer(PhotonNetwork.LocalPlayer);
                currentPlayersList.Add(myPlayer);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            SendForInitilizeCards();
        }
    }

    public void Send_AddToRemainingCards(string cardID)
    {
        gameObject.GetComponent<PhotonView>().RPC("Receive_AddToRemainingCards", RpcTarget.All, cardID);
    }

    public void Send_RemoveFromRemainingCards(string cardID)
    {
        gameObject.GetComponent<PhotonView>().RPC("Receive_RemoveFromRemainingCards", RpcTarget.All, cardID);
    }

    public void Send_AddToDiscardedCards(string cardID)
    {
        gameObject.GetComponent<PhotonView>().RPC("ReceiveSend_AddToDiscardedCards", RpcTarget.All, cardID);
    }

    public void Send_RemoveFromDiscardedCards(string cardID)
    {
        gameObject.GetComponent<PhotonView>().RPC("Receive_RemoveFromDiscardedCards", RpcTarget.All, cardID);
    }

    public void Send_PlayerRankUpdate()
    {
        //playerID
        //[string array] "cardID","cardRank"
        string playerID = myPlayer.playerID.ToString();
        List<CardUI> cards = myPlayer.cardsUI;

        List<string> cardIDs = new List<string>();
        List<string> cardRanks = new List<string>();

        foreach (CardUI card in cards)
        {
            cardIDs.Add(card.card.cardId.ToString());
            card.card.cardRank = card.rankDropdown.value;
            cardRanks.Add(card.card.cardRank.ToString());
        }

        gameObject.GetComponent<PhotonView>().RPC("ReceiveCardsAndRankByPlayer", RpcTarget.All, playerID, (object)cardIDs.ToArray(), (object)cardRanks.ToArray());
    }

    public List<CardSO> ReceivedCardsFromAllPlayersAfterRanking = new List<CardSO>();
    int ReceivedCardsFromAllPlayersAfterRankingCount = 0;
    [PunRPC]
    public void ReceiveCardsAndRankByPlayer(string playerID, string[] cardIDs, string[] rank)
    {
        Debug.Log("-------------------------------------");
        Debug.Log("Player ID:" + playerID);
        var idList = new List<string>(cardIDs);
        var rankList = new List<string>(rank);


        for(int i = 0; i < idList.Count; i++)
        {
            int r = 0;
            int.TryParse(rankList[i], out r);

            CardSO cardSO = CardManager.instance.GetCardBasedOnId(idList[i]);
            cardSO.cardRank = r;
            Debug.Log("CardID:" + cardSO.cardId + ",CardName:" + cardSO.cardTitle + "," + "CardRank:" + cardSO.cardRank);
            ReceivedCardsFromAllPlayersAfterRanking.Add(cardSO);
        }

        Debug.Log("-------------------------------------");
        ReceivedCardsFromAllPlayersAfterRankingCount++;

        if(ReceivedCardsFromAllPlayersAfterRankingCount>= PlayerManager.instance.GetCurrentPlayersList().Count)
        {
            //all players have ranked. Send RPC for stage change
            PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_THREE.ToString());
            PlayerManager.instance.SendPlayerTurnUpdate(CardGameManager.instance.lastTurn.ToString(), CardGameManager.instance.currentTurn.ToString());
        }
    }


    [PunRPC]
    public void Receive_AddToRemainingCards(string cardID)
    {
        CardManager.instance.AddCardToRemainingCards(cardID);
        CardManager.instance.UpdateRemainingDeckUI();
    }

    [PunRPC]
    public void Receive_RemoveFromRemainingCards(string cardID)
    {
        CardSO co = CardManager.instance.GetCardBasedOnId(cardID);
        CardManager.instance.RemoveCardFromRemainingDeck(co);
        CardManager.instance.UpdateRemainingDeckUI();
    }

    [PunRPC]
    public void ReceiveSend_AddToDiscardedCards(string cardID)
    {
        CardManager.instance.AddCardToDiscardedCards(cardID);
        CardManager.instance.UpdateDiscardedDeckUI();
    }

    [PunRPC]
    public void Receive_RemoveFromDiscardedCards(string cardID)
    {
        CardManager.instance.RemoveCardFromDiscardedCards(cardID);
        CardManager.instance.UpdateDiscardedDeckUI();
    }



    public void SendForInitilizeCards()
    {
        int i = 0;
        foreach (KeyValuePair<int, Photon.Realtime.Player> kvp in (PhotonNetwork.CurrentRoom.Players))
        {
            List<string> cardId = new List<string>();
            string actorsNumber = string.Empty;
            actorsNumber= (kvp.Value.ActorNumber.ToString());

            for (int j = 0; j < 5; j++)
            {
                CardSO c = CardManager.instance.GetShuffledCards().RandomElement();
                CardManager.instance.RemoveCardFromRemainingDeck(c);
                cardId.Add(c.cardId.ToString());
                i++;
            }

            gameObject.GetComponent<PhotonView>().RPC("ReceiveShuffledCards", RpcTarget.All, actorsNumber,
                cardId[0],
                cardId[1],
                cardId[2],
                cardId[3],
                cardId[4]);
        }
        Debug.Log("[Distributed cards] " + i);
        Debug.Log("[GetRemaingCards length] " + CardManager.instance.GetRemaingCards().Count);

        //get the ids of remaining cards
        List<string> remainingCardsID = new List<string>();
        foreach(CardSO so in CardManager.instance.GetRemaingCards())
        {
            remainingCardsID.Add(so.cardId.ToString());
        }
        //update all clients with the remaining cards
        gameObject.GetComponent<PhotonView>().RPC("ReceiveRemainingShuffleCards", RpcTarget.All, (object)remainingCardsID.ToArray());

        //Start First Players Turn here
        SendPlayerTurnUpdate("0", "1");
        if (!CardGameManager.instance.ROUND_ONE_PlayersThatHaveTakenTurn.Contains("1"))
        {
            CardGameManager.instance.ROUND_ONE_PlayersThatHaveTakenTurn.Add("1");
        }
    }

    public void SendRoundRPC(string round)
    {
        gameObject.GetComponent<PhotonView>().RPC("ReceiveRound", RpcTarget.All, round);
    }

    public void SendPlayerCardChanged(string actorID, string cardSlot, string cardId)
    {
        gameObject.GetComponent<PhotonView>().RPC("ReceivePlayerCardChanged", RpcTarget.All, actorID, cardSlot, cardId);
    }

    public void SendPlayerTurnUpdate(string lastTurn, string currentTurn)
    {
        gameObject.GetComponent<PhotonView>().RPC("ReceivePlayerTurnValue", RpcTarget.All, lastTurn, currentTurn);
    }

    [PunRPC]
    public void ReceiveRound(string round)
    {
        switch (round)
        {
            case "ROUND_TWO":
                CardGameManager.instance.UpdateGameState(GameStateEnum.ROUND_TWO);
                break;
            case "ROUND_THREE":
                CardGameManager.instance.UpdateGameState(GameStateEnum.ROUND_THREE);
                break;
            case "ROUND_FOUR":
                CardGameManager.instance.UpdateGameState(GameStateEnum.ROUND_FOUR);
                break;
        }
    }

    [PunRPC]
    public void ReceivePlayerTurnValue(string lastTurn, string currentTurn)
    {
        CardGameManager.instance.UpdateTurnValueFromRPC(currentTurn);
        int last = 0;
        int current = 0;
        int.TryParse(lastTurn, out last);
        int.TryParse(currentTurn, out current);

        Debug.Log("[ReceivePlayerTurnValue]:" + "[lastTurn]:" + lastTurn + "[currentTurn]" + currentTurn);
        Debug.Log("[ReceivePlayerTurnValue int]:" + "[lastTurn]:" + last + "[currentTurn]" + current);

        foreach (Player p in currentPlayersList)
        {
            if (p.playerID.ToString() == last.ToString())
            {
                p.OtherPlayerTurn();
            }
            
            if(p.playerID.ToString() == current.ToString())
            {
                p.OnTurnReceived();
            }
            else
            {
                p.OtherPlayerTurn();
            }
        }
        CardGameManagerUI.instance.UpdatePlayerTurnText();
    }

    public GameObject ShuffleAnimation;

    [PunRPC]
    public void ReceivePlayerCardChanged(string actorID, string cardSlot, string cardId)
    {
        foreach (Player p in currentPlayersList)
        {
            if (p.playerID.ToString() == actorID)
            {   
                p.ReceiveUpdatedCardInSlot(cardSlot, cardId);
            }
        }
    }

    [PunRPC]
    public void ReceiveRemainingShuffleCards(string[] ids)
    {
        Debug.Log("[ReceiveRemainingShuffleCards] ids length" + ids.Length);
        var list = new List<string>(ids);
        var cardSOs = CardManager.instance.GetCardListBasedOnIds(list);
        Debug.Log("[ReceiveRemainingShuffleCards] ids length" + cardSOs.Count);
        CardManager.instance.UpdateDeckFromData(cardSOs);        
        CardManager.instance.UpdateRemainingDeckUI();
    }

    [PunRPC]
    public void ReceiveShuffledCards(string actorID, string c1, string c2, string c3, string c4, string c5)
    {
        Debug.Log("actorID other" + actorID);
        Debug.Log(c1 + "," + c2 + "," + c3 + "," + c4 + "," + c5);

        foreach(Player p in currentPlayersList)
        {
            if (p.playerID.ToString() == actorID)
            {
                Debug.Log("actorID self" + actorID);
                p.ReceiveShuffledCards(c1,c2,c3,c4,c5);
            }
        }
        CardGameManager.instance.UpdateGameState(GameStateEnum.ROUND_ONE);
    }

    public List<Player> GetCurrentPlayersList()
    {
        return currentPlayersList;
    }

}
