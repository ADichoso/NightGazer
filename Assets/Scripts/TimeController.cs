using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeController : MonoBehaviour
{
    #region Singleton
    public static TimeController sharedInstance;

    void Awake()
    {
        sharedInstance = this;
        if (this != sharedInstance)
        {
            Debug.Log("Warning! More than 1 instance of TimerController has been detected");
        }
    }
    #endregion

    public TextMeshProUGUI timeCounter;

    private TimeSpan timePlaying;
    public float remainingTime;
    private bool isTimePlaying = false;

    public void BeginTimer(int startingTimeInSeconds)
    {
        isTimePlaying = true;
        remainingTime = startingTimeInSeconds;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer(int index)
    {
        totalTimes[index] = remainingTime;
        isTimePlaying = false;

        StopAllCoroutines();
    }

    IEnumerator UpdateTimer()
    {
        while (isTimePlaying)
        {
            if (!UIController.sharedInstance.isPaused)
            {
                remainingTime -= Time.deltaTime;

                timePlaying = TimeSpan.FromSeconds(remainingTime);

                string timeString = "Time: " + timePlaying.ToString("mm':'ss'.'ff");

                timeCounter.text = timeString;

                if (remainingTime <= 0)
                {
                    LevelController.sharedInstance.RestartLevel();
                }

            }
            yield return null;
        }
    }

    public void decreaseTimer(int decrease)
    {
        remainingTime -= decrease;
    }

    public float[] totalTimes;
}
