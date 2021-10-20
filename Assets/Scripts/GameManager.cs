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
    private List<AsyncOperation> scenesLoading;

    private bool canDo = true;
    private UIScript uiScript;
    private float startTime;
    private float targetTime;
    private bool isTiming;
    private bool startedAsWin;

    //Set only
    private bool hasWon; /* Start as true and set false = win on surviving
                          Start as false and set true = win on action done*/

    //Read only
    private float progressValue, score, health;

    private Health _health;

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

        uiScript = gameObject.GetComponent<UIScript>();
        _health = gameObject.GetComponent<Health>();
        RestartValues();
    }
    #endregion

    private void Start()
    {
        scenesLoading = new List<AsyncOperation>();
    }

    #region Timing + Gameplay
    public void StartGame(float time, bool startAsWin, string gameplayText)
    {
        startTime = Time.time;
        targetTime = time;
        isTiming = true;
        hasWon = startAsWin;
        startedAsWin = startAsWin;
    }

    public void RestartGame()
    {
        NextGame();
        ResetValues();
    }

    private void Timer()
    {
        if (isTiming)
        { //Second bit check out (targetTime - Time.time + startTime)
            float timeRemap = (Time.time - startTime) / targetTime;
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
        if (canDo == true)
        {
            canDo = false;
            _health.RemoveOneHealth();
            health = _health.health;
            if (health <= 0) { GameOver(); ResetValues(); RestartValues(); }
            else { ResetValues(); NextGame(); }
        }
    }

    private void GameWon()
    {
        if (canDo == true)
        {
            canDo = false;
            score++;
            ResetValues();
            NextGame();
        }
    }
    #endregion

    #region SceneSwitching
    //Possible different colours on scene switching
    //Animation on start loading and end loading (before countdown + string given by player)

    private void NextGame()
    {
        LoadScene(Random.Range(startGamesCount, gamesCount + startGamesCount));
    }

    private void GameOver()
    {
        if (canDo == true)
        {
            canDo = false;
            uiScript.DeathScreen();
            LoadScene(0);
        }
    }

    private void LoadScene(int scene)
    {
        StartCoroutine(GetSceneLoadProgress(scene));
    }

    private IEnumerator GetSceneLoadProgress(int scene)
    {
        Debug.Log(scene);

        int toUnload = SceneManager.GetActiveScene().buildIndex;
        loadingScreen.GetComponentInChildren<Canvas>().enabled = true;
        scenesLoading.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));

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

        while (scenesLoading[scenesLoading.Count - 1].isDone)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(scene));
            scenesLoading.Add(SceneManager.UnloadSceneAsync(toUnload));
        }
        loadingScreen.GetComponentInChildren<Canvas>().enabled = false;
        

        canDo = true;
    }
    #endregion



    private void Update()
    {
        GameHandler();
    }

    private void RestartValues()
    {
        score = 0;
        _health.ResetHealth();
        health = _health.health;
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
