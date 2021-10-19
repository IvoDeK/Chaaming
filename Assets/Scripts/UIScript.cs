using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject deathScreen;

    [SerializeField] private Button mainStartButton;
    [SerializeField] private Button mainTutorialButton;
    [SerializeField] private Button mainHighScoreButton;
    
    [SerializeField] private Button deathRestartButton;
    [SerializeField] private Button deathMainButton;

    [SerializeField] private Scrollbar progressbar;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        MainMenu();
        
        inGame.GetComponentInChildren<Canvas>().enabled = false;
        loading.GetComponentInChildren<Canvas>().enabled = false;
        deathScreen.GetComponentInChildren<Canvas>().enabled = false;
        
        mainStartButton.onClick.AddListener(delegate { RestartGame(); });
        mainTutorialButton.onClick.AddListener(delegate { Tutorial(); });
        mainHighScoreButton.onClick.AddListener(delegate { HighScores(); });
        
        deathRestartButton.onClick.AddListener(delegate { RestartGame(); });
        deathMainButton.onClick.AddListener(delegate { MainMenu(); });
    }

    private void Update()
    {
        SetProgressbar();
    }

    private void SetProgressbar()
    {
        progressbar.size = gm.GetProgressValue();
    } 

    private void Tutorial()
    {
        mainMenu.GetComponentInChildren<Canvas>().enabled = false;
        RestartGame();
    }

    private void HighScores()
    {
        mainMenu.GetComponentInChildren<Canvas>().enabled = false;
        RestartGame();
    }

    private void RestartGame()
    {
        mainMenu.GetComponentInChildren<Canvas>().enabled = false;
        deathScreen.GetComponentInChildren<Canvas>().enabled = false;
        inGame.GetComponentInChildren<Canvas>().enabled = true;
        gm.RestartGame();
    }

    private void MainMenu()
    {
        deathScreen.GetComponentInChildren<Canvas>().enabled = false;
        mainMenu.GetComponentInChildren<Canvas>().enabled = true;
        mainStartButton.Select();
    }

    public void ResetButton()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void DeathScreen()
    {
        inGame.GetComponentInChildren<Canvas>().enabled = false;
        deathScreen.GetComponentInChildren<Canvas>().enabled = true;
        deathRestartButton.Select();
    }
}
