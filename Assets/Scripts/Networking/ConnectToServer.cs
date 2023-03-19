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
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connected to the Photon Server");
    }

    public override void OnConnectedToMaster()
    {
        
        SceneManager.LoadScene(1);
    }


}
