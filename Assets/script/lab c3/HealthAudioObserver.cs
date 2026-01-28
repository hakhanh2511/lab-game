using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HealthAudioObserver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip lowHealthSound;
    
    [Header("Settings")]
    [SerializeField] private float lowHealthThreshold = 0.3f;
    
    private AudioSource audioSource;
    private float previousHealth;
    private bool isLowHealthPlaying;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (playerHealth != null)
        {
            previousHealth = playerHealth.GetHealthPercentage() * 100f;
        }
    }
    
    void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += OnHealthChanged;
        }
    }
    
    void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= OnHealthChanged;
        }
    }
    
    void OnHealthChanged(float currentHealth, float maxHealth)
    {
        float percentage = currentHealth / maxHealth;
        
        // Phát âm thanh damage hoặc heal
        if (currentHealth < previousHealth)
        {
            PlaySound(damageSound);
            Debug.Log("<color=orange>[Audio] Damage sound played</color>");
        }
        else if (currentHealth > previousHealth)
        {
            PlaySound(healSound);
            Debug.Log("<color=green>[Audio] Heal sound played</color>");
        }
        
        // Low health warning
        if (percentage <= lowHealthThreshold && !isLowHealthPlaying)
        {
            PlaySound(lowHealthSound);
            isLowHealthPlaying = true;
            Debug.Log("<color=yellow>[Audio] Low health warning!</color>");
        }
        else if (percentage > lowHealthThreshold)
        {
            isLowHealthPlaying = false;
        }
        
        previousHealth = currentHealth;
    }
    
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}