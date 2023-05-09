using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherTurnSelfCheck : MonoBehaviour
{
    private bool isFirstTimeSHowingWaitForTurn = false;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI desc;

    private void OnEnable()
    {
        SetText();
    }

    private void SetText()
    {
        if (!isFirstTimeSHowingWaitForTurn)
        {
            infoText.text = "<font-weight=900>This stage of the game is now started, please wait until it is your turn";
            desc.text = "";

            isFirstTimeSHowingWaitForTurn = true;
        }
        else
        {
            infoText.text = "<font-weight=900>Wait for other player...";
            desc.text = "<font-weight=700>You can also review any card on your hand or in the discard pile.";
        }
    }
}
