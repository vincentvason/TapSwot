using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Player : MonoBehaviour
{
    public string playerName;
    public int playerID;
    public List<CardSO> playerCards = new List<CardSO>();

    public TextMeshProUGUI playerNameText;

    public TextMeshProUGUI playerStatusText; // Beta implementation for Player status
    public string playeStatus;  // Beta implementation for Player status

    public void Initialize (string playerName, int playerID, List<CardSO> playercards)
    {
        this.playerName = playerName;
        this.playerID = playerID;
        this.playerCards = playercards;
        playerNameText.text = this.playerName;
    }
}
