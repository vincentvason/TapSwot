using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<Player> localPlayersList = new List<Player>();
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
                localPlayersList.Add(a.GetComponent<Player>());
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
                localPlayersList.Add(myPlayer);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            SendForInitilizeCards();
        }
    }

    public void SendForInitilizeCards()
    {
        foreach (KeyValuePair<int, Photon.Realtime.Player> kvp in (PhotonNetwork.CurrentRoom.Players))
        {
            List<string> cardId = new List<string>();
            string actorsNumber = string.Empty;
            actorsNumber= (kvp.Value.ActorNumber.ToString());

            for (int j = 0; j < 5; j++)
            {
                CardSO c = CardManager.instance.GetShuffledCards()[CardManager.instance.GetShuffledCards().Count - 1];
                CardManager.instance.GetShuffledCards().RemoveAt(CardManager.instance.GetShuffledCards().Count - 1);
                cardId.Add(c.cardId.ToString());
            }

            gameObject.GetComponent<PhotonView>().RPC("ReceiveCards", RpcTarget.All, actorsNumber,
                cardId[0],
                cardId[1],
                cardId[2],
                cardId[3],
                cardId[4]);

        }

    }

    [PunRPC]
    public void ReceiveCards(string actorID, string c1, string c2, string c3, string c4, string c5)
    {
        Debug.Log("actorID other" + actorID);
        Debug.Log(c1 + "," + c2 + "," + c3 + "," + c4 + "," + c5);

        foreach(Player p in localPlayersList)
        {
            if (p.playerID.ToString() == actorID)
            {
                Debug.Log("actorID self" + actorID);
                p.ReceiveCards(c1,c2,c3,c4,c5);
            }
        }
    }

}
