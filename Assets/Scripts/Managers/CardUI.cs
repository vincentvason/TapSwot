using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

//IPointerEnterHandler, IPointerExitHandler, 
public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public CardSO card = new CardSO();
    public TextMeshProUGUI cardTitle;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardRank;
    public TextMeshProUGUI cardCategory;

    [HeaderAttribute("For Full Card")]
    public TextMeshProUGUI CardBrief;

    public void Initialize(CardSO card)
    {
        this.card = card;
        cardTitle.text = "<font-weight=800>" + card.cardTitle;
        cardDescription.text = card.cardDescription;
        cardRank.text = card.cardRank.ToString();
        cardCategory.text = card.cardCategory;        
    }

    public void InitializeFullCard(CardSO card)
    {
        this.card = card;
        cardTitle.text = "<font-weight=800>" + card.cardTitle;
        cardDescription.text = card.cardDescription;
        CardBrief.text = card.cardBrief;
        cardRank.text = card.cardRank.ToString();
        cardCategory.text = card.cardCategory;
    }

    public void ClearCard()
    {
        this.card = null;
        cardTitle.text = "<font-weight=800>";
        cardDescription.text = string.Empty;
        cardRank.text = string.Empty;
        cardCategory.text = string.Empty;
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
