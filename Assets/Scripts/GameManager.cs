using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //In code
    private float startTime;
    private float targetTime;
    private bool isTiming;
    private bool startedAsWin;

    //Set only
    private bool hasWon; /* Start as true and set false = win on surviving
                          Start as false and set true = win on action done*/

    //Read only
    private float progressValue, score, health;

    //Player options (read only)
    private bool playSound, playMusic;

    #region singleton
    public static GameManager instance { get; private set; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    #endregion

    #region Timing + Gameplay
    public void StartGame(float time, bool startAsWin)
    {
        startTime = Time.time;
        targetTime = time;
        isTiming = true;
        hasWon = startAsWin;
        startedAsWin = startAsWin;
    }

    private void Timer()
    {
        if (isTiming)
        { //Second bit check out (targetTime - Time.time + startTime)
            float timeRemap = (Time.time - startTime) / (targetTime - startTime);
            progressValue = Mathf.Clamp(timeRemap, 0, 1);
        }
    }

    private void GameHandler()
    {
        Timer();

        if (startedAsWin)
        {
            if (!hasWon) { GameLost(); }
            else 
            {
                if (progressValue >= 1)
                {
                    GameWon();
                }
            }
        }
        else 
        {
            if (hasWon) { GameWon(); }
            else 
            {
                if (progressValue >= 1)
                {
                    GameLost();
                }
            }
        }
    }
    
    private void GameLost()
    {
        health--;
        if (health <= 0) GameOver();
        else { NextGame(); ResetValues(); }
    }

    private void GameWon()
    {
        score++;
        ResetValues();
        NextGame();
    }

    private void NextGame()
    {
        //Do stuff (Scene switch to random scene out of list)
    }

    private void GameOver()
    {
        //Do stuff (go back to main scene)
    }
    #endregion

    void Update()
    {
        GameHandler();
    }

    private void ResetValues()
    {
        isTiming = false;
        hasWon = false;
        startTime = 0;

        //Not really needed
        progressValue = 0;
    }

    public float GetProgressValue() { return progressValue; }
    public float GetTime() { return Time.time - startTime; }
    public int GetScore() { return (int)score; }
    public void HasWon() { hasWon = true; }
}
