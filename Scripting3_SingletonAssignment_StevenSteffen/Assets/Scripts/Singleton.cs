using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Singleton : MonoBehaviour
{ public static Singleton Instance { get; private set; }

    #region Variables
    [Header("Health Slider")]
    public int MaxHealth = 100;
    public int CurrentHealth = 100;
    public Slider HealthSlider;
    public Gradient HealthSliderGradient;
    public Image HealthSliderBackground;

    [Header("Timer")]
    public int timeSeconds;
    public float timeStart = 0;
    public TextMeshProUGUI timerText;
    public bool timerActive = false;

    [Header("Collectible Things")]
    public int thingsRemaining = 1;
    public TextMeshProUGUI thingsToCollect;

    [Header("Scene Management")]
    private static int sceneCounter = 1;

    [Header("Lives")]
    public int livesRemaining = 10;
    public TextMeshProUGUI playerLives;


    #endregion
    #region Main Functions
    private void Awake()
    {
        SetMaxHealth(MaxHealth);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        UpdateCollectibles();
        timerText.text = timeStart.ToString();
    }

    private void Update()
    {
        ReSpawn();
        TimeTimer();
    }

    #endregion
    #region Healthbar
    public void SetMaxHealth(int health)
    {
        HealthSlider.maxValue = health;
        HealthSlider.value = health;
        HealthSliderBackground.color = HealthSliderGradient.Evaluate(1.0f);
    }

    public void UpdateHealthBar(int health)
    {
        HealthSlider.value = health;
        HealthSliderBackground.color = HealthSliderGradient.Evaluate(HealthSlider.normalizedValue);
    }

    #endregion
    #region Health System
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        UpdateHealthBar(CurrentHealth);
    }

    public void ReSpawn()
    {
        if (CurrentHealth == 0)
        {
            SceneManager.LoadScene(sceneCounter);
            CurrentHealth = 100;
            UpdateHealthBar(CurrentHealth);
        }
    }

    public void GiveHealth(int health)
    {
        if (CurrentHealth <= MaxHealth)
        {
            CurrentHealth += health;
            UpdateHealthBar(CurrentHealth);
        }
    }

    #endregion

    #region Collectibles Functions
    public void Collect()
    {
        thingsRemaining -= 1;
        thingsToCollect.text = "Things Remaining: " + thingsRemaining;

        if (thingsRemaining == 0)
        {
            sceneCounter++;
            SceneManager.LoadScene(sceneCounter);
            thingsRemaining = 1;
        }
        UpdateCollectibles();
    }

    public void UpdateCollectibles()
    {
        thingsToCollect.text = "Things Remaining: " + thingsRemaining;
    }

    public void TimeTimer()
    {
        timeStart += Time.deltaTime;
        timerText.text = Mathf.Round(timeStart).ToString();

        if (sceneCounter == 5)
        {
            timerActive = false;
        }
    }

    #endregion
}