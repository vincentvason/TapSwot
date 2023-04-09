using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuAnimation : MonoBehaviour
{
    [SerializeField] SceneTransition menuTransition;
    [SerializeField] RectTransform createJoinWindow;
    [SerializeField] RectTransform lobbyWaitingWindow;

    // Start is called before the first frame update
    void Start()
    {
        createJoinWindow.anchoredPosition = new Vector2(0f,540f);
        lobbyWaitingWindow.anchoredPosition = new Vector2(0f,540f);
    }

    public IEnumerator OpenCreateJoinWindow()
    {
        createJoinWindow.DOAnchorPosY(-540f,0.5f,false);
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator CloseCreateJoinWindow()
    {
        createJoinWindow.DOAnchorPosY(540f,0.5f,false);
        yield return new WaitForSeconds(1f);
    } 

    public IEnumerator OpenLobbyWaitingWindow()
    {
        lobbyWaitingWindow.DOAnchorPosY(-540f,0.5f,false);
        yield return new WaitForSeconds(1f);
    } 

    public IEnumerator CloseLobbyWaitingWindow()
    {
        lobbyWaitingWindow.DOAnchorPosY(540f,0.5f,false);
        yield return new WaitForSeconds(1f);
    }  
}
