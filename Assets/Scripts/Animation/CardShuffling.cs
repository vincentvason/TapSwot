using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardShuffling : MonoBehaviour
{
    [SerializeField] private GameObject[] cardStack;
    [SerializeField] private GameObject[] cardShuffle;
    [SerializeField] private GameObject[] cardPlayer;
    [SerializeField] private GameObject cardDraw;
    [SerializeField] private GameObject pileLeft;

    [SerializeField] private GameObject[] clientCard;


    /**
    * startPlayer - Player no. who get a card first.
    * 1 - you (1 -> 2 -> 3 -> 4)
    * 2 - teammate (on left) (2 -> 3 -> 4 -> 1)
    * 3 - teammate (on middle) (3 -> 4 -> 1 -> 2)
    * 4 - teammate (on right) (4 -> 1 -> 2 -> 3)
    */

    /**
    * Card Position (at Test)
    * player1@pos1 -505, -399
    * player1@pos2 -219, -399
    * player1@pos3 69, -399
    * player1@pos4 358, -399
    * player1@pos5 645, -399
    * player2 -365, 450
    * player3 0, 450
    * player4 365, 450
    * drawpile 228, -226
    */

    [SerializeField] private int startPlayer;
    [SerializeField] private Vector2[] cardPlayer1Position;
    [SerializeField] private Vector2 cardPlayer2Position;
    [SerializeField] private Vector2 cardPlayer3Position;
    [SerializeField] private Vector2 cardPlayer4Position;
    [SerializeField] private Vector2 drawPilePosition;

    [HideInInspector] private int cardLeft;
    [HideInInspector] private float drawDuration = 0.2f;

    [SerializeField] private RectTransform[] cardPlayer1Rect;

    public void StartShuffleAnimation()
    {
        //for (int x = 0; x < cardPlayer1Rect.Length; x++)
        //{
        //    cardPlayer1Position[x] = new Vector2(cardPlayer1Rect[x].rect.x, cardPlayer1Rect[x].rect.y);
        //}
        StartCoroutine(DrawCard());
    }

    IEnumerator DrawCard()
    {
        //Hide Deck
        cardLeft = cardStack.Length;
        for(int i = 0; i < cardStack.Length; i++)
        {
            cardStack[i].GetComponent<Image>().color = new Color32(255,255,255,0);
        }

        for(int i = 0; i < cardShuffle.Length; i++)
        {
            cardShuffle[i].GetComponent<Image>().color = new Color32(255,255,255,0);
        }

        cardDraw.GetComponent<Image>().color = new Color32(255,255,255,0);
        

        //Show Deck Animation
        for(int i = 0; i < cardStack.Length; i++)
        {
            cardStack[i].GetComponent<Image>().color = new Color32(255,255,255,255);
            yield return new WaitForSeconds(0.08f);
        }

        for(int i = 0; i < cardShuffle.Length; i++)
        {
            cardShuffle[i].GetComponent<Image>().color = new Color32(255,255,255,255);
        }

        
        //Play Shuffing Animation
        Sequence mySequence1 = DOTween.Sequence();
        Sequence mySequence2 = DOTween.Sequence();
        for(int i = 0; i < 4; i++)
        {
            mySequence1.Append(cardShuffle[0].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-400f,15f),0.075f,false));
            mySequence1.Append(cardShuffle[0].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f,10f),0.075f,false));
            mySequence2.Append(cardShuffle[1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(400f,5f),0.075f,false));
            mySequence2.Append(cardShuffle[1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f,0f),0.075f,false));
            mySequence1.Append(cardShuffle[2].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-400f,-5f),0.075f,false));
            mySequence1.Append(cardShuffle[2].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f,-10f),0.075f,false));
            mySequence2.Append(cardShuffle[3].GetComponent<RectTransform>().DOAnchorPos(new Vector2(400f,-15f),0.075f,false));
            mySequence2.Append(cardShuffle[3].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f,-20f),0.075f,false));
        }
        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < cardShuffle.Length; i++)
        {
            cardShuffle[i].GetComponent<Image>().color = new Color32(255,255,255,0);
        }
        cardDraw.GetComponent<Image>().color = new Color32(255,255,255,255);
        
        //Play a card
        StartCoroutine(DistributeCard(startPlayer));
        for(cardLeft = cardStack.Length-1; cardLeft >= cardStack.Length/2; cardLeft--)
        {
            cardStack[cardLeft].SetActive(false);
            yield return new WaitForSeconds(0.8f);
        }

        pileLeft.GetComponent<RectTransform>().DOAnchorPos(drawPilePosition,drawDuration*2,false);
        pileLeft.transform.DOScale(0f, drawDuration*2);
        yield return new WaitForSeconds(drawDuration*2);
        
        yield return null;
    }

    IEnumerator DistributeCard(int player)
    {
        for(int i = 0; i < 20; i++)
        {
            if((i+player)%4 == 1) //Player 1
            {
                cardDraw.GetComponent<RectTransform>().DOAnchorPos(cardPlayer1Position[i/4],drawDuration,false);
                yield return new WaitForSeconds(drawDuration);
                cardPlayer[i/4].GetComponent<RectTransform>().anchoredPosition = cardPlayer1Position[i/4];
                cardDraw.GetComponent<RectTransform>().anchoredPosition = cardStack[cardLeft-1].GetComponent<RectTransform>().anchoredPosition;
                cardDraw.transform.localScale = new Vector3(1,1,1);
            }
            else //Player 2
            {
                if((i+player)%4 == 2)
                {
                    cardDraw.GetComponent<RectTransform>().DOAnchorPos(cardPlayer2Position,drawDuration,false);
                }
                else if((i+player)%4 == 3)
                {
                    cardDraw.GetComponent<RectTransform>().DOAnchorPos(cardPlayer3Position,drawDuration,false);
                }
                else //Player 4
                {
                    cardDraw.GetComponent<RectTransform>().DOAnchorPos(cardPlayer4Position,drawDuration,false);
                }
                    
                cardDraw.transform.DOScale(0f, drawDuration);
                yield return new WaitForSeconds(drawDuration);
                if(cardLeft> cardStack.Length) { Debug.Log("some issue cardLeft" + cardLeft); }
                cardDraw.GetComponent<RectTransform>().anchoredPosition = cardStack[cardLeft].GetComponent<RectTransform>().anchoredPosition;
                cardDraw.transform.localScale = new Vector3(1,1,1);
            }

            if(i == 19) //last card draw
            {
                cardDraw.SetActive(false);
            }
        }
        yield return new WaitForSeconds(6f);

        for (int x = 0; x < clientCard.Length; x++)
        {
            clientCard[x].SetActive(true);
        }
        gameObject.SetActive(false);
        yield return null;
    }

}