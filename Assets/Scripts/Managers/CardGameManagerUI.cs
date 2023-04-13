using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CardGameManagerUI : MonoBehaviour
{
    public static CardGameManagerUI instance;

    public TMPro.TextMeshProUGUI PlayerTurnText;
    public TMPro.TextMeshProUGUI CurrentRoundText, CurrentObjective, CurrentStageDescription;

    public List<CardUI> clientCardsUI = new List<CardUI>();
    public List<ReorderableList> playerDraggableCards = new List<ReorderableList>();

    private void Awake()
    {
        instance = this;
    }

    public void UpdatePlayerTurnText()
    {
        PlayerTurnText.text = "Current Turn:" + CardGameManager.instance.GetPlayerNameFromTurn();
    }

    public void UpdateCurrentRoundText()
    {
        GameStateEnum currentGameState = CardGameManager.instance.GetGameState();
        switch (currentGameState)
        {
            case GameStateEnum.SETUP:
                CurrentRoundText.text = "Stage 1";
                break;
            case GameStateEnum.ROUND_ONE:
                CurrentRoundText.text = "Stage 2";//card selection
                break;
            case GameStateEnum.ROUND_TWO:
                CurrentRoundText.text = "Stage 3";//ranking + select from pile
                break;
            case GameStateEnum.ROUND_TWO_END:
                CurrentRoundText.text = "Stage 3"; //rankin + select from discarded pile + joker(new card)
                break;
            case GameStateEnum.ROUND_THREE:
                CurrentRoundText.text = "Stage 4"; //voting
                break;
            case GameStateEnum.ROUND_FOUR:
                CurrentRoundText.text = "Stage 5 & 6";
                break;
        }
    }


}
