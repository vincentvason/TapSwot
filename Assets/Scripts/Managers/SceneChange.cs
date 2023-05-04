using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SceneChange : MonoBehaviourPunCallbacks 
{
    public MainMenuAnimation transition;
    public TMP_Text TapAvatarInfo;
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

    public void UpdateAvatarTextOnProceed() 
    {
        TapAvatarInfo.text = "Please choose to create a new room or join a room";
    }

    public void UpdateAvatarTextOnCreate()
    {
        TapAvatarInfo.text = "Please enter a name for your room and indicate how much time you want for the game session.";
    }

    public void UpdateAvatarTextOnJoin()
    {
        TapAvatarInfo.text = "Please enter the exact name for the room you wish to join";
    }
}
