using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField] private Bonus bonusPrefab = null;
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private string collideWithTag = "Player";

    internal Action<Invader> onDestroy;

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
            Destroy(gameObject);
        }

        if (collision.gameObject.tag != collideWithTag) { return; }

        
        Destroy(gameObject);
        collision.gameObject.GetComponent<Bullet>()?.OnCollide();
        Destroy(collision.gameObject);
        List<BonusType> bonusAvailable = Player.Instance.GetBonusAvailable();
        if (bonusAvailable.Count > 0 && UnityEngine.Random.Range(0, 100) < 10) {
            BonusType bonusType = bonusAvailable[UnityEngine.Random.Range(0, bonusAvailable.Count)];
            Bonus bonus = Instantiate(bonusPrefab, transform.position, Quaternion.identity);
            bonus.Initialize(bonusType);
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, shootAt.position, Quaternion.identity);
    }
}
