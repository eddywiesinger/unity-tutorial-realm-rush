using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Enemy))] 
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [SerializeField] GameObject healthBar;

    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;
    
    int currentHitPoints = 0;
    ParticleSystem hitParticles;

    Enemy enemy;
    AudioSource audioSource;
    Canvas canvas;

    Slider healthSlider;
    void OnEnable()
    {
        healthSlider = healthBar.GetComponent<Slider>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        canvas = GetComponentInChildren<Canvas>();
        ResetHealth();
    }

    void ResetHealth()
    {
        currentHitPoints = maxHitPoints;
        healthSlider.maxValue = maxHitPoints;
        healthSlider.value = maxHitPoints;
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        screenPosition.y += 60f * canvas.scaleFactor;
        healthBar.transform.position = screenPosition;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoints--;
        healthSlider.value -= 1;
        audioSource.Play();
        hitParticles.Play();

        if (currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }
}
