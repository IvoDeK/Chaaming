using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //In code
    public int gamesCount = 2;
    private int startGamesCount = 1;
    private float totalSceneProgress;
    public GameObject loadingScreen;
    public Image loadingImage;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private float startTime;
    private float targetTime;
    private bool isTiming;
    private bool startedAsWin;

    //Set only
    private bool hasWon; /* Start as true and set false = win on surviving
                          Start as false and set true = win on action done*/

    //Read only
    private float progressValue, score, health;

    //Player options (read only) (not needed)
    private bool playSound, playMusic;
    private float volumeSound, volumeMusic;

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
    public void StartGame(float time, bool startAsWin, string gameplayText)
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
        else { ResetValues(); NextGame(); }
    }

    private void GameWon()
    {
        score++;
        ResetValues();
        NextGame();
    }
    #endregion

    #region SceneSwitching
    //Possible different colours on scene switching
    //Animation on start loading and end loading (before countdown + string given by player)

    private void NextGame()
    {
        LoadScene(Random.Range(startGamesCount, gamesCount + 1 + startGamesCount));
    }

    private void GameOver()
    {
        LoadScene(0);
    }

    private void LoadScene(int scene)
    {
        loadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex));
        scenesLoading.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    private IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }
                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                loadingImage.fillAmount = totalSceneProgress;
                yield return null;
            }
        }

        loadingScreen.gameObject.SetActive(false);
    }
    #endregion



    private void Update()
    {
        GameHandler();
    }

    private void ResetValues()
    {
        isTiming = false;
        hasWon = false;
        startedAsWin = false;
        startTime = 0;

        //Not really needed
        progressValue = 0;
    }

    public int GetHealth() { return (int)health; }
    public float GetProgressValue() { return progressValue; }
    public float GetTime() { return Time.time - startTime; }
    public int GetScore() { return (int)score; }
    public void SetWon(bool won) { hasWon = won; }
}
