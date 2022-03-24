using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect3Manager : MonoBehaviour
{
    public static bool gameIsRunning = false;
    public static bool gameWon = false;
    public static bool gameLost = false;
    public static int totalEasyModeTime = 75;
    public static int totalMediumModeTime = 60;
    public static int totalHardModeTime = 50;

    public static int easyTileWinCounter = 30;
    public static int mediumTileWinCounter = 40;
    public static int hardTileWinCounter = 50;

    public static DifficultyTypes gameDifficulty;
    public static int currentTime;
    public static int currentTileDestroyed = 0;
    public static int tilesToWin;
    public static int score;

    public Connect3UIController uIController;

    public void Awake()
    {
        uIController = FindObjectOfType<Connect3UIController>();
        NewGame();
    }

    private void FixedUpdate()
    {
        if (gameIsRunning && !gameWon && !gameLost)
        {
            if (currentTileDestroyed >= tilesToWin)
            {
                gameWon = true;
                CancelInvoke("DecrementTimer");
                uIController.SetGameOverText(true);

                //Broadcast some message 
            }
            else if (currentTime <= 0 || gameLost)
            {
                uIController.SetGameOverText(false);
                CancelInvoke("DecrementTimer");
            }
        }
    }

    public void SetRandomDifficulty()
    {
        DifficultyTypes ran = (DifficultyTypes)Random.Range(0, (int)DifficultyTypes.Num_Of_Difficulty_Types);
        switch (ran)
        {
            case DifficultyTypes.Easy:
                gameDifficulty = DifficultyTypes.Easy;
                currentTime = totalEasyModeTime;
                tilesToWin = easyTileWinCounter;
                break;
            case DifficultyTypes.Medium:
                gameDifficulty = DifficultyTypes.Medium;
                currentTime = totalMediumModeTime;
                tilesToWin = mediumTileWinCounter;
                break;
            case DifficultyTypes.Hard:
                gameDifficulty = DifficultyTypes.Hard;
                currentTime = totalHardModeTime;
                tilesToWin = hardTileWinCounter;
                break;
        }
    }
    public void DecrementTimer()
    {
        currentTime--;
    }

    public void NewGame()
    {
        score = 0;
        CancelInvoke("DecrementTimer");
        SetRandomDifficulty();
        gameIsRunning = true;
        gameWon = false;
        gameLost = false;
        InvokeRepeating("DecrementTimer", 0.0f, 1.0f);
        uIController.SetText();
    }
}
