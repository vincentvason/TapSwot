using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviourPunCallbacks
{
    private GameStateEnum currentGameState;
    public int currentTurn = 0;

    public static CardGameManager instance = null;

    //public bool RoundOneAllPlayersPlayed = false;
    //public List<string> ROUND_ONE_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn

    //public bool startCountingRoundTwoPLayers = false;
    //public bool RoundTwoAllPlayersPlayed = false;
    //public List<string> ROUND_TWO_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn

    //public bool RoundThreeAllPlayersPlayed = false;
    //public List<string> ROUND_THREE_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn


    public static event Action<GameStateEnum> OnGameStateChanged;

    public void QuitApplication()
    {
        Application.Quit();
    }

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
    private bool cardShuffled = false;
    public void ShowShuffleCardAnimationOnce()
    {
        if (!cardShuffled)
        {
            PlayerManager.instance.ShuffleAnimation.SetActive(true);
            cardShuffled = true;
        }
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

    public void UpdateTurnFirstTime()
    {
        lastTurn = currentTurn;
        currentTurn++;
        if (currentTurn > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currentTurn = 1;
        }
        CardGameManagerUI.instance.UpdatePlayerTurnText(); 
        //0,1
        PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
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
    private bool checkLastTurns = false;
    void UpdateAnimationCard(GameObject animatedCard)
    {
        animatedCard.transform.SetParent(CardGameManagerUI.instance.DiscardScrollContent);
        animatedCard.GetComponent<LayoutElement>().ignoreLayout = false;

        if (lastStage && !didOnce)
        {
            StartRound(); //we will start counting turns now.
            checkLastTurns = true;
            List<CardSO> carsSoLIst = new List<CardSO>();
            for (int i = 0; i < CardGameManagerUI.instance.DiscardScrollContent.childCount; i++)
            {
                CardSO cardSo = CardGameManagerUI.instance.DiscardScrollContent.GetChild(i).GetComponent<CardUI>().card;
                carsSoLIst.Add(cardSo);
                Destroy(CardGameManagerUI.instance.DiscardScrollContent.GetChild(i).gameObject);
            }

            for (int i = 0; i < carsSoLIst.Count; i++)
            {
                GameObject a = GameObject.Instantiate(CardGameManagerUI.instance.VotingCardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                a.SetActive(true);
                a.transform.SetParent(CardGameManagerUI.instance.DiscardScrollContent);

                a.GetComponent<CardUI>().Initialize(carsSoLIst[i]);
                a.GetComponent<CardUI>().DisableBackCard();
                a.GetComponent<CardUI>().ShowRanking();
                a.GetComponent<CardUI>().ShowKeepCardButtton();
            }
            didOnce = true;
        }
    }

    public void KeepCard(string idFromDiscard)
    {
        PlayerManager.instance.SendKeepCardVoting(idFromDiscard);
        UpdateTurn();
    }
    public void DiscardSelectedCardVoting()
    {
        PlayerManager.instance.SendDiscardCardVoting();
    }

    public void KeepCardAnimation(int toReplace, int fromDiscard)
    {
        int originalIndexDiscard = 1;
        int toSetIndexForAnimation = 2;

        //instead of animation. just set parent

        GameObject fromCard = CardGameManagerUI.instance.VotingCardHolders[toReplace - 1].GetChild(0).gameObject;
        GameObject toCard = CardGameManagerUI.instance.DiscardScrollContent.GetChild(fromDiscard).gameObject;
        //fromCard.GetComponent<LayoutElement>().ignoreLayout = false;
        //toCard.GetComponent<LayoutElement>().ignoreLayout = false;

        CardSO fromCardSO = fromCard.GetComponent<CardUI>().card;
        CardSO toCardSo = toCard.GetComponent<CardUI>().card;

        Destroy(fromCard);
        Destroy(toCard);

        {
            GameObject a = GameObject.Instantiate(CardGameManagerUI.instance.VotingCardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            a.SetActive(true);
            a.transform.SetParent(CardGameManagerUI.instance.VotingCardHolders[toReplace - 1].transform);

            a.GetComponent<CardUI>().Initialize(toCardSo);
            a.GetComponent<CardUI>().DisableBackCard();
            a.GetComponent<CardUI>().ShowRanking();
            a.GetComponent<CardUI>().HideKeepCardButton();
        }

        {
            GameObject a = GameObject.Instantiate(CardGameManagerUI.instance.VotingCardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            a.SetActive(true);
            a.transform.SetParent(CardGameManagerUI.instance.DiscardScrollContent);

            a.GetComponent<CardUI>().Initialize(fromCardSO);
            a.GetComponent<CardUI>().DisableBackCard();
            a.GetComponent<CardUI>().ShowRanking();
            a.GetComponent<CardUI>().HideKeepCardButton();
        }

        //if (fromCard != null)
        //{
        //    fromCard.transform.parent = null;
        //}

        //if (toCard != null)
        //{
        //    toCard.transform.parent = null;
        //}

        {  
            //fromCard.SetActive(true);
            //fromCard.transform.SetParent(CardGameManagerUI.instance.DiscardScrollContent);
            //toCard.transform.SetParent(CardGameManagerUI.instance.VotingCardHolders[toReplace - 1].transform);
        }

        {   
            //toCard.SetActive(true);
            //UpdateDiscardAnimationToLeft(toCard, CardGameManagerUI.instance.VotingCardHolders[toReplace - 1]);
        }

        //UpdateTurn();
    }

    private void UpdateDiscardAnimationToLeft(GameObject animatedCard, Transform parent)
    {
        animatedCard.transform.SetParent(parent);
        animatedCard.GetComponent<LayoutElement>().ignoreLayout = false;
        CardGameManagerUI.instance.selectedSmallVotingCard = null;
    }

    public bool lastStage = false;
    public bool didOnce = false;

    public void DiscardSelectedCardVotingAnimation(int id)
    {
        GameObject actualCard = CardGameManagerUI.instance.VotingCardHolders[id - 1].GetChild(0).gameObject;

        if (actualCard != null)
        {
            //instantiatte voting card 
            //set values from selectedSmallVotingCard
            GameObject animatedCard = actualCard;
            animatedCard.SetActive(true);
            
            //also send RPC
            //add cardso information to discarded list
            animatedCard.transform.DOMove(CardGameManagerUI.instance.DiscardScrollPosition.position, 2.5f, false)
            .OnStart(()=>
            animatedCard.GetComponent<LayoutElement>().ignoreLayout = true
            )
            .OnComplete(() =>
                UpdateAnimationCard(animatedCard)                
            ).SetEase(Ease.Flash);
            //set active false prompt 

            //change turn
            CardGameManagerUI.instance.Prompt.SetActive(false);
            //set null// CardGameManagerUI.instance.selectedSmallVotingCard
            CardGameManagerUI.instance.selectedSmallVotingCard = null;
            UpdateTurn();
            CardManager.instance.votingCardsCOunt--;
            CardGameManagerUI.instance.CardsRemaining.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = CardManager.instance.votingCardsCOunt.ToString();
            if (CardManager.instance.votingCardsCOunt < 6)
            {
                //last 5 cards left
                Debug.Log("last 5 cards left");
                if (!lastStage)
                {
                    //enable helpers
                    CardGameManagerUI.instance.Information.SetActive(true);
                    lastStage = true;
                }
            }

        }
    }

    //Turns

    // This is the key we'll use to store the turn count in the room's properties
    private const string TurnCountKey = "TurnCount";

    // Call this when the round starts
    public void StartRound()
    {
        // Reset the turn count
        SetTurnCount(0);
    }

    // Call this when a player takes their turn
    public void PlayerTookTurn()
    {
        int turnCount = GetTurnCount();
        turnCount++;
        SetTurnCount(turnCount);
    }

    // Use this to check if all players have taken their turn
    public bool AllPlayersTookTurn()
    {
        int turnCount = GetTurnCount();
        Debug.Log("AllPlayersTookTurn()" + turnCount);
        return turnCount >= PhotonNetwork.CurrentRoom.PlayerCount;
    }

    // Helper method to get the turn count
    private int GetTurnCount()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(TurnCountKey, out object turnCountObj))
        {
            return (int)turnCountObj;
        }
        else
        {
            return 0;
        }
    }

    // Helper method to set the turn count
    private void SetTurnCount(int turnCount)
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties[TurnCountKey] = turnCount;
        PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
    }

    //


    public int lastTurn = -1;
    private void UpdateTurn()
    {
        //lastTurn = 1;
        //currentTurn = 1;

        //lastTurn = 1;
        //current = 2;


        lastTurn = currentTurn;
        currentTurn++;
        if (currentTurn > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currentTurn = 1;
        }
        CardGameManagerUI.instance.UpdatePlayerTurnText();
        PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());

        //if(currentGameState == GameStateEnum.ROUND_THREE)
        //{
        //    if (lastStage)
        //    {
        //        if (!CardGameManager.instance.ROUND_THREE_PlayersThatHaveTakenTurn.Contains(currentTurn.ToString()))
        //        {
        //            Debug.Log("ROUND_THREE_PlayersThatHaveTakenTurn.Add");
        //            //CardGameManager.instance.ROUND_THREE_PlayersThatHaveTakenTurn.Add(currentTurn.ToString());
        //        }
        //        if (ROUND_THREE_PlayersThatHaveTakenTurn.Count == PlayerManager.instance.GetCurrentPlayersList().Count)
        //        {
        //            RoundThreeAllPlayersPlayed = true;
        //            //This round 2 also has ended. now its timr to hide discarded cards and
        //            //show all players card on table with their ranks
        //            PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_FOUR.ToString());
        //        }
        //    }


        //    PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
        //}
        //else
        //{   //SendRPC here to update turn of player
        //    PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());

        //    if (!CardGameManager.instance.ROUND_ONE_PlayersThatHaveTakenTurn.Contains(currentTurn.ToString()))
        //    {
        //        Debug.Log("ROUND_ONE_PlayersThatHaveTakenTurn.Add");
        //        //CardGameManager.instance.ROUND_ONE_PlayersThatHaveTakenTurn.Add(currentTurn.ToString());
        //    }

        //    if (startCountingRoundTwoPLayers)
        //    {
        //        if (!CardGameManager.instance.ROUND_TWO_PlayersThatHaveTakenTurn.Contains(currentTurn.ToString()))
        //        {
        //            Debug.Log("ROUND_TWO_PlayersThatHaveTakenTurn.Add");
        //            //CardGameManager.instance.ROUND_TWO_PlayersThatHaveTakenTurn.Add(currentTurn.ToString());
        //        }

        //        if (ROUND_TWO_PlayersThatHaveTakenTurn.Count == PlayerManager.instance.GetCurrentPlayersList().Count)
        //        {
        //            RoundTwoAllPlayersPlayed = true;
        //            //This round 2 also has ended. now its timr to hide discarded cards and
        //            //show all players card on table with their ranks
        //            PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_TWO_END.ToString());
        //            PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
        //        }
        //    }
        //}


    }

    private IEnumerator DelayAndCheck()
    {
        yield return new WaitForSeconds(1f);
        if (currentGameState == GameStateEnum.ROUND_TWO)
        {
            if (AllPlayersTookTurn())
            {
                PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_TWO_END.ToString());
                Debug.Log("Turn Count is :" + GetTurnCount());
            }
        }
        yield return new WaitForSeconds(1f);

        if (currentGameState == GameStateEnum.ROUND_TWO_END)
        {
            if (AllPlayersTookTurn())
            {
                PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_THREE.ToString());
                Debug.Log("Turn Count is :" + GetTurnCount());
            }
        }
        yield return new WaitForSeconds(1f);

        if (currentGameState == GameStateEnum.ROUND_THREE)
        {
            if (AllPlayersTookTurn() && checkLastTurns)
            {
                PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_FOUR.ToString());
                Debug.Log("Turn Count is :" + GetTurnCount());
            }
        }
    }

    public void UpdateTurnValueFromRPC(string _lastTurn, string _currentTurn)
    {
        int lastTurn = 0;
        int.TryParse(_currentTurn, out currentTurn);
        int.TryParse(_lastTurn, out lastTurn);
        PlayerTookTurn();
        StartCoroutine(DelayAndCheck());

        //if (currentGameState == GameStateEnum.ROUND_THREE)
        //{
        //    if (lastStage)
        //    {
        //        if (!CardGameManager.instance.ROUND_THREE_PlayersThatHaveTakenTurn.Contains(lastTurn.ToString()))
        //        {
        //            Debug.Log("ROUND_THREE_PlayersThatHaveTakenTurn.Add");
        //            CardGameManager.instance.ROUND_THREE_PlayersThatHaveTakenTurn.Add(lastTurn.ToString());
        //        }
        //    }
        //}
        //else
        //{
        //    if (!CardGameManager.instance.ROUND_ONE_PlayersThatHaveTakenTurn.Contains(lastTurn.ToString()))
        //    {
        //        Debug.Log("ROUND_ONE_PlayersThatHaveTakenTurn.Add");
        //        CardGameManager.instance.ROUND_ONE_PlayersThatHaveTakenTurn.Add(lastTurn.ToString());
        //    }

        //    if (startCountingRoundTwoPLayers)
        //    {
        //        if (!CardGameManager.instance.ROUND_TWO_PlayersThatHaveTakenTurn.Contains(lastTurn.ToString()))
        //        {
        //            Debug.Log("ROUND_TWO_PlayersThatHaveTakenTurn.Add");
        //            CardGameManager.instance.ROUND_TWO_PlayersThatHaveTakenTurn.Add(lastTurn.ToString());
        //        }
        //    }
        //}
    }

    internal void CheckAllPlayersAndUpdateGameStage()
    {
        if (currentGameState == GameStateEnum.ROUND_ONE)
        {
            if (AllPlayersTookTurn())
            {
                PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
                PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_TWO.ToString());
            }
        }

        //Debug.Log("ROUND_ONE_PlayersThatHaveTakenTurn.Count" + ROUND_ONE_PlayersThatHaveTakenTurn.Count);
        //Debug.Log(" PlayerManager.instance.GetCurrentPlayersList().Count" + PlayerManager.instance.GetCurrentPlayersList().Count);
        //if (ROUND_ONE_PlayersThatHaveTakenTurn.Count >= PlayerManager.instance.GetCurrentPlayersList().Count)
        //{
        //    RoundOneAllPlayersPlayed = true;
        //    Debug.Log("RoundOneAllPlayersPlayed");
        //}

        //if (RoundOneAllPlayersPlayed)
        //{
        //    Debug.Log("SendRoundRPC");

        //    //SEND RPC for round update
        //    PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_TWO.ToString());
        //    PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
        //    startCountingRoundTwoPLayers = true;
        //}
    }
}
