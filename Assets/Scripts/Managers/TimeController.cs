using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    public TextMeshProUGUI timeCounter;

    private TimeSpan timePlaying;
    private bool timeGoing;
    private float elapsedTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCounter.text = "00:00";
        timeGoing = false;

        BeginTimer();
    }

    public void BeginTimer()
    {
        timeGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());

    }

    public void EndTimer()
    {
        timeGoing = false;

    }

    private IEnumerator UpdateTimer()
    {
        while(timeGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);

            string timePlayingStr = timePlaying.ToString("mm':'ss");
            timeCounter.text = timePlayingStr;

            yield return null;
        }
    }
}
