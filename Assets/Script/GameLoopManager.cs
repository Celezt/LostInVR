using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : Singleton <GameLoopManager>
{
    protected GameLoopManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    public int MaxGameTime = 120;

    public float secondsSinceLastItemDelivered;

    private GameObject ScoreTimeMonitor;
    private GameObject ActiveGameScreen;
    private GameObject GameOverScreen;
    private GameObject Client;
    private GameObject RegisterArea;
    private UnityEngine.UI.Text gameScoreDisplay;
    private UnityEngine.UI.Text gameTimeDisplay;
    private UnityEngine.UI.Text finalScoreDisplay;



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
                    //GameObject.FindGameObjectWithTag("Menu").SetActive(true);
                    Client.SetActive(false);
                    RegisterArea.SetActive(false);                    
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
        ScoreTimeMonitor = GameObject.Find("ScoreTimeMonitor");
        ActiveGameScreen = GameObject.Find("/ScoreTimeMonitor/ActiveGameScreen");
        GameOverScreen = GameObject.Find("/ScoreTimeMonitor/GameOverScreen");
        Client = GameObject.Find("Client");
        RegisterArea = GameObject.Find("RegisterArea");
        gameScoreDisplay = GameObject.Find("/ScoreTimeMonitor/ActiveGameScreen/ScoreValue").GetComponent< UnityEngine.UI.Text>();
        gameTimeDisplay = GameObject.Find("/ScoreTimeMonitor/ActiveGameScreen/TimeValue").GetComponent<UnityEngine.UI.Text>();
        finalScoreDisplay = GameObject.Find("/ScoreTimeMonitor/GameOverScreen/TotalScoreValue").GetComponent<UnityEngine.UI.Text>();
        Debug.Log("" + GameOverScreen + Client + RegisterArea + gameScoreDisplay + gameTimeDisplay);
        currentGameScore = 0;
        currentGameTimeRemaining = MaxGameTime;
        RemainingMinutes = (int)(currentGameTimeRemaining / 60f);
        RemainingSeconds = (int)(currentGameTimeRemaining % 60f);
        gameTimeDisplay.text = RemainingMinutes.ToString("00") + ":" + RemainingSeconds.ToString("00");
        gameScoreDisplay.text = "0";
        GameOverScreen.SetActive(false);
        Client.SetActive(false);
        RegisterArea.SetActive(false);
        ActiveGameScreen.SetActive(true);
        gameOver = false;
        currentGameState = GameState.Idle;
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
            AkSoundEngine.StopAll();
            currentGameState = GameState.Idle;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            ResetScene();
        }
    }
}
