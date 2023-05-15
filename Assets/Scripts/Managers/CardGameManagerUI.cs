using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    [Header("New Card Dialog")]
    public GameObject NewCardHelper;
    public GameObject NewCardSkipButton;
    public GameObject NewCardDialog;

    public GameObject NewCardParent;
    public GameObject[] NewCardSlotButtons;

    public TMPro.TMP_Dropdown SWOTDropdown;
    public TMPro.TMP_InputField newCardTitle, newCardSubTitle, newCardDescription;

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
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_TWO_END)
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
                PlayerManager.instance.myPlayer.DisableAllCardsDropdownAfterRankingFinalised();
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
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_TWO_END)
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
            PlayerManager.instance.myPlayer.EnableKeepCardButton();
        }
        else
        {
            DisableAllHelperEmojisOfRoundOne();
            ItsYourTurn.SetActive(true);
        }

        if (CardManager.instance.GetRemaingCards().Count == 0 && rankingRound)
        {
            DisableAllHelperEmojisOfRoundOne();
            ItsYourTurn.SetActive(false);
        }

        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_TWO)
        {
            NewCardParent.SetActive(true);
            NewCardSlotButtons[0].SetActive(true);
            NewCardSlotButtons[1].SetActive(true);
            NewCardSlotButtons[2].SetActive(true);
            NewCardSlotButtons[3].SetActive(true);
            NewCardSlotButtons[4].SetActive(true);
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
            PlayerManager.instance.myPlayer.DisableKeepCardButton();
        }
        else
        {
            DisableAllHelperEmojisOfRoundOne();
            WaitForOtherPlayer.SetActive(true);
        }

        if (CardManager.instance.GetRemaingCards().Count == 0 && rankingRound)
        {
            DisableAllHelperEmojisOfRoundOne();
            WaitForOtherPlayer.SetActive(false);
        }

        if(CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_TWO)
        {
            NewCardParent.SetActive(false);
            NewCardSlotButtons[0].SetActive(false);
            NewCardSlotButtons[1].SetActive(false);
            NewCardSlotButtons[2].SetActive(false);
            NewCardSlotButtons[3].SetActive(false);
            NewCardSlotButtons[4].SetActive(false);
        }

        MoveToNextStage.SetActive(false);
    }

    public List<Transform> VotingCardHolders = new List<Transform>();

    public void SkipNewCard()
    {
        PlayerManager.instance.SendSkipNewCard();
        CardGameManager.instance.OnConfirmButtonPressed();//sending turn

        NewCardDialog.SetActive(false);
        NewCardSkipButton.SetActive(false);
        NewCardParent.SetActive(false);
        NewCardHelper.SetActive(false);
    }
    public void SaveNewCardData()
    {
        int value = SWOTDropdown.value;
        string cardTitle = newCardTitle.text;
        string cardSubTitle = newCardSubTitle.text;
        string cardDesc = newCardDescription.text;
        string cardSlotToReplace = newCardSlotOpened.ToString();

        PlayerManager.instance.SendNewCardData(value.ToString(), cardTitle, cardSubTitle, cardDesc, cardSlotToReplace);
        CardGameManager.instance.OnConfirmButtonPressed();//sending turn

        NewCardDialog.SetActive(false);
        NewCardSkipButton.SetActive(false);
        NewCardParent.SetActive(false);
        NewCardHelper.SetActive(false);
    }

    private int newCardSlotOpened;
    public void OpenNewCardDilog(int slotOpened) 
    {
        newCardSlotOpened = slotOpened;
        NewCardDialog.SetActive(true);

        NewCardParent.SetActive(false);
        NewCardSlotButtons[0].SetActive(false);
        NewCardSlotButtons[1].SetActive(false);
        NewCardSlotButtons[2].SetActive(false);
        NewCardSlotButtons[3].SetActive(false);
        NewCardSlotButtons[4].SetActive(false);
    }
    private bool rankingRound = false;
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
                NewCardHelper.SetActive(true);
                NewCardSkipButton.SetActive(true);
                NewCardDialog.SetActive(false);
                //disable drag on all cards (discarded, card slots, remaining)

                PlayerManager.instance.myPlayer.Ex_DisableDragOnAllCardSlots();
                CardGameManagerUI.instance.RemainingDeckScroll.IsDraggable = false;
                CardGameManagerUI.instance.DiscardedDeckScroll.IsDraggable = false;

                NewCardParent.SetActive(true);
                NewCardSlotButtons[0].SetActive(true);
                NewCardSlotButtons[1].SetActive(true);
                NewCardSlotButtons[2].SetActive(true);
                NewCardSlotButtons[3].SetActive(true);
                NewCardSlotButtons[4].SetActive(true);
                remaingingDeckGameObject.SetActive(false);
                SendRankButton.interactable = false;
                break;
            case GameStateEnum.ROUND_TWO_END:
                PlayerManager.instance.myPlayer.RankingStageStarted = true;
                NewCardHelper.SetActive(false);
                NewCardSkipButton.SetActive(false);
                NewCardDialog.SetActive(false);
                CurrentRoundText.text = "Stage 3";//ranking + select from pile
                cardRankingAndActions_1.SetActive(true);
                remaingingDeckGameObject.SetActive(false); //rankin + select from discarded pile
                rankingRound = true;
                List<TMPro.TMP_Dropdown> dropdowns = new List<TMPro.TMP_Dropdown>();
                foreach (CardUI c in PlayerManager.instance.myPlayer.cardsUI)
                {
                    dropdowns.Add(c.rankDropdown);
                }
                dropdownController.InitialiseAllDropdowns(dropdowns);
                SendRankButton.interactable = false;
                PlayerTurnText.text = "";
                NewCardParent.SetActive(false);
                NewCardSlotButtons[0].SetActive(false);
                NewCardSlotButtons[1].SetActive(false);
                NewCardSlotButtons[2].SetActive(false);
                NewCardSlotButtons[3].SetActive(false);
                NewCardSlotButtons[4].SetActive(false);
                break;
            case GameStateEnum.ROUND_THREE:
                rankingRound = false;
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

                StageThreeItsYourTurn.SetActive(false);
                StageThreeWaitForTurn.SetActive(false);
                DisableAllHelperEmojisOfRoundOne();
                ItsYourTurn.SetActive(false);
                RoundThree.SetActive(false);
                CardsRemaining.SetActive(false);
                AlLCards.SetActive(false);
                DiscardedInVoting.SetActive(false);

                ShowLastFiveCards();

                break;
        }
    }
    public GameObject FinalCardPanel;
    public GameObject FinalCardPrefab;
    public Transform FinalCardContentTransform;

    public GameObject TakeScreenshotButton, OpenFolderButton;
    private bool showLastFiveCardsCalled = false;
    private void ShowLastFiveCards()
    {
        if (showLastFiveCardsCalled) return;
        FinalCardPanel.SetActive(true);
        List<CardSO> cardSOs = new List<CardSO>();
        for(int i = 0; i < VotingCardHolders.Count; i++)
        {
            if(VotingCardHolders[i].childCount > 0)
            {
                cardSOs.Add(VotingCardHolders[i].GetChild(0).GetComponent<CardUI>().card);
            }
        }

        foreach(CardSO so in cardSOs)
        {
            GameObject a = GameObject.Instantiate(FinalCardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            a.SetActive(true);
            a.transform.SetParent(FinalCardContentTransform);

            a.transform.GetChild(0).GetComponent<CardUI>().InitializeFullCard(so);
            a.transform.GetChild(0).GetComponent<CardUI>().DisableBackCard();
            a.transform.GetChild(0).GetComponent<CardUI>().ShowRanking();
            a.transform.GetChild(0).GetComponent<CardUI>().HideKeepCardButton();
            a.transform.GetChild(0).GetComponent<CardUI>().SetNotInteractable();
        }

        AlLCards.SetActive(false);
        showLastFiveCardsCalled = true;
    }

    public RectTransform targetRect;
    public Camera _camera;

    public void TakeScreenshot()
    {
        string guid = Guid.NewGuid().ToString();
        string fileName = "TAP_SWOT_FinalCards_" + guid;
        fileName = fileName.Replace("-", "_");
        StartCoroutine(CutSpriteFromScreen(fileName));
    }
    //Object To Screenshot
    [SerializeField] private RectTransform _objToScreenshot;
    private IEnumerator CutSpriteFromScreen(string fileName)
    {
        fileName = new string(fileName.ToCharArray()
        .Where(c => !Char.IsWhiteSpace(c))
        .ToArray());

        //Code will throw error at runtime if this is removed
        yield return new WaitForEndOfFrame();
        //Get the corners of RectTransform rect and store it in a array vector
        Vector3[] corners = new Vector3[4];
        _objToScreenshot.GetWorldCorners(corners);
        //Remove 100 and you will get error
        int width = ((int)corners[3].x - (int)corners[0].x) - 100;
        int height = (int)corners[1].y - (int)corners[0].y;
        var startX = corners[0].x;
        var startY = corners[0].y;
        //Make a temporary texture and read pixels from it
        Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        ss.Apply();
        Debug.Log("Start X : " + startX + " Start Y : " + startY);
        Debug.Log("Screen Width : " + Screen.width + " Screen Height : " + Screen.height);
        Debug.Log("Texture Width : " + width + " Texture Height : " + height);
        //Save the screenshot to disk
        byte[] byteArray = ss.EncodeToPNG();
        string savePath = Application.streamingAssetsPath + "/" + fileName + ".png";
        System.IO.File.WriteAllBytes(savePath, byteArray);
        Debug.Log("Screenshot Path : " + savePath);
        // Destroy texture to avoid memory leaks
        Destroy(ss);
    }

    public void OpenFolder()
    {
        Application.OpenURL("file://" + Application.streamingAssetsPath);
    }

    public GameObject selectedSmallVotingCard = null;
    public Transform DiscardScrollPosition;
    public Transform DiscardScrollContent;
    public GameObject Information;

    public void SetSelectedSmallVotingCard(GameObject cardGameObject)
    {
        selectedSmallVotingCard = cardGameObject;
    } 
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
            //if (cardGameObject.transform.parent.name.ToLower().Contains("card")) { selectedSmallVotingCard = cardGameObject; }
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
