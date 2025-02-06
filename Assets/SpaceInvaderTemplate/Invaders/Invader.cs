using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Invader : MonoBehaviour
{
    [SerializeField] private Bonus bonusPrefab = null;
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private string collideWithTag = "Player";
    [SerializeField] int _value;
    [SerializeField] int _hp;
    [SerializeField]  UnityEvent onEnemeyDamageTaken;

    internal Action<Invader> onDestroy;

    [Header("Enemy Events")]
    [SerializeField] public UnityEvent onEnemyDeathLaser;
    [SerializeField] public UnityEvent onEnemyDeathBullet;
    [SerializeField] public UnityEvent onEnemyDamageTaken;
    [SerializeField] public UnityEvent onEnemyShoot;

    public Vector2Int GridIndex { get; private set; }

    public void Initialize(Vector2Int gridIndex)
    {
        this.GridIndex = gridIndex;
    }

    public void OnDestroy()
    {
        onDestroy?.Invoke(this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Laser")
        {
            onEnemyDeathLaser.Invoke();
            GameManager.Instance.Score += _value;
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.tag != collideWithTag) { return; }


        _hp--;
        if (_hp > 0)
        {
           onEnemeyDamageTaken.Invoke();
        }
        else
        {
            onEnemyDeathBullet.Invoke();
            GameManager.Instance.Score += _value;
            Destroy(gameObject);
            List<BonusType> bonusAvailable = Player.Instance.GetBonusAvailable();
            if (bonusAvailable.Count > 0 && UnityEngine.Random.Range(0, 100) < 10)
            {
                BonusType bonusType = bonusAvailable[UnityEngine.Random.Range(0, bonusAvailable.Count)];
                Bonus bonus = Instantiate(bonusPrefab, transform.position, Quaternion.identity);
                bonus.Initialize(bonusType);
            }
        }
        collision.gameObject.GetComponent<Bullet>()?.OnCollide();
        Destroy(collision.gameObject);
    }

    public void Shoot()
    {
        onEnemyShoot.Invoke();
        Instantiate(bulletPrefab, shootAt.position, Quaternion.identity);
    }
}
