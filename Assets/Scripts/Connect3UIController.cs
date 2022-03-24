using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Connect3UIController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI tilesDestroyedText;
    public TextMeshProUGUI tilesToWinText;
    public TextMeshProUGUI gameOverText;
    public Connect3Manager connect3Manager;

    private void Start()
    {
        connect3Manager = FindObjectOfType<Connect3Manager>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        SetTimeRemainingText();
    }

    public void OnRestartPressed()
    {
        connect3Manager.NewGame();
        SetText();
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
        SceneManager.LoadScene("MainLevel");
    }

    public void OnLeavePressed()
    {
        SceneManager.LoadScene("MainLevel");
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

        switch(Connect3Manager.gameDifficulty)
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
            gameOverText.text = "You Won! \n Your score was: " + Connect3Manager.score;
        }
        else
        {
            gameOverText.color = Color.red;
            gameOverText.text = "You Lost! \n Your score was: " + Connect3Manager.score;
        }
    }

}
