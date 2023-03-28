using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        //check players
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        PlayerManager.instance.InitializeNetworkPlayers(PhotonNetwork.CurrentRoom.Players);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
       
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        
    }
}
