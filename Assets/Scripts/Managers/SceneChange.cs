using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneChange : MonoBehaviourPunCallbacks 
{
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
        SceneManager.LoadScene(1);
    }

}
