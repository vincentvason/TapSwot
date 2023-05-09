using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviourPunCallbacks
{
    private GameStateEnum currentGameState;
    public int currentTurn = 0;

    public static CardGameManager instance = null;

    public bool RoundOneAllPlayersPlayed = false;
    public List<string> ROUND_ONE_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn

    public bool startCountingRoundTwoPLayers = false;
    public bool RoundTwoAllPlayersPlayed = false;
    public List<string> ROUND_TWO_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn

    public bool RoundThreeAllPlayersPlayed = false;
    public List<string> ROUND_THREE_PlayersThatHaveTakenTurn = new List<string>(); // a list of players who have taken their turn


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

    public int lastTurn = -1;
    private void UpdateTurn()
    {
        lastTurn = currentTurn;
        currentTurn++;
        if(currentTurn> PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currentTurn = 1;
        }
        CardGameManagerUI.instance.UpdatePlayerTurnText();

        if(currentGameState == GameStateEnum.ROUND_THREE)
        {
            if (lastStage)
            {
                if (!CardGameManager.instance.ROUND_THREE_PlayersThatHaveTakenTurn.Contains(currentTurn.ToString()))
                {
                    CardGameManager.instance.ROUND_THREE_PlayersThatHaveTakenTurn.Add(currentTurn.ToString());
                }
                if (ROUND_THREE_PlayersThatHaveTakenTurn.Count == PlayerManager.instance.GetCurrentPlayersList().Count)
                {
                    RoundThreeAllPlayersPlayed = true;
                    //This round 2 also has ended. now its timr to hide discarded cards and
                    //show all players card on table with their ranks
                    PlayerManager.instance.SendRoundRPC(GameStateEnum.ROUND_FOUR.ToString());
                }
            }


            PlayerManager.instance.SendPlayerTurnUpdate(lastTurn.ToString(), currentTurn.ToString());
        }
        else
        {   //SendRPC here to update turn of player
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

    void UpdateAnimationCard(GameObject animatedCard)
    {
        animatedCard.transform.SetParent(CardGameManagerUI.instance.DiscardScrollContent);
        animatedCard.GetComponent<LayoutElement>().ignoreLayout = false;

        if (lastStage && !didOnce)
        {
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
    }
    public void DiscardSelectedCardVoting()
    {
        PlayerManager.instance.SendDiscardCardVoting();
    }

    public void KeepCardAnimation(int toReplace, int fromDiscard)
    {
        int originalIndexDiscard = 1;
        int toSetIndexForAnimation = 2;

        

        GameObject fromCard = CardGameManagerUI.instance.VotingCardHolders[toReplace - 1].GetChild(0).gameObject;
        if (fromCard != null)
        {
            //instantiatte voting card 
            //set values from selectedSmallVotingCard
            GameObject animatedCard = fromCard;
            animatedCard.SetActive(true);

            animatedCard.transform.DOMove(CardGameManagerUI.instance.DiscardScrollPosition.position, 2.2f, false)
            .OnStart(() =>
            animatedCard.GetComponent<LayoutElement>().ignoreLayout = true
            )
            .OnComplete(() =>
                UpdateAnimationCard(animatedCard)//CardGameManagerUI.instance.DiscardScrollContent
            ).SetEase(Ease.Flash);
        }

        

        GameObject toCard = CardGameManagerUI.instance.DiscardScrollContent.GetChild(fromDiscard).gameObject;
        if (toCard != null)
        {
            //instantiatte voting card 
            //set values from selectedSmallVotingCard
            GameObject animatedCard = toCard;
            animatedCard.SetActive(true);

            animatedCard.transform.DOMove(CardGameManagerUI.instance.VotingCardHolders[toReplace - 1].position, 2.5f, false)
            .OnStart(() =>
            animatedCard.GetComponent<LayoutElement>().ignoreLayout = true
            )
            .OnComplete(() =>
                UpdateDiscardAnimationToLeft(animatedCard, CardGameManagerUI.instance.VotingCardHolders[toReplace - 1])
            ).SetEase(Ease.Flash);
        }

        UpdateTurn();
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

}
