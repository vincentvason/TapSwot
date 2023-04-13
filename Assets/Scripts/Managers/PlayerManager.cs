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

                localPlayer.GetComponent<Player>().InitialiseNetworkPlayer(PhotonNetwork.LocalPlayer);
                myPlayer = localPlayer.GetComponent<Player>();
                currentPlayersList.Add(myPlayer);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            SendForInitilizeCards();
        }
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
                CardManager.instance.RemoveCardFromDeck(c);
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
        gameObject.GetComponent<PhotonView>().RPC("FirstPlayerTurn", RpcTarget.All);
    }

    public void SendPlayerCardChanged(string actorID, string cardSlot, string cardId)
    {
        gameObject.GetComponent<PhotonView>().RPC("ReceivePlayerCardChanged", RpcTarget.All, actorID, cardSlot, cardId);
    }

    [PunRPC]
    public void FirstPlayerTurn()
    {
        foreach (Player p in currentPlayersList)
        {
            if (p.playerID.ToString() == "1")
            {
                p.OnTurnReceived();
            }
            else
            {
                p.OtherPlayerTurn();
            }
            CardGameManagerUI.instance.UpdatePlayerTurnText();
        }
    }

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
        CardManager.instance.UpdateDiscardedDeckUI();
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
    }

}
