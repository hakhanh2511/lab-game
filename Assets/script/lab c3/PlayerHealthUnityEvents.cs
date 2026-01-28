using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthChangedEvent : UnityEvent<float, float> { }

public class PlayerHealthUnityEvents : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float lowHealthThreshold = 30f;

    [Header("Unity Events")]
    public HealthChangedEvent OnHealthChanged;
    public UnityEvent OnPlayerDied;
    public UnityEvent OnHealthLow;
    public UnityEvent OnHealthRecovered;

    private bool isLowHealth = false;

    void Start()
    {
        currentHealth = maxHealth;
        // Gọi event lần đầu để cập nhật UI
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        // Nhấn H để giảm máu
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }

        // Nhấn R để hồi máu
        if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(15f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"<color=red>Took damage: {amount}. Current HP: {currentHealth}</color>");

        // Gọi event health changed
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Kiểm tra low health
        if (currentHealth <= lowHealthThreshold && currentHealth > 0 && !isLowHealth)
        {
            isLowHealth = true;
            OnHealthLow?.Invoke();
        }

        // Kiểm tra chết
        if (currentHealth <= 0)
        {
            OnPlayerDied?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"<color=green>Healed: {amount}. Current HP: {currentHealth}</color>");

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Kiểm tra hồi phục
        if (currentHealth > lowHealthThreshold && isLowHealth)
        {
            isLowHealth = false;
            OnHealthRecovered?.Invoke();
        }
    }
}