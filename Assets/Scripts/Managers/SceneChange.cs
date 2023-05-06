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
    public Username user;
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
        StartCoroutine(transition.CloseLobbyWaitingWindow());
        StartCoroutine(transition.OpenCreateJoinWindow());
        // SceneManager.LoadScene(1);
    }

    public void UpdateAvatarTextOnProceed() 
    {
        TapAvatarInfo.text = "<font-weight=600>Hello, " + user.username + ". Please choose to create a new room or join a room.\n";
    }

    public void UpdateAvatarTextOnCreate()
    {
        TapAvatarInfo.text = "<font-weight=600>Please enter a name for your room and indicate how much time you want for the game session.\n";
    }

    public void UpdateAvatarTextOnJoin()
    {
        TapAvatarInfo.text = "<font-weight=600>Please enter the exact name for the room you wish to join.\n";
    }

    public void UpdateAvatarTextOnEdit()
    {
        TapAvatarInfo.text = "<font-weight=600>Welcome!\nPlease set up your profile.\n";
    }
}
