using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneChange : MonoBehaviour 
{


    


    public void onSelectQuit()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    public void OnSelectStartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(3);
        }
        else
        {
            Debug.Log("Insufficient Players in the Room, Current Players : "+ PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
    

   
}
