using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : Singleton <GameLoopManager>
{
    protected GameLoopManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    public int MaxGameTime = 120;

    public float secondsSinceLastItemDelivered;

    public GameObject ActiveGameScreen;
    public GameObject GameOverScreen;
    public GameObject Client;
    public GameObject RegisterArea;
    public UnityEngine.UI.Text gameScoreDisplay;
    public UnityEngine.UI.Text gameTimeDisplay;
    public UnityEngine.UI.Text finalScoreDisplay;

    private int currentGameScore = 0, RemainingMinutes = 0, RemainingSeconds = 0;
    public float currentGameTimeRemaining = 120;

    [HideInInspector]
    public bool gameOver = false;

    public enum GameState
    {
        Idle,
        Started,
        GameOver
    }

    public GameState currentGameState = GameState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        ResetScene();
    }

    // Update is called once per frame
    void Update()
    {
        secondsSinceLastItemDelivered += Time.deltaTime;
        AkSoundEngine.SetRTPCValue("GameTimeRemaining", currentGameTimeRemaining);
        if (currentGameState == GameState.Started)
        {
            if (!gameOver)
            {
                currentGameTimeRemaining -= Time.fixedDeltaTime;
                RemainingMinutes = (int)(currentGameTimeRemaining / 60f);
                RemainingSeconds = (int)(currentGameTimeRemaining % 60f);
                gameTimeDisplay.text = RemainingMinutes.ToString("00") + ":" + RemainingSeconds.ToString("00");

                if (currentGameTimeRemaining <= 0)
                {
                    DisplayGameOverMessage();
                    GameObject.FindGameObjectWithTag("Menu").SetActive(true);
                    gameOver = true;
                    currentGameState = GameState.GameOver;
                }
            }
        }
        
    }

    public void IncreaseScore(int scoreincreaseamount)
    {
        {
            currentGameScore += scoreincreaseamount;
            gameScoreDisplay.text = "" + currentGameScore;
        }
    }

    public void IncreaseTimeRemaining(int timeincreaseamount)
    {
        {
            currentGameTimeRemaining += timeincreaseamount;
            RemainingMinutes = (int)(currentGameTimeRemaining / 60f);
            RemainingSeconds = (int)(currentGameTimeRemaining % 60f);
            gameTimeDisplay.text = RemainingMinutes.ToString("00") + ":" + RemainingSeconds.ToString("00");
        }
    }

    public void ResetScene()
    {
        currentGameScore = 0;
        currentGameTimeRemaining = MaxGameTime;
        RemainingMinutes = (int)(currentGameTimeRemaining / 60f);
        RemainingSeconds = (int)(currentGameTimeRemaining % 60f);
        gameTimeDisplay.text = RemainingMinutes.ToString("00") + ":" + RemainingSeconds.ToString("00");
        gameScoreDisplay.text = "0";
        GameOverScreen.SetActive(false);
        ActiveGameScreen.SetActive(true);
        gameOver = false;
    }

    public void DisplayGameOverMessage()
    {
        finalScoreDisplay.text = "" + currentGameScore;
        ActiveGameScreen.SetActive(false);
        GameOverScreen.SetActive(true);
    }

    public void StartGame()
    {
        if (currentGameState == GameState.Started)
        {
            return;
        }
        
        else if (currentGameState == GameState.Idle)
        {
            currentGameState = GameState.Started;
            Client.SetActive(true);
            RegisterArea.SetActive(true);
        }
        
        else if (currentGameState == GameState.GameOver)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
