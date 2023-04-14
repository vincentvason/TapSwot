using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviourPunCallbacks
{
    private GameStateEnum currentGameState;
    private int currentTurn = 1;

    public static CardGameManager instance = null;

    public bool Stage1AllPlayersPlayed = false;
    public List<string> Stage1_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn

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
        int lastTurn = currentTurn;
        currentTurn++;
        if(currentTurn> PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currentTurn = 1;
        }
        CardGameManagerUI.instance.UpdatePlayerTurnText();

        //SendRPC here to update turn of player
        PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
        if (!CardGameManager.instance.Stage1_PlayersThatHaveTakenTurn.Contains(currentTurn.ToString()))
        {
            CardGameManager.instance.Stage1_PlayersThatHaveTakenTurn.Add(currentTurn.ToString());
        }
    }

    public void UpdateTurnValueFromRPC(string _currentTurn)
    {
        int.TryParse(_currentTurn, out currentTurn);
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

    internal void CheckAllPlayersAndUpdateGameStage()
    {
        if(Stage1_PlayersThatHaveTakenTurn.Count == PlayerManager.instance.GetCurrentPlayersList().Count)
        {
            Stage1AllPlayersPlayed = true;
        }
        
        if (Stage1AllPlayersPlayed)
        {
            CardGameManager.instance.UpdateGameState(GameStateEnum.ROUND_TWO);
            PlayerManager.instance.SendPlayerTurnUpdate("0","0"); //reset turn

            //to-do. ask player to do card ranking and select once from discared card or create a new joker card if they want. do this in the above called function.
        }
    }
}
