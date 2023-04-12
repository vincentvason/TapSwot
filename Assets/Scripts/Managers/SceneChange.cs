using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneChange : MonoBehaviourPunCallbacks 
{
    public MainMenuAnimation transition;
    private float time = 0;


    public void onSelectQuit()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    public void OnSelectLeaveRoom()
    {
        
        PhotonNetwork.LeaveRoom();
        Debug.Log("Room Left : " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnLeftRoom()
    {
        StartCoroutine(transition.CloseLobbyWaitingWindowAndLoadScene(1));
        // SceneManager.LoadScene(1);
    }

}
