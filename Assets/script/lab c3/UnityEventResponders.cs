using UnityEngine;
using UnityEngine.UI;

public class UnityEventResponders : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject lowHealthWarning;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverText;

    [Header("Colors")]
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.yellow;
    [SerializeField] private Color criticalHealthColor = Color.red;

    // Hàm này sẽ được gọi từ UnityEvent OnHealthChanged
    public void UpdateHealthDisplay(float currentHealth, float maxHealth)
    {
        // Cập nhật slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            healthSlider.maxValue = maxHealth;
        }

        // Cập nhật text
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth:F0}/{maxHealth:F0}";
        }

        // Đổi màu fill dựa trên % máu
        if (fillImage != null)
        {
            float healthPercent = currentHealth / maxHealth;
            
            if (healthPercent > 0.5f)
                fillImage.color = healthyColor;
            else if (healthPercent > 0.3f)
                fillImage.color = lowHealthColor;
            else
                fillImage.color = criticalHealthColor;
        }

        Debug.Log($"<color=cyan>UI Updated: {currentHealth}/{maxHealth}</color>");
    }

    // Hàm hiển thị warning khi máu thấp
    public void ShowLowHealthWarning()
    {
        if (lowHealthWarning != null)
        {
            lowHealthWarning.SetActive(true);
            Debug.Log("<color=yellow>LOW HEALTH WARNING SHOWN!</color>");
        }
    }

    // Hàm ẩn warning
    public void ClearLowHealthWarning()
    {
        if (lowHealthWarning != null)
        {
            lowHealthWarning.SetActive(false);
            Debug.Log("<color=green>Health recovered - warning cleared</color>");
        }
    }

    // Hàm hiển thị Game Over
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (gameOverText != null)
            {
                gameOverText.text = "GAME OVER\nPress R to restart";
            }

            Debug.Log("<color=red>GAME OVER!</color>");
            Time.timeScale = 0; // Dừng game
        }
    }
}