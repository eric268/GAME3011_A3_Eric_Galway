using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Connect3Manager : MonoBehaviour
{
    public static bool gameIsRunning = false;
    public static bool gameWon = false;
    public static bool gameLost = false;
    public static int totalEasyModeTime = 60;
    public static int totalMediumModeTime = 70;
    public static int totalHardModeTime = 80;

    public static int easyTileWinCounter = 60;
    public static int mediumTileWinCounter = 80;
    public static int hardTileWinCounter = 100;

    public static DifficultyTypes gameDifficulty;
    public static int currentTime;
    public static int currentTileDestroyed = 0;
    public static int tilesToWin;
    public static int score;

    GridManager gridManager;
    public Connect3UIController uIController;

    public void Awake()
    {
        uIController = FindObjectOfType<Connect3UIController>();
        gridManager = FindObjectOfType<GridManager>();
        NewGame();
    }

    private void FixedUpdate()
    {
        if (gameIsRunning && !gameWon)
        {
            if (currentTileDestroyed >= tilesToWin)
            {
                CancelInvoke("DecrementTimer");
            }
            else if (currentTime <= 0 || gameLost)
            {
                uIController.SetGameOverText(false);
                CancelInvoke("DecrementTimer");
            }
            foreach (GameObject obj in gridManager.bombTileList)
            {
                int count = int.Parse(obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
                if (count <= 0)
                {
                    Connect3Manager.gameLost = true;
                }
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
        currentTileDestroyed = 0;
        gameIsRunning = true;
        gameWon = false;
        gameLost = false;
        InvokeRepeating("DecrementTimer", 0.0f, 1.0f);
        uIController.SetText();
                gridManager.bombTileList.Clear();
    }
}
