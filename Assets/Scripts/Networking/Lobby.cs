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



    void Awake()
    {
        roomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
    }

    public void OnSelectLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("Room Left : " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
    }


}
