using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    float currentTime = 0; // in seconds
    public float allottedTime = 300; // in seconds
    bool paused = false;
    bool timeUp = false;
    TextMesh txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused) currentTime += Time.deltaTime;
        timeUp = isTimeUp();
        DisplayTimer();
    }

    void DisplayTimer()
    {
        if (timeUp)
        {
            txt.text = "Time's Up!";
        }
        else
        {
            txt.text = TimeToString();
        }
    }

    public void ResetTime()
    {
        currentTime = 0;
    }

    public string TimeToString()
    {
        string mins = (Mathf.Floor(currentTime / 60)).ToString();
        //string secs = (Mathf.Floor(currentTime % 60)).ToString();
        string secs = string.Format("{0:00}", Mathf.Floor(currentTime % 60));
        return (mins + ":" + secs);
    }

    public bool isTimeUp()
    {
        if (currentTime >= allottedTime)
        {
            Debug.Log("Time's up!");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void changeAllottedTime(float newTime)
    {
        allottedTime = newTime;
    }
}
