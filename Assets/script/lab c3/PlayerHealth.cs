using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    // C# EVENTS - Observer Pattern
    public event Action<float, float> OnHealthChanged;  // (currentHealth, maxHealth)
    public event Action OnPlayerDied;
    
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    void Update()
    {
        // Press H để giảm máu (test)
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
        
        // Press R để hồi máu
        if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(20f);
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        
        Debug.Log($"<color=red>Took {damage} damage! Health: {currentHealth}/{maxHealth}</color>");
        
        // Phát sự kiện health changed
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // Nếu chết, phát sự kiện died
        if (currentHealth <= 0)
        {
            OnPlayerDied?.Invoke();
        }
    }
    
    public void Heal(float amount)
    {
        if (currentHealth >= maxHealth || currentHealth <= 0) return;
        
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        Debug.Log($"<color=green>Healed {amount}! Health: {currentHealth}/{maxHealth}</color>");
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}