using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public static CreateAndJoin instance;
    public SceneTransition sceneTransition;

    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public MainMenuAnimation mainMenu;
     

    public Lobby lobby;

    public GameObject playerLobbyPrefab;
    public GameObject LobbyWaitingWindow;
    public GameObject StartGameBtn;

    [SerializeField]
    private Transform _content;

    public Color[] playerColor;



    #region START AND AWAKE
    private void Start()
    {
        // To initialize the Create and Join canvas at the start of the scene
        // CreateJoinCanvas.SetActive(true);
        StartCoroutine(mainMenu.OpenCreateJoinWindow());
        // LobbyCanvas.SetActive(false);
        StartCoroutine(mainMenu.CloseLobbyWaitingWindow());

        //Enable Start Button only for the Host of the room
     
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
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 4,
                IsVisible = true,
                IsOpen = true

            };

            Hashtable roomCustomProps = new Hashtable();
            roomCustomProps.Add("TIME_LIMIT", lobby.timeLimit.text.ToString());

            roomOptions.CustomRoomProperties = roomCustomProps;
            

            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
           
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
        // CreateJoinCanvas.SetActive(false);
        StartCoroutine(mainMenu.CloseCreateJoinWindow());
        // LobbyCanvas.SetActive(true);
        StartCoroutine(mainMenu.OpenLobbyWaitingWindow());
        lobby.RoomCreate();  // Lobby class method to display to room details using photon callbacks

        Debug.Log("Current Players in the Room(Photon)"+PhotonNetwork.CurrentRoom.PlayerCount);

        Photon.Realtime.Player[] _playerList = PhotonNetwork.PlayerList;
        for (int i=0; i< _playerList.Count(); i++)
        {
            Instantiate(playerLobbyPrefab, _content).GetComponent<PlayerInfo>().SetPlayerInfo(_playerList[i]);

            Debug.Log("Player Prefab instantiated for  Player :" + _playerList[i].NickName);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartGameBtn.SetActive(true);
        }
        else
        {
            StartGameBtn.SetActive(false);
        }



        //CleanList();

        //if (PhotonNetwork.CurrentRoom.Players.Count > 0)
        //{
        //    foreach(KeyValuePair<int, Photon.Realtime.Player> kvp in (PhotonNetwork.CurrentRoom.Players))
        //    {
        //        GameObject a = PhotonNetwork.Instantiate("PlayerLobby", new Vector3(0, 0, 0), Quaternion.identity);
        //        a.transform.SetParent(_content);
        //        PlayerInfo listing = a.GetComponent<PlayerInfo>();

        //        listing.SetPlayerInfo(kvp.Value);
        //        _listing.Add(listing);

        //        a.transform.localScale = new Vector3(1f, 1f, 1f);
        //        a.GetComponent<Image>().color = playerColor[_listing.Count - 1];
        //    }

        //for(int emptySeat = 0; emptySeat < 4; emptySeat++)
        //{
        //    GameObject b = Instantiate(emptySeatPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //    b.transform.SetParent(_content);
        //    b.transform.localScale = new Vector3(1f, 1f, 1f);
        //}



    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed because {message}");
    }

    #endregion

    #region PLAYER JOIN AND LEFT CALLBACKS


    //private List<PlayerInfo> _listing = new List<PlayerInfo>();

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"player {newPlayer.NickName} entered the room");        
        Debug.Log("Current Players in the Room(Photon)" + PhotonNetwork.CurrentRoom.PlayerCount);

        Instantiate(playerLobbyPrefab, _content).GetComponent<PlayerInfo>().SetPlayerInfo(newPlayer);

        //CleanList();

        //// Reinstantiate the Player list prefab whenever a player joins
        //if (PhotonNetwork.CurrentRoom.Players.Count > 0)
        //{
        //    foreach (KeyValuePair<int, Photon.Realtime.Player> kvp in (PhotonNetwork.CurrentRoom.Players))
        //    {
        //        GameObject a = PhotonNetwork.Instantiate("PlayerLobby", new Vector3(0, 0, 0), Quaternion.identity);
        //        a.transform.localScale = new Vector3(1f, 1f, 1f);
        //        a.transform.SetParent(_content);
        //        PlayerInfo listing = a.GetComponent<PlayerInfo>();

        //        listing.SetPlayerInfo(kvp.Value);
        //        _listing.Add(listing);
        //    }

        //    for(int emptySeat = 0; emptySeat < 4; emptySeat++)
        //    {
        //        GameObject b = Instantiate(emptySeatPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //        b.transform.localScale = new Vector3(1f, 1f, 1f);
        //        b.transform.SetParent(_content);
        //    }
        //}
        if (PhotonNetwork.IsMasterClient)
        {
            StartGameBtn.SetActive(true);
        }
        else
        {
            StartGameBtn.SetActive(false);
        }

    }

    //public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    //{
    //    // To destroy the Player prefab from the player list in the lobby
    //    int index = _listing.FindIndex(x => x.Player == otherPlayer);
    //    if (index != -1)
    //    {
    //        Destroy(_listing[index].gameObject);
    //        _listing.RemoveAt(index);
    //    }
    //}

    // Clean the player list and its child
    //private void CleanList()
    //{
    //    _listing.Clear();
    //    if (_content.transform.childCount > 0)
    //    {
    //        for(int i=0;i< _content.transform.childCount; i++)
    //        {
    //            Destroy(_content.transform.GetChild(i).gameObject);
    //        }
    //    }
    //}
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
    IEnumerator LoadLevel()
    {
        if (SceneManager.GetActiveScene().name != "MainGame2")
        {
            LobbyWaitingWindow.SetActive(false);
            yield return StartCoroutine(sceneTransition.SceneTransitionBegin());
            PhotonNetwork.LoadLevel("MainGame2");
        }
    }

}

#endregion