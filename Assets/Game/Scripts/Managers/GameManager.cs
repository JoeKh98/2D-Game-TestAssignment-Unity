using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;


public delegate void OnGameOverHandler();


public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject winText;
    [SerializeField] float winDelay = 2.0f;
    [SerializeField] float gameOverDelay = 2.0f;

    float timeLeft;
    bool gameOver = false;
    public bool GameOver => gameOver;

    private ScoreManager scoreManager; 

    public event OnGameOverHandler OnGameOverEvent;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.OnAllEnemiesKilledEvent += PlayerWin; 
        }
    }



    public void PlayerWin()
    {
        playerController.enabled = false;
        winText.SetActive(true);

        StartCoroutine(DelayedAction(winDelay));

        LoadNextLevel();
    }

    public void PlayerGameOver()
    {
        gameOver = true;
        playerController.enabled = false;
        gameOverText.SetActive(true);

        StartCoroutine(DelayedAction(gameOverDelay));

        OnGameOverEvent?.Invoke(); // For future needs
        ReloadLevel();
    }

    IEnumerator DelayedAction(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
    }

    private void StartNextLevelSequence()
    {
        // TODO Play win VFX 
        // TODO Stop audio etc
        // TODO Delay
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);

    }
    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
