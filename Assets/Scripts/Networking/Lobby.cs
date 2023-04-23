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
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        displayTimeLimit.text = timeLimit.text;

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
