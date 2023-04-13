using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviourPunCallbacks
{
    private GameStateEnum currentGameState;
    private int currentTurn = 1;

    public static CardGameManager instance = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //check players
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        PlayerManager.instance.InitializeNetworkPlayers(PhotonNetwork.CurrentRoom.Players);
        currentTurn = 1; //init with 1st player turn
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
       
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        
    }

    public string GetPlayerNameFromTurn()
    {
        string name = string.Empty;
        foreach (KeyValuePair<int, Photon.Realtime.Player> kvp in (PhotonNetwork.CurrentRoom.Players))
        {
            if(kvp.Value.ActorNumber == currentTurn)
            {
                name = kvp.Value.NickName;
            }
        }
        return name;
    }

    public GameStateEnum GetGameState()
    {
        return currentGameState;
    }

    public int CurrentTurn()
    {
        return currentTurn;
    }

    private void UpdateTurn()
    {
        currentTurn++;
        if(currentTurn> PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currentTurn = 1;
        }
        CardGameManagerUI.instance.UpdatePlayerTurnText();

        //SendRPC here to update turn of player
    }

    public void UpdateGameState(GameStateEnum state)
    {
        currentGameState = state;
        CardGameManagerUI.instance.UpdateCurrentRoundText();
    }

    public void OnConfirmButtonPressed()
    {
        UpdateTurn();
    }

}
