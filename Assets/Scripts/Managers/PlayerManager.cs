using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
 
    public List<Player> players = new List<Player>();
    public Player myPlayer;

    private void Start()
    {
        myPlayer = players[0];
        CardManager.instance.cardInitilaized += InitilizeAllPlayers; 

    }

    public void InitilizeAllPlayers(bool b)
    {
        
        // To do - Get Player ID and Player name from Photon
        // Currently using placeholder name and ID

        for (int i = 0; i < players.Count; i++)
        {

            List<CardSO> cards = new List<CardSO>();

            Debug.Log("Cards before distributing to player:"+(i+1)+",Card count :"+CardManager.instance.GetShuffledCards().Count);

            for (int j = 0; j < 5; j++)
            {
                cards.Add(CardManager.instance.GetShuffledCards()[CardManager.instance.GetShuffledCards().Count-1]);
                CardManager.instance.GetShuffledCards().RemoveAt(CardManager.instance.GetShuffledCards().Count-1);
            }
            // To do - Get Player ID and Player name from Photon
            players[i].Initialize("SamplePlayer",0,cards);

            Debug.Log("Cards after distributing to player:" + (i + 1)+ ",Card count :" + CardManager.instance.GetShuffledCards().Count);

        }
    }

}
