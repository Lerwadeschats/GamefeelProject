using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance = null;


    [Header("Player Events")]
    [SerializeField] public UnityEvent onPlayerShoot, onPlayerDamageTaken, onPlayerDeath, onPlayerRespawn, onLaserActivation, onLaserRelease;

    [Header("Enemy Events")]
    [SerializeField] public UnityEvent onEnemyDeathLaser, onEnemyDeathBullet, onEnemyShoot, onEnemySpawn;

    [Header("Wave Events")]
    [SerializeField] public UnityEvent onWaveSpawn, onWaveMovement;
    private void Awake()
    {
        Instance = this;
    }


}
