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

    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public GameObject CreateJoinCanvas, LobbyCanvas;

    #region START AND AWAKE
    private void Start()
    {
        // To initialize the Create and Join canvas at the start of the scene
        CreateJoinCanvas.SetActive(true);  
        LobbyCanvas.SetActive(false);
    }

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
          
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }

    //    DontDestroyOnLoad(gameObject);

    //}
    # endregion 

    #region CREATE AND JOIN ROOM PUN CALLBACKS 

    // Create room function with Max players currently set to 4
    public void CreateRoom()
    {
        if(createInput.text.Length>=1)
        {
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { MaxPlayers=4, IsVisible=true, IsOpen=true},TypedLobby.Default,null );
           
            Debug.Log("Multiplayer Lobby succesfully created: " + createInput.text);
        }
        else
        {
            Debug.Log("Multiplayer Lobby name cannot be blank");
        }
        
    }

    // Join Room function to join a room that already exists
    public void JoinRoom()
    {
        if (joinInput.text.Length >= 1)
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
        CreateJoinCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);

        Debug.Log("Current Players in the Room(Photon)"+PhotonNetwork.CurrentRoom.PlayerCount);

        CleanList();

        if (PhotonNetwork.CurrentRoom.Players.Count > 0)
        {
            foreach(KeyValuePair<int, Photon.Realtime.Player> kvp in (PhotonNetwork.CurrentRoom.Players))
            {
                GameObject a = PhotonNetwork.Instantiate("PlayerPlane1", new Vector3(0, 0, 0), Quaternion.identity);
                a.transform.SetParent(_content);
                PlayerInfo listing = a.GetComponent<PlayerInfo>();

                listing.SetPlayerInfo(kvp.Value);
                _listing.Add(listing);
            }
        }
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed because {message}");
    }

    #endregion

    #region PLAYER JOIN AND LEFT CALLBACKS

    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerInfo playerListPrefab;

    private List<PlayerInfo> _listing = new List<PlayerInfo>();

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"player {newPlayer.NickName} entered the room");        
        Debug.Log("Current Players in the Room(Photon)" + PhotonNetwork.CurrentRoom.PlayerCount);
        
        CleanList();

        // Reinstantiate the Player list prefab whenever a player joins
        if (PhotonNetwork.CurrentRoom.Players.Count > 0)
        {
            foreach (KeyValuePair<int, Photon.Realtime.Player> kvp in (PhotonNetwork.CurrentRoom.Players))
            {
                GameObject a = PhotonNetwork.Instantiate("PlayerPlane1", new Vector3(0, 0, 0), Quaternion.identity);
                a.transform.SetParent(_content);
                PlayerInfo listing = a.GetComponent<PlayerInfo>();

                listing.SetPlayerInfo(kvp.Value);
                _listing.Add(listing);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // To destroy the Player prefab from the player list in the lobby
        int index = _listing.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listing[index].gameObject);
            _listing.RemoveAt(index);
        }
    }

    // Clean the player list and its child
    private void CleanList()
    {
        _listing.Clear();
        if (_content.transform.childCount > 0)
        {
            for(int i=0;i< _content.transform.childCount; i++)
            {
                Destroy(_content.transform.GetChild(i).gameObject);
            }
        }
    }
    #endregion

    #region ON SELECT - START BUTTON - RPC TO START LOAD MAIN GAME 

    public void OnSelectStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //GetComponent<PhotonView>().RPC("LoadLevel", RpcTarget.AllBuffered);
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 1) // To check the count of players in the room is 4 including the host
            {
                { 
                    GetComponent<PhotonView>().RPC("LoadLevel", RpcTarget.AllBuffered);
                }
            }
            else
            {
                // Change debug to UI message 
                Debug.Log("Insufficient Players in the Room, Current Players : " + PhotonNetwork.CurrentRoom.PlayerCount);
            }
        }
    }

    [PunRPC]
    void LoadLevel()
    {
        if (SceneManager.GetActiveScene().name != "MainGame2")
        {
            PhotonNetwork.LoadLevel("MainGame2");
        }
    }

}

#endregion