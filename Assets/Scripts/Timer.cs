using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text timer;
    float gameTime = 600f;
    float timeAtStart;
    bool start = false;

    void Start()
    {
        timer = GetComponent<Text>();
    }

    public void GameTime(float gameTime)
    {
        this.gameTime = gameTime;
    }

    public void StopTimer()
    {
        start = false;
    }

    public void StartTimer()
    { 
        timeAtStart = Time.time;
        start = true;
    }

    private void Update()
    {
        if (start)
        {
            string time = "";
            float currentTime = gameTime - (Time.time - timeAtStart);
            if ((int)(currentTime / 60) == 0)
                time += "00";
            else if ((int)(currentTime / 60) < 10)
                time += "0" + (int)(currentTime / 60);
            else
                time += (int)(currentTime / 60);

            time += ":";

            if ((int)(currentTime % 60) < 10)
                time += "0" + (int)(currentTime % 60);
            else
                time += (int)(currentTime % 60);

            timer.text = time;
        }
    }
}
