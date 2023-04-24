using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private RectTransform bg;
    [SerializeField] private RectTransform mascot;
    [SerializeField] private Image iconS;
    [SerializeField] private Image iconW;
    [SerializeField] private Image iconO;
    [SerializeField] private Image iconT;

    void Start()
    {
 
    }

    public IEnumerator SceneTransitionBegin()
    {
        bg.anchoredPosition = new Vector2(1920f,0f);
        mascot.anchoredPosition = new Vector2(1120f,0f);
        iconT.color = new Color32(255,255,255,0);
        iconT.color = new Color32(255,255,255,0);
        iconT.color = new Color32(255,255,255,0);
        iconT.color = new Color32(255,255,255,0);

        bg.DOAnchorPosX(0f,0.5f,false);
        yield return new WaitForSeconds(0.5f);
        mascot.DOAnchorPosX(-1120f,2f,false);
        yield return new WaitForSeconds(0.4f);
        iconT.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(0.1f);
        iconO.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(0.15f);
        iconW.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(0.2f);
        iconS.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(1.15f);
        yield return null;
    }
    
    public IEnumerator SceneTransitionBegin(int scene)
    {
        bg.anchoredPosition = new Vector2(1920f,0f);
        mascot.anchoredPosition = new Vector2(1120f,0f);
        iconT.color = new Color32(255,255,255,0);
        iconT.color = new Color32(255,255,255,0);
        iconT.color = new Color32(255,255,255,0);
        iconT.color = new Color32(255,255,255,0);

        bg.DOAnchorPosX(0f,0.5f,false);
        yield return new WaitForSeconds(0.5f);
        mascot.DOAnchorPosX(-1120f,2f,false);
        yield return new WaitForSeconds(0.4f);
        iconT.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(0.1f);
        iconO.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(0.15f);
        iconW.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(0.2f);
        iconS.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(1.15f);
        SceneManager.LoadScene(scene);
        yield return null;
    }

    public IEnumerator SceneTransitionEnd()
    {
        DOTween.KillAll();
        bg.anchoredPosition = new Vector2(0,0f);
        mascot.anchoredPosition = new Vector2(-1120f,0f);
        iconS.color = new Color32(255,255,255,255);
        iconW.color = new Color32(255,255,255,255);
        iconO.color = new Color32(255,255,255,255);
        iconT.color = new Color32(255,255,255,255);

        Debug.Log("End Scene Transition");
        bg.DOAnchorPosX(-1920f,0.5f,false);
        iconS.DOFade(0f,0.25f);
        iconW.DOFade(0f,0.25f);
        iconO.DOFade(0f,0.25f);
        iconT.DOFade(0f,0.25f);
        yield return new WaitForSeconds(0.5f);
        yield return null;        
    }


}
