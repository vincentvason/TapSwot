using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI timeLimit;

    public TextMeshProUGUI displayTimeLimit;

    private bool _ready = false;

    public void RoomCreate()
    {
        roomName.text = "<font-weight=800>"+PhotonNetwork.CurrentRoom.Name;
        displayTimeLimit.text = "<font-weight=600>"+PhotonNetwork.CurrentRoom.CustomProperties["TIME_LIMIT"].ToString();
      
    }


    //private void SetReadyUp (bool state)
    //{
    //    _ready = state;
    //}
    //public void OnClickReadyButton()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        SetReadyUp(!_ready);
    //    }
    //}

    //[PunRPC]
    //private void RPC_ChangeReadyState(bool ready)
    //{

    //}
}
