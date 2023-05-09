using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VideoPlayerAnimation : MonoBehaviour
{
    public GameObject introductionBubble;
    public GameObject introductionDialogue;
    public GameObject introductionButton;
    public GameObject videoPlayer;
    public GameObject introductionPanel;
    public TimeController timeController;

    [SerializeField] private float timeElapsed;
    [SerializeField] private bool isVideoPlayed;

    public GameObject backBlackScreen;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        backBlackScreen.SetActive(true);
        introductionPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        introductionBubble.transform.DOScale(1,0.1f);
        yield return new WaitForSeconds(0.2f);
        introductionDialogue.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        introductionButton.SetActive(true);
    }

    void Update()
    {
        if(isVideoPlayed == true)
        {
            timeElapsed = timeElapsed + Time.deltaTime;
        }

        if(timeElapsed > 633 && videoPlayer.activeSelf == true)
        {
            videoPlayer.SetActive(false);
            backBlackScreen.SetActive(false);
        }
    }

    public void ProceedButton()
    {
        StartCoroutine(Proceed());
    }

    IEnumerator Proceed()
    {
        introductionDialogue.SetActive(false);
        introductionButton.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        introductionPanel.transform.DOScale(3,0.5f);
        videoPlayer.SetActive(true);
        videoPlayer.transform.DOScale(1,0.1f).OnComplete(()=>
                StartCoroutine(DisableAfterAnimationComplete()
        ));
    }

    private IEnumerator DisableAfterAnimationComplete()
    {
        yield return new WaitForSeconds(2f);
        backBlackScreen.SetActive(false);
        introductionPanel.SetActive(false);
    }
}
