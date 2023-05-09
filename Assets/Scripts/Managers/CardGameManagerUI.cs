using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CardGameManagerUI : MonoBehaviour
{
    public static CardGameManagerUI instance;

    public TMPro.TextMeshProUGUI PlayerTurnText;
    public TMPro.TextMeshProUGUI CurrentRoundText, CurrentObjective, CurrentStageDescription;

    public List<CardUI> clientCardsUI = new List<CardUI>();
    public List<Transform> clientCardsUIParent = new List<Transform>();

    public List<ReorderableList> playerDraggableCards = new List<ReorderableList>();

    public ReorderableList DiscardedDeckScroll;
    public ReorderableList RemainingDeckScroll;

    public GameObject WaitForOtherPlayer;
    public GameObject ItsYourTurn;
    public GameObject SelectFromRemaining;
    public GameObject ConfirmReplace;

    public TMPro.TextMeshProUGUI DiscardedScrollText, RemainingScrollText;

    public GameObject FullCard;

    public GameObject cardRankingAndActions_1;
    public GameObject discardedDeckGameObject, remaingingDeckGameObject;

    public DropdownController dropdownController;

    public GameObject RankingError;
    public Button SendRankButton;

    public GameObject RoundOne, RoundTwo, RoundTwoEnd, RoundThree;
    public GameObject StageThreeItsYourTurn, StageThreeWaitForTurn;
    public GameObject CardsRemaining, AlLCards, DiscardedInVoting, CardWithLabel,Prompt;

    public GameObject VotingCardPrefab;

    public GameObject Stage1HelpWindow;

    public GameObject MoveToNextStage;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DisableAllHelperEmojisOfRoundOne();
        Stage1HelpWindow.SetActive(true);
    }
    public void UpdateDiscardedScrollText(string count)
    {
        DiscardedScrollText.text = "<font-weight=900>DISCARDED CARDS (" + count + ")";
    }

    public void UpdateRemainingScrollText(string count)
    {
        RemainingScrollText.text = "<font-weight=900>REMAINING CARDS (" + count + ")"; 
    }

    public void UpdatePlayerTurnText()
    {
        PlayerTurnText.text = "Current Turn:" + CardGameManager.instance.GetPlayerNameFromTurn();
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_THREE)
        {
            PlayerTurnText.text = "";
        }
    }

    [SerializeField]private bool rankValuesSent = false;
    public void CheckIfPlayerHasRankedCardsAndSendTurnUpdate()
    {
        if (dropdownController.CheckIfAllDropdownsHasSetValue())
        {
            if (!rankValuesSent)
            {
                //send turn update
                PlayerManager.instance.Send_PlayerRankUpdate();
                //show wait for other players
                DisableAllHelperEmojisOfRoundOne();
                WaitForOtherPlayer.SetActive(true);
                rankValuesSent = true;
                SendRankButton.interactable = false;
            }
        }
        else
        {
            //show error dialog to ask to rank all cards
            RankingError.SetActive(true);
        }
    }

    public void DisableAllHelperEmojisOfRoundOne()
    {
        WaitForOtherPlayer.SetActive(false);
        ItsYourTurn.SetActive(false);
        SelectFromRemaining.SetActive(false);
    }

    public void ShowConfirmReplace()
    {
        if (CardGameManager.instance.GetGameState() == GameStateEnum.SETUP || CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_ONE)
        {
            DisableAllHelperEmojisOfRoundOne();
            ConfirmReplace.SetActive(true);
        }
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_TWO)
        {
            DisableAllHelperEmojisOfRoundOne();
        }
    }

    public void ShowSelectFromRemaining()
    {
        if(CardGameManager.instance.GetGameState() == GameStateEnum.SETUP || CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_ONE)
        {
            DisableAllHelperEmojisOfRoundOne();
            SelectFromRemaining.SetActive(true);
        }
    }
    string s;
    private void Update()
    {
        s = PlayerTurnText.text;
        s = s.Replace("Current Turn:", "");
        //my turn.. 
        if (s == PlayerManager.instance.myPlayer.playerName)
        {
            ShowItsYourTurn();
        }
        //not my turn
        else
        {
            ShowWaitForTurn();
        }
    }

    public void ShowItsYourTurn()
    {
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_THREE)
        {
            StageThreeItsYourTurn.SetActive(true);
            StageThreeWaitForTurn.SetActive(false);
        }
        else
        {
            DisableAllHelperEmojisOfRoundOne();
            ItsYourTurn.SetActive(true);
        }

        if (CardManager.instance.GetRemaingCards().Count == 0)
        {
            DisableAllHelperEmojisOfRoundOne();
            ItsYourTurn.SetActive(false);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            MoveToNextStage.SetActive(true);
        }
        else
        {
            MoveToNextStage.SetActive(false);
        }
    }
    public void ShowWaitForTurn()
    {
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_THREE)
        {
            StageThreeItsYourTurn.SetActive(false);
            StageThreeWaitForTurn.SetActive(true);
        }
        else
        {
            DisableAllHelperEmojisOfRoundOne();
            WaitForOtherPlayer.SetActive(true);
        }

        if (CardManager.instance.GetRemaingCards().Count == 0)
        {
            DisableAllHelperEmojisOfRoundOne();
            WaitForOtherPlayer.SetActive(false);
        }
        MoveToNextStage.SetActive(false);
    }

    public List<Transform> VotingCardHolders = new List<Transform>();

    public void UpdateCurrentRoundText()
    {
        GameStateEnum currentGameState = CardGameManager.instance.GetGameState();
        switch (currentGameState)
        {
            case GameStateEnum.SETUP:
                CurrentRoundText.text = "Stage 1";
                break;
            case GameStateEnum.ROUND_ONE:
                CurrentRoundText.text = "Stage 2";//card selection from remaining
                break;
            case GameStateEnum.ROUND_TWO:
                CurrentRoundText.text = "Stage 3";//ranking + select from pile
                cardRankingAndActions_1.SetActive(true);
                remaingingDeckGameObject.SetActive(false); //rankin + select from discarded pile

                List<TMPro.TMP_Dropdown> dropdowns = new List<TMPro.TMP_Dropdown>();
                foreach (CardUI c in PlayerManager.instance.myPlayer.cardsUI)
                {
                    dropdowns.Add(c.rankDropdown);
                }
                dropdownController.InitialiseAllDropdowns(dropdowns);
                SendRankButton.interactable = false;
                PlayerTurnText.text = "";
                PlayerManager.instance.myPlayer.RankingStageStarted = true;

                break;
            case GameStateEnum.ROUND_THREE:
                CurrentRoundText.text = "Stage 4"; // joker(new card) + voting

                //disable discarded or destroy all cards in discarded
                //disable player cards
                //instantiate and show all cards based on ranking. PlayerManager.instance.ReceivedCardsFromAllPlayersAfterRankingCount
                RoundThree.SetActive(true);

                DisableAllHelperEmojisOfRoundOne();
                cardRankingAndActions_1.SetActive(false);
                remaingingDeckGameObject.SetActive(false);
                CardManager.instance.CleanDiscarded();
                discardedDeckGameObject.SetActive(false);
                RoundOne.SetActive(false);
                RoundTwo.SetActive(false);
                RoundTwoEnd.SetActive(false);

                //there will be a separate discarded scrollview for voting stage and separate list
                CardsRemaining.SetActive(true);
                AlLCards.SetActive(true);
                DiscardedInVoting.SetActive(true);

                CardManager.instance.CreateCardsForVotingDiscard();

                DisableAllHelperEmojisOfRoundOne();

                StageThreeItsYourTurn.SetActive(false);
                StageThreeWaitForTurn.SetActive(false);
                break;
            case GameStateEnum.ROUND_FOUR:
                DiscardedInVoting.SetActive(false);
                Prompt.SetActive(false);
                CardWithLabel.SetActive(false);
                CurrentRoundText.text = "Stage 5 & 6";
                break;
        }
    }

    public GameObject selectedSmallVotingCard = null;
    public Transform DiscardScrollPosition;
    public Transform DiscardScrollContent;
    public GameObject Information;

    public void ShowFullCardForDecision(CardSO card, GameObject cardGameObject)
    {
        s = PlayerTurnText.text;
        s = s.Replace("Current Turn:", "");

        if (s == PlayerManager.instance.myPlayer.playerName)
        {
            if (!CardGameManager.instance.lastStage)
            {
                Prompt.SetActive(true);
            }
            if (cardGameObject.transform.parent.name.ToLower().Contains("card")) { selectedSmallVotingCard = cardGameObject; }
        }
        else
        {
            Prompt.SetActive(false);
        }
        CardWithLabel.SetActive(true);
        CardWithLabel.transform.GetChild(0).GetComponent<CardUI>().InitializeFullCard(card);
    }

    public void ShowFullCard(CardSO card)
    {
        FullCard.SetActive(true);
        FullCard.transform.GetChild(0).GetComponent<CardUI>().InitializeFullCard(card);
    }

    public void HideFullCard()
    {
        FullCard.transform.GetChild(0).GetComponent<CardUI>().ClearCard();
        FullCard.SetActive(true);
    }

}
