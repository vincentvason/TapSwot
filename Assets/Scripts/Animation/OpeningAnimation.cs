using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpeningAnimation : MonoBehaviour
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
        StartCoroutine(IconsAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if(time >= 4f)
        {
            StartCoroutine(Idle());
            StartCoroutine(IconsAnimation());
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

    IEnumerator IconsAnimation()
    {
        iconS.GetComponent<RectTransform>().anchoredPosition = mascot.anchoredPosition;
        iconW.GetComponent<RectTransform>().anchoredPosition = mascot.anchoredPosition;
        iconO.GetComponent<RectTransform>().anchoredPosition = mascot.anchoredPosition;
        iconT.GetComponent<RectTransform>().anchoredPosition = mascot.anchoredPosition;
        iconS.transform.DOScale(1,0.02f);
        iconW.transform.DOScale(1,0.02f);
        iconO.transform.DOScale(1,0.02f);
        iconT.transform.DOScale(1,0.02f);
        StartCoroutine(IconAnimation(iconS));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(IconAnimation(iconW));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(IconAnimation(iconO));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(IconAnimation(iconT));
        yield return new WaitForSeconds(0.5f);
        yield return null;
    }

    IEnumerator IconAnimation(GameObject go)
    {
        Sequence mySequence = DOTween.Sequence();
        float cx = go.GetComponent<RectTransform>().anchoredPosition.x;
        float cy = go.GetComponent<RectTransform>().anchoredPosition.y;
        mySequence.Append(go.GetComponent<RectTransform>().DOAnchorPos(new Vector2(cx-150,cy+90),0.1f,false));
        mySequence.Append(go.GetComponent<RectTransform>().DOAnchorPos(new Vector2(cx-165,cy+150),0.1f,false));
        mySequence.Append(go.GetComponent<RectTransform>().DOAnchorPos(new Vector2(cx-195,cy+90),0.1f,false));
        mySequence.Append(go.GetComponent<RectTransform>().DOAnchorPos(new Vector2(cx-240,cy+30),0.1f,false));
        mySequence.Append(go.GetComponent<RectTransform>().DOAnchorPos(new Vector2(cx-300,cy+0),0.1f,false));
        yield return new WaitForSeconds(0.5f);
        go.GetComponent<RectTransform>().DOAnchorPos(new Vector2(cx-500,cy+0),1.5f,false);
        yield return new WaitForSeconds(1f);
        go.transform.DOScale(0,0.5f);
        yield return new WaitForSeconds(0.5f);
    }
}
