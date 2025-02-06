using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;


    [Header("Player Events")]
    [SerializeField] public UnityEvent onPlayerShoot;
    [SerializeField] public UnityEvent onPlayerDamageTaken;
    [SerializeField] public UnityEvent onPlayerDeath;
    [SerializeField] public UnityEvent onPlayerRespawn;
    [SerializeField] public UnityEvent onLaserActivation;
    [SerializeField] public UnityEvent onLaserReady;
    [SerializeField] public UnityEvent onLaserRelease;
    [SerializeField] public UnityEvent onLaserFire;
    [SerializeField] public UnityEvent onPlayerMovement;
    [SerializeField] public UnityEvent onPlayerExhausted;

    

    [Header("Wave Events")]
    [SerializeField] public UnityEvent onWaveSpawn;
    [SerializeField] public UnityEvent onWaveMovement;
    private void Awake()
    {
        Instance = this;
    }


}
