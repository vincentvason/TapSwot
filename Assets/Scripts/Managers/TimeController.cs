using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI maxTimeLimit;

    private TimeSpan timePlaying;
    private bool timeGoing = false;
    private float elapsedTime;
    private float timeLimit = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {      
        string timeLimitType = PhotonNetwork.CurrentRoom.CustomProperties["TIME_LIMIT"].ToString();
        
        if(timeLimitType == "No Time Limit")
        {
            maxTimeLimit.text = timeLimitType;
            timeCounter.color = Color.white;
            timeCounter.text = "<font-weight=900>0:00:00";
        }
        else if(timeLimitType == "30 minutes")
        {
            timeLimit = 30*60;
            timeCounter.text = "<font-weight=900>0:30:00";
        }
        else if(timeLimitType == "1 hour")
        {
            timeLimit = 1*60*60;
            timeCounter.text = "<font-weight=900>1:00:00";
        }
        else if(timeLimitType == "1.5 hours")
        {
            timeLimit = 1.5f*60*60;
            timeCounter.text = "<font-weight=900>1:30:00";
        }
        else if(timeLimitType == "2 hours")
        {
            timeLimit = 2*60*60;
            timeCounter.text = "<font-weight=900>2:00:00";
        }
        else if(timeLimitType == "2.5 hours")
        {
            timeLimit = 2.5f*60*60;
            timeCounter.text = "<font-weight=900>2:30:00";
        }
        else if(timeLimitType == "3 hours")
        {
            timeLimit = 3*60*60;
            timeCounter.text = "<font-weight=900>3:00:00";
        }

        maxTimeLimit.text = "Time Limit: "+timeLimitType;
    }

    public void BeginTimer()
    {
        Debug.Log("Begin Timer");
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
            string timePlayingStr;

            //No time limit
            if(timeLimit == 0)
            {
                timePlayingStr = "<font-weight=900>" + TimeSpan.FromSeconds(elapsedTime).ToString("h':'mm':'ss");
            }
            else
            {
                timePlayingStr = "<font-weight=900>" + TimeSpan.FromSeconds(timeLimit-elapsedTime).ToString("h':'mm':'ss");
            }

            timeCounter.text = timePlayingStr;
            yield return null;
        }
    }
}
