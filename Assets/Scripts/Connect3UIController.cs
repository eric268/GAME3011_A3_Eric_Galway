using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Connect3UIController : MonoBehaviour
{
    public static DifficultyTypes difficultyType;

    public TMP_Dropdown difficultyDropDown;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI tilesDestroyedText;
    public TextMeshProUGUI tilesToWinText;
    public TextMeshProUGUI gameOverText;
    public Connect3Manager connect3Manager;
    public GridManager gridManager;
    public GridGenerator gridGenerator;

    private void Start()
    {
        connect3Manager = FindObjectOfType<Connect3Manager>();
        gridManager = FindObjectOfType<GridManager>();

        if (difficultyDropDown)
        {
            difficultyDropDown.onValueChanged.AddListener(delegate
            {
                OnDifficultyLevelSelected();
            });
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeText != null)
            SetTimeRemainingText();
    }

    public void OnRestartPressed()
    {
        connect3Manager.NewGame();
        SetText();
        gridManager.PopulateTilesInGrid(Connect3UIController.difficultyType);
    }

    public void SetText()
    {
        SetScoreText();
        SetTimeRemainingText();
        SetTilesDestroyedText();
        SetNewGameText();
        gameOverText.text = "";
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Connect3Scene");
    }

    public void OnLeavePressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void SetScoreText()
    {
        scoreText.text = "Score: " + Connect3Manager.score;
    }
    public void SetTimeRemainingText()
    {
        timeText.text = "Time Left: " + Connect3Manager.currentTime;
    }

    public void SetTilesDestroyedText()
    {
        tilesDestroyedText.text = "Tiles Destroyed: " + Connect3Manager.currentTileDestroyed;
    }

    public void SetNewGameText()
    {
        tilesToWinText.text = "Total Tiles To Win: " + Connect3Manager.tilesToWin;

        switch(difficultyType)
        {
            case DifficultyTypes.Easy:
                difficultyText.text = "Difficulty: Easy";
                break;
            case DifficultyTypes.Medium:
                difficultyText.text = "Difficulty: Medium";
                break;
            case DifficultyTypes.Hard:
                difficultyText.text = "Difficulty: Hard";
                break;
        }
    }

    public void SetGameOverText(bool gameWon)
    {
        if (gameWon)
        {
            gameOverText.color = Color.green;
            gameOverText.text = "You Won!";
        }
        else
        {
            gameOverText.color = Color.red;
            gameOverText.text = "You Lost!";
        }
    }

    public void OnDifficultyLevelSelected()
    {
        difficultyType = (DifficultyTypes)difficultyDropDown.value; 
    }    

}
