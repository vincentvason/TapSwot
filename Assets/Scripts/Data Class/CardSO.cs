using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Card", menuName ="Add New Card")]
public class CardSO : ScriptableObject
{
    public int cardId;
    public string cardTitle;
    public string cardDescription;
    public string cardBrief;
    public string cardCategory;
    public int cardRank;

}
