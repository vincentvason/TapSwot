using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource clickButton;

    public void PlayClickButton()
    {
        clickButton.Play();
    }
}
