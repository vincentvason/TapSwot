using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviourPunCallbacks
{
    private GameStateEnum currentGameState;
    public int currentTurn = 1;

    public static CardGameManager instance = null;

    public bool RoundOneAllPlayersPlayed = false;
    public List<string> ROUND_ONE_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn

    public bool startCountingRoundTwoPLayers = false;
    public bool RoundTwoAllPlayersPlayed = false;
    public List<string> ROUND_TWO_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn


    public static event Action<GameStateEnum> OnGameStateChanged;

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

    public int lastTurn;
    private void UpdateTurn()
    {
        lastTurn = currentTurn;
        currentTurn++;
        if(currentTurn> PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currentTurn = 1;
        }
        CardGameManagerUI.instance.UpdatePlayerTurnText();

        //SendRPC here to update turn of player
        PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
        if (!CardGameManager.instance.ROUND_ONE_PlayersThatHaveTakenTurn.Contains(currentTurn.ToString()))
        {
            CardGameManager.instance.ROUND_ONE_PlayersThatHaveTakenTurn.Add(currentTurn.ToString());
        }

        if (startCountingRoundTwoPLayers)
        {
            if (!CardGameManager.instance.ROUND_TWO_PlayersThatHaveTakenTurn.Contains(currentTurn.ToString()))
            {
                CardGameManager.instance.ROUND_TWO_PlayersThatHaveTakenTurn.Add(currentTurn.ToString());
            }

            if (ROUND_TWO_PlayersThatHaveTakenTurn.Count == PlayerManager.instance.GetCurrentPlayersList().Count)
            {
                RoundTwoAllPlayersPlayed = true;
                //This round 2 also has ended. now its timr to hide discarded cards and
                //show all players card on table with their ranks
                PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_THREE.ToString());
                PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
            }
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
        OnGameStateChanged?.Invoke(currentGameState);
    }

    public void OnConfirmButtonPressed()
    {
        UpdateTurn();
    }

    internal void CheckAllPlayersAndUpdateGameStage()
    {
        Debug.Log("ROUND_ONE_PlayersThatHaveTakenTurn.Count" + ROUND_ONE_PlayersThatHaveTakenTurn.Count);
        Debug.Log(" PlayerManager.instance.GetCurrentPlayersList().Count" + PlayerManager.instance.GetCurrentPlayersList().Count);
        if (ROUND_ONE_PlayersThatHaveTakenTurn.Count >= PlayerManager.instance.GetCurrentPlayersList().Count)
        {
            RoundOneAllPlayersPlayed = true;
            Debug.Log("RoundOneAllPlayersPlayed");
        }
        
        if (RoundOneAllPlayersPlayed)
        {
            Debug.Log("SendRoundRPC");

            //SEND RPC for round update
            PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_TWO.ToString());
            PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
            startCountingRoundTwoPLayers = true;
        }
    }


    public void DiscardSelectedCardVoting()
    {
        if (CardGameManagerUI.instance.selectedSmallVotingCard != null)
        {
            //instantiatte voting card 
            //set values from selectedSmallVotingCard
            GameObject animatedCard = Instantiate(CardGameManagerUI.instance.selectedSmallVotingCard);
            animatedCard.transform.SetParent(CardGameManagerUI.instance.MainCanvas);
            animatedCard.SetActive(true);

            CopyRectTransformSize(CardGameManagerUI.instance.selectedSmallVotingCard.GetComponent<RectTransform>(), animatedCard.GetComponent<RectTransform>());

            CardGameManagerUI.instance.selectedSmallVotingCard.SetActive(false);//maybe destroy?

            //also send RPC
            //add cardso information to discarded list
            animatedCard.transform.DOMove(CardGameManagerUI.instance.DiscardScrollPosition.position, 2.5f, false).OnComplete(() =>
            animatedCard.transform.SetParent(CardGameManagerUI.instance.DiscardScrollContent)
            ).SetEase(Ease.Flash);

            //set active false prompt 

            //change turn
            CardGameManagerUI.instance.Prompt.SetActive(false);
            //set null// CardGameManagerUI.instance.selectedSmallVotingCard
        }
    }

    void CopyRectTransformSize(RectTransform copyFrom, RectTransform copyTo)
    {
        copyTo.anchorMin = copyFrom.anchorMin;
        copyTo.anchorMax = copyFrom.anchorMax;
        copyTo.anchoredPosition = copyFrom.anchoredPosition;
        copyTo.sizeDelta = copyFrom.sizeDelta;
    }
}
