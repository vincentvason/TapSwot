using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public static CreateAndJoin instance;

    private int totalPlayerCount=4; // Total players to start the game  

    public TMPro.TMP_InputField createInput;
    public TMPro.TMP_InputField joinInput;

    private void Start()
    {
        //if(SelectionManager.instance.GetPlayerSelection()=="Create")
        //{
        //    selectionScreens[0].SetActive(true);
        //    selectionScreens[1].SetActive(false);

        //}
        //else
        //{
        //    selectionScreens[0].SetActive(false);
        //    selectionScreens[1].SetActive(true);
        //}
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
          
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    public void CreateRoom()
    {
        if(createInput!=null)
        {
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { MaxPlayers=4, IsVisible=true, IsOpen=true},TypedLobby.Default,null );
           
            Debug.Log("Multiplayer Lobby succesfully created: " + createInput.text);
        }
        else
        {
            Debug.Log("Multiplayer Lobby name cannot be blank");
        }
        
    }

    public void JoinRoom()
    {
        if (joinInput != null)
        { 
            PhotonNetwork.JoinRoom(joinInput.text);
            Debug.Log("Multiplayer Lobby succesfully joined: " + joinInput.text);
        }
        else
        {
            Debug.Log("Multiplayer Lobby name cannot be blank");
        }

    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
        Debug.Log("Current Players in the Room(Photon)"+PhotonNetwork.CurrentRoom.PlayerCount);
       
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed because {message}");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"player {newPlayer.NickName} entered the room");
        
      
        Debug.Log("Current Players in the Room(Photon)" + PhotonNetwork.CurrentRoom.PlayerCount);
        
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == totalPlayerCount)
        {
            PhotonNetwork.LoadLevel(3);
            
        }
    }
    
}
