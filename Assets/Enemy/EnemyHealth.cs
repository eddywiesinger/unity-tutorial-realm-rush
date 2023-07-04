using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Enemy))] 
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [SerializeField] AudioSource audioSource;

    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;
    
    int currentHitPoints = 0;
    ParticleSystem hitParticles;

    Enemy enemy;
    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
        hitParticles = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoints--;
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
