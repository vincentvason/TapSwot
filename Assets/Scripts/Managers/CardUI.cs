using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

//IPointerEnterHandler, IPointerExitHandler, 
public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public CardSO card = new CardSO();
    public TextMeshProUGUI cardTitle;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardRank;
    public TextMeshProUGUI cardCategory;

    public TMPro.TMP_Dropdown rankDropdown;

    [HeaderAttribute("For Full Card")]
    public TextMeshProUGUI CardBrief;

    public GameObject BackCard;

    public void Initialize(CardSO card)
    {
        this.card = card;
        cardTitle.text = "<font-weight=800>" + card.cardTitle;
        cardDescription.text = card.cardDescription;
        cardRank.text = card.cardRank.ToString();
        cardCategory.text = card.cardCategory;
        if (rankDropdown != null)
        {
            rankDropdown.onValueChanged.AddListener(OnRankChanged);
            rankDropdown.value = card.cardRank;
            DisableRankDropdown();
        }
        if (transform.parent != null)
        {
            if (transform.parent.parent != null)
            {
                CheckParentAndSetBackCard();
            }
        }
    }

    public void CheckParentAndSetBackCard()
    {
        if (transform.parent != null)
        {
            if (transform.parent.parent != null)
            {
                //to-do add round check
                if (transform.parent.parent.name == "DiscardedCardsScroll" || transform.parent.parent.name == "RemainingCardsScroll")
                {
                    EnableBackCard();
                }
                else if (transform.parent.parent.name == "Card1" || transform.parent.parent.name == "Card2" || transform.parent.parent.name == "Card3" || transform.parent.parent.name == "Card4" ||
                    transform.parent.parent.name == "Card5")
                {
                    DisableBackCard();
                }
            }
        }
    }

    public void EnableBackCard()
    {
        BackCard.SetActive(true);
    }

    public void DisableBackCard()
    {
        BackCard.SetActive(false);
    }

    private void Update()
    {
        if (rankDropdown != null)
        {
            if (gameObject.transform.parent.parent.name == "RemainingCardsScroll" ||
                    gameObject.transform.parent.parent.name == "DiscardedCardsScroll")
            {
                rankDropdown.enabled = false;
            }
            else
            {
                if (rankDropdown.enabled) return;                
                if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_TWO ||
                    CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_TWO_END)
                {
                    rankDropdown.enabled = true;
                }
            }
        }
    }
    public void OnRankChanged(int value)
    {
        //send RPC of player, cardID, and rank value
    }

    public void InitializeFullCard(CardSO card)
    {
        this.card = card;
        cardTitle.text = "<font-weight=800>" + card.cardTitle;
        cardDescription.text = card.cardDescription;
        CardBrief.text = card.cardBrief;
        cardRank.text = card.cardRank.ToString();
        cardCategory.text = card.cardCategory;
        if (rankDropdown != null)
        {
            rankDropdown.gameObject.SetActive(true);
            rankDropdown.value = card.cardRank;
        }
    }

    public void ClearCard()
    {
        this.card = null;
        cardTitle.text = "<font-weight=800>";
        cardDescription.text = string.Empty;
        cardRank.text = string.Empty;
        cardCategory.text = string.Empty;
        if (rankDropdown != null)
        {
            rankDropdown.value = 0;
        }
    }

    public void EnableRankDropdown()
    {
        if (rankDropdown != null)
        {
            rankDropdown.gameObject.SetActive(true);
            rankDropdown.enabled = true;
            rankDropdown.value = card.cardRank;
        }
    }

    public void DisableRankDropdown()
    {
        if (rankDropdown != null)
        {
            rankDropdown.enabled = false;
            //rankDropdown.gameObject.SetActive(false);
        }
    }

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(gameObject.name != "FullCard")
        {
            CardGameManagerUI.instance.ShowFullCard(this.card);
        }
    }

    /*
    private bool mouse_over = false;
    void Update()
    {
        if (mouse_over)
        {
            
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        mouse_over = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        Debug.Log("Mouse exit");
    }
    */
}
