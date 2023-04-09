using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MascotTalkAnimation : MonoBehaviour
{
    [SerializeField] private float time = 0f;
    
    [SerializeField] private RectTransform obj;
    [SerializeField] private RectTransform mascot;
    [SerializeField] private CanvasGroup speechBubble;
    [SerializeField] private Vector2 startAnchor;
    [SerializeField] private Vector2 finishAnchor;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(EnterTheScene());
        yield return null;
    }

    void Update()
    {
        time = time + Time.deltaTime;
        if(time >= 4f)
        {
            StartCoroutine(Idle());
            time = 0f;
        }
    }
    
    public IEnumerator EnterTheScene()
    {
        obj.anchoredPosition = startAnchor;
        speechBubble.alpha = 0f;
        obj.DOAnchorPos(finishAnchor,0.5f,false);
        yield return new WaitForSeconds(0.6f);
        speechBubble.alpha = 1f;
        yield return null;
    }

    public IEnumerator Idle()
    {
        mascot.DOAnchorPosY(mascot.anchoredPosition.y-30,1.9f,false);
        yield return new WaitForSeconds(2f);
        mascot.DOAnchorPosY(mascot.anchoredPosition.y+30,1.9f,false);
        yield return new WaitForSeconds(2f);
        yield return null;
    }
}
