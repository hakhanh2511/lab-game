using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverObserver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverText;
    
    [Header("Settings")]
    [SerializeField] private float restartDelay = 2f;
    
    private bool isGameOver = false;
    
    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    
    void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied += OnPlayerDied;
        }
    }
    
    void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied -= OnPlayerDied;
        }
    }
    
    void OnPlayerDied()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Debug.Log("<color=red><b>[GAME OVER] Player has died!</b></color>");
        
        // Hiển thị Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Update text
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\n\nPress R to Restart";
        }
        
        // Pause game (optional)
        // Time.timeScale = 0f;
        
        // Auto restart after delay
        Invoke(nameof(EnableRestart), restartDelay);
    }
    
    void EnableRestart()
    {
        Debug.Log("Press R to restart...");
    }
    
    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
    
    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}