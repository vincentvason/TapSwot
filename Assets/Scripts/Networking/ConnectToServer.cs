using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

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
        SceneManager.LoadScene(1);
        // PhotonNetwork.JoinLobby();
        
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene(1);
    }

}
