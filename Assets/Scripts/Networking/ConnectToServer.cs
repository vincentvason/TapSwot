using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //Establish connection of Player and Photon Server
        PhotonNetwork.GameVersion="1.0";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connected to the Photon Server");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene(1);
    }


}
