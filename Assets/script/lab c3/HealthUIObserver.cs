using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIObserver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    
    [Header("Colors")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private Image fillImage;
    
    void OnEnable()
    {
        // SUBSCRIBE to event
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
        }
    }
    
    void OnDisable()
    {
        // UNSUBSCRIBE from event
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }
    
    // Event handler
    void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        float percentage = currentHealth / maxHealth;
        
        // Update slider
        if (healthSlider != null)
        {
            healthSlider.value = percentage;
        }
        
        // Update text
        if (healthText != null)
        {
            healthText.text = $"{currentHealth:F0} / {maxHealth:F0}";
        }
        
        // Update color
        if (fillImage != null)
        {
            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, percentage);
        }
        
        Debug.Log($"<color=cyan>[UI] Health updated: {percentage * 100:F0}%</color>");
    }
}