using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform mascot;

    [SerializeField] private float time = 0f;

    [SerializeField] private GameObject iconS;
    [SerializeField] private GameObject iconW;
    [SerializeField] private GameObject iconO;
    [SerializeField] private GameObject iconT;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Idle());
        StartCoroutine(IconAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if(time >= 4f)
        {
            StartCoroutine(Idle());
            StartCoroutine(IconAnimation());
            time = 0f;
        }
    }

    IEnumerator Idle()
    {
        mascot.DOAnchorPosY(mascot.anchoredPosition.y+30,1.9f,false);
        yield return new WaitForSeconds(2f);
        mascot.DOAnchorPosY(mascot.anchoredPosition.y-30,1.9f,false);
        yield return new WaitForSeconds(2f);
    }

    IEnumerator IconAnimation()
    {
        iconS.GetComponent<RectTransform>().anchoredPosition = mascot.anchoredPosition;
        iconW.GetComponent<RectTransform>().anchoredPosition = mascot.anchoredPosition;
        iconO.GetComponent<RectTransform>().anchoredPosition = mascot.anchoredPosition;
        iconT.GetComponent<RectTransform>().anchoredPosition = mascot.anchoredPosition;
        yield return null;
    }
}
