using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    public SceneTransition sceneTransition;
    
    private void Start()
    {
        //Establish connection of Player and Photon Server
        PhotonNetwork.GameVersion="1.0";
      
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connected to the Photon Server");
    }

    public override void OnConnectedToMaster()
    {
        StartCoroutine("StartGame");
    }

    public override void OnJoinedLobby()
    {
     //    SceneManager.LoadScene(1);
    }

    IEnumerator StartGame()
    {
      
        yield return new WaitForSeconds(30);
        StartCoroutine(sceneTransition.SceneTransitionBegin(1));
    }

    public void StartGameNow()
    {
        StopAllCoroutines();
        StartCoroutine(sceneTransition.SceneTransitionBegin(1));
        
    }

}
