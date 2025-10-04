using TMPro;
using UnityEngine;

public delegate void OnAllEnemiesKilledHandler();

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text scoreText;
    int kills = 0;
    private int currentEnemiesCount;
    public event OnAllEnemiesKilledHandler OnAllEnemiesKilledEvent; 

    void Start()
    {
        currentEnemiesCount = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;
    }

    public void IncreaseKillCounter()
    {
        if (gameManager.GameOver) return;

        ++kills;

        if (kills >= currentEnemiesCount)
        {
            OnAllEnemiesKilledEvent?.Invoke(); 
        }

        scoreText.text = kills.ToString();
    }

    public void ClearKillCounter()
    {
        kills = 0;
    }
}
