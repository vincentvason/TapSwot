using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

//IPointerEnterHandler, IPointerExitHandler, 
public class CardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardSO card = new CardSO();
    public TextMeshProUGUI cardTitle;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardRank;
    public TextMeshProUGUI cardCategory;

    public TMPro.TMP_Dropdown rankDropdown;
    public Transform rankDropdownTransform;

    [HeaderAttribute("For Full Card")]
    public TextMeshProUGUI CardBrief;

    public GameObject BackCard;

    public int lastDropDownValue = 0;

    private Canvas canvas;
    private Transform originalParent;

    public void Initialize(CardSO card)
    {
        this.card = card;
        cardTitle.text = "<font-weight=800>" + card.cardTitle;
        cardDescription.text = card.cardDescription;
        cardRank.text = card.cardRank.ToString();
        cardCategory.text = card.cardCategory;
        if (rankDropdown != null)
        {
            rankDropdown.value = 0;
            DisableRankDropdown();
        }
        if (transform.parent != null)
        {
            if (transform.parent.parent != null)
            {
                CheckParentAndSetBackCard();
            }
        }
        lastDropDownValue = 0;
        // Save the original parent transform
        originalParent = transform.parent;
        
    }
    bool createdCanvas = false;


    private void CreateCanvas()
    {
        if (!createdCanvas)
        {
            // Create a new Canvas object as a child of the root canvas
            GameObject canvasObject = new GameObject("Top Canvas");
            canvasObject.AddComponent<LayoutElement>();
            canvasObject.GetComponent<LayoutElement>().ignoreLayout = true;
            canvasObject.transform.SetParent(transform.parent.parent, false);

            // Add a Canvas component to the new canvas object
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Add a GraphicRaycaster component to the new canvas object
            canvasObject.AddComponent<GraphicRaycaster>();

            // Set the Canvas Sorting Order to a high value so it's rendered on top of other UI elements
            canvas.sortingOrder = 999;
            createdCanvas = true;

            RectTransform sourceRectTransform = transform.parent.GetComponent<RectTransform>();
            RectTransform destinationRectTransform = canvasObject.GetComponent<RectTransform>();

            destinationRectTransform.anchoredPosition = sourceRectTransform.anchoredPosition;
            destinationRectTransform.sizeDelta = sourceRectTransform.sizeDelta;
            destinationRectTransform.anchorMin = sourceRectTransform.anchorMin;
            destinationRectTransform.anchorMax = sourceRectTransform.anchorMax;
        }
    }

    public void CheckParentAndSetBackCard()
    {
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_TWO || CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_ONE ||
            CardGameManager.instance.GetGameState() == GameStateEnum.SETUP)
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
        else
        {
            EnableBackCard();
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
    int myHierarchy = 0;
    private bool isFullCard = false;

    private bool CheckForCardAndCanvasParent()
    {
        bool b = false;
        string name = gameObject.transform.parent.name.ToLower();
        if(name.Contains("card") || name.Contains("top canvas"))
        {
            b = true;
        }
        return b;
    }

    private void Update()
    {
        if (gameObject.transform.parent == null) return;

        if (isFullCard) { rankDropdown.enabled = false; }
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_THREE && !isFullCard && CheckForCardAndCanvasParent())
        {
            if (canvas == null)
            {
                CreateCanvas();
            }

            if (mouse_over)
            {
                transform.SetParent(canvas.transform, false);
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            else
            {
                transform.SetParent(originalParent, false);
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }

        if(CardGameManager.instance.GetGameState() != GameStateEnum.ROUND_THREE)
        {
            if (rankDropdown != null)
            {
                if (gameObject.transform.parent.parent.name == "RemainingCardsScroll" ||
                        gameObject.transform.parent.parent.name == "DiscardedCardsScroll")
                {
                    rankDropdown.enabled = false;
                }
                if (gameObject.transform.parent.parent.name == "Card1" ||
                    gameObject.transform.parent.parent.name == "Card2" ||
                    gameObject.transform.parent.parent.name == "Card3" ||
                    gameObject.transform.parent.parent.name == "Card4" ||
                    gameObject.transform.parent.parent.name == "Card5")
                {
                    if (CardGameManagerUI.instance.cardRankingAndActions_1.activeInHierarchy)
                    {
                        if (rankDropdown.enabled) { return; }
                        else
                        {
                            rankDropdown.enabled = true;
                        }
                    }
                    else
                    {
                        rankDropdown.enabled = false;
                    }
                }
            }
        }
    }


    public void InitializeFullCard(CardSO card)
    {
        isFullCard = true;
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

    public void ShowRanking()
    {
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_THREE)
        {
            rankDropdown.SetValueWithoutNotify(card.cardRank);
            rankDropdown.interactable = false;
        }
    }

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(gameObject.name != "FullCard")
        {
            if(CardGameManager.instance.GetGameState() != GameStateEnum.ROUND_THREE 
                && transform.parent.parent.name =="Card1" ||
                transform.parent.parent.name == "Card2" ||
                transform.parent.parent.name == "Card3" ||
                transform.parent.parent.name == "Card4" ||
                transform.parent.parent.name == "Card5")
            {
                CardGameManagerUI.instance.ShowFullCard(this.card);
            }

            if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_THREE)
            {
                CardGameManagerUI.instance.ShowFullCardForDecision(this.card, this.gameObject);
            }
        }
    }

    
    private bool mouse_over = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_THREE)
        {
            mouse_over = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CardGameManager.instance.GetGameState() == GameStateEnum.ROUND_THREE)
        {
            mouse_over = false;
        }  
    }
    
}
