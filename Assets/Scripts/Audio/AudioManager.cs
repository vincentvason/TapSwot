using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource clickButton;
    [SerializeField] AudioSource chatButton;
    [SerializeField] AudioSource sendButton;

    public void PlayClickButton()
    {
        clickButton.Play();
    }

    public void PlayChatButton()
    {
        chatButton.Play();
    }

    public void PlaySendChatButton()
    {
        sendButton.Play();
    }
}
