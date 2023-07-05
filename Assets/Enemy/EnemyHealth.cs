using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Enemy))] 
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject mainCam;

    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;
    
    int currentHitPoints = 0;
    ParticleSystem hitParticles;

    Enemy enemy;

    Slider healthSlider;
    void OnEnable()
    {
        healthSlider = healthBar.GetComponent<Slider>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        currentHitPoints = maxHitPoints;
        healthSlider.value = maxHitPoints;
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        if (mainCam == null)
        {
            mainCam = GameObject.Find("Main Camera");
        }
        healthBar.transform.LookAt(healthBar.transform.position + mainCam.transform.forward);
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
