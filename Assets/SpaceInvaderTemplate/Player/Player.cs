using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Player : MonoBehaviour
{
    private static Player instance;
    
    [SerializeField] private float deadzone = 0.3f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int health = 3;
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private LifeUI lifeUI;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private string collideWithTag = "Untagged";

    [SerializeField] private Laser _laser;

    [SerializeField] UnityEvent _onShoot;
    [SerializeField] UnityEvent _onDamageTaken;
    [SerializeField] UnityEvent _onDeath;
    [SerializeField] UnityEvent _onRespawn;


    private float lastShootTimestamp = Mathf.NegativeInfinity;
    
    public static Player Instance { get => instance; }

    bool _isExhausted;
    [SerializeField] float _exhaustion = 5f;

    bool _isImmobile;

    private void Awake()
    {
        if (instance != null)  {
            Destroy(gameObject);
            return;
        }
        instance = this;
        _laser = transform.Find("Laser").GetComponent<Laser>();
    }

    void Update()
    {
        UpdateMovement();
        UpdateActions();
    }

    void UpdateMovement()
    {
        if (!_isImmobile)
        {
            float move = Input.GetAxis("Horizontal");
            if (Mathf.Abs(move) < deadzone) { return; }


            move = Mathf.Sign(move);
            float delta = move * speed * Time.deltaTime;
            transform.position = GameManager.Instance.KeepInBounds(transform.position + Vector3.right * delta);
            EventManager.Instance.onPlayerMovement?.Invoke();
        }
        
    }

    void UpdateActions()
    {
        if (!_isExhausted)
        {
            if (Input.GetKey(KeyCode.Space)
            && Time.time > lastShootTimestamp + shootCooldown)
            {
                Shoot();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    EventManager.Instance.onLaserActivation?.Invoke();
                    _laser.OnActivation();
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    EventManager.Instance.onLaserRelease?.Invoke();
                    _laser.OnRelease();
                }
            }
        }
        

        

    }

    public void NoMovementMode(bool isNowImmobile)//j'avais pas d'id�e de nom
    {
        _isImmobile = isNowImmobile;
    }

    void Shoot()
    {
        _onShoot.Invoke();
        Vector3 position = shootAt.position;
        for (int i = 0; i < projectiles; i++) {
            Vector3 correctPosition = position + Vector3.right * ((i - (projectiles - 1) / 2) * projectileOffset);
            Bullet bullet = Instantiate(bulletPrefab, correctPosition, Quaternion.identity);
            bullet.SetSpeed(projectileSpeed);
            bullet.SetVelocity(Vector3.up);
        }

        if (sideProjectile) {
            position = shootAtRight.position;
            Vector3 direction = SideDirection;
            direction.y *= -1;
            for (int i = 0; i < projectiles; i++) {
                Vector3 correctPosition = position + direction * ((i - (projectiles - 1) / 2) * projectileOffset);
                Bullet bullet = Instantiate(bulletPrefab, correctPosition, Quaternion.identity);
                bullet.SetSpeed(projectileSpeed);
                bullet.SetVelocity(SideDirection.normalized);
            }
            position = shootAtLeft.position;
            Vector3 reverseSideDirection = SideDirection;
            reverseSideDirection.x *= -1;
            direction = reverseSideDirection;
            direction.y *= -1;
            for (int i = 0; i < projectiles; i++) {
                Vector3 correctPosition = position + direction * ((i - (projectiles - 1) / 2) * projectileOffset);
                Bullet bullet = Instantiate(bulletPrefab, correctPosition, Quaternion.identity);
                bullet.SetSpeed(projectileSpeed);
                bullet.SetVelocity(reverseSideDirection.normalized);
            }
        }
        EventManager.Instance.onPlayerShoot?.Invoke();
        Instantiate(bulletPrefab, shootAt.position, Quaternion.identity);
        lastShootTimestamp = Time.time;
    }
    
    void UpdateHealth()
    {
        EventManager.Instance.onPlayerDamageTaken?.Invoke();
        health--;
        //GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        lifeUI.UpdateDisplay();
        if (health == 0)
        {
            EventManager.Instance.onPlayerDeath?.Invoke();
            GameManager.Instance.PlayGameOver();
        }
        else
        {
            EventManager.Instance.onPlayerRespawn?.Invoke();
            //StartCoroutine(Respawn());
        }
    }
    //IEnumerator Respawn()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    //GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    //    //gameObject.transform.position = new Vector3(0, -4, 0);
    //    yield return null;
    //}
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bonus"))
        {
            Bonus bonus = collision.gameObject.GetComponent<Bonus>();
            Debug.Log($"Get bonus {bonus.GetBonusType()}");
            switch (bonus.GetBonusType())
            {
                case BonusType.ExtraLife:
                    health++;
                    if (health > lifeUI.GetMaxLife()) {
                        health = lifeUI.GetMaxLife();
                    }
                    break;
                case BonusType.ProjectileCount:
                    projectiles++;
                    if (projectiles > maxProjectiles) {
                        projectiles = maxProjectiles;
                    }
                    break;
                case BonusType.SideProjectile:
                    sideProjectile = true;
                    break;
                case BonusType.HomingMissile:
                    homingProjectile = true;
                    break;
            }
            lifeUI.UpdateDisplay(health);
            collision.gameObject.GetComponent<Bonus>()?.OnCollide();
            Destroy(collision.gameObject);
        } else {
            if (collision.gameObject.tag != collideWithTag) { return; }
            UpdateHealth();
        }
    }

    public List<BonusType> GetBonusAvailable()
    {
        List<BonusType> bonusAvailable = new List<BonusType>() { BonusType.ExtraLife, BonusType.ProjectileCount, BonusType.SideProjectile, BonusType.HomingMissile };
        
        if (health >= lifeUI.GetMaxLife()) {
            bonusAvailable.Remove(BonusType.ExtraLife);
        }
        if (projectiles >= maxProjectiles) {
            bonusAvailable.Remove(BonusType.ProjectileCount);
        }
        if (sideProjectile) {
            bonusAvailable.Remove(BonusType.SideProjectile);
        }
        if (homingProjectile) {
            bonusAvailable.Remove(BonusType.HomingMissile);
        }
        string bonus = "";
        foreach (var item in bonusAvailable)
        {
            bonus += item.ToString() + " ";
        }
        Debug.Log($"Bonus available: {bonusAvailable.Count}: {bonus}");
        return bonusAvailable;
    }

    public void IsExhausted()
    {
        _isExhausted = true;
        StartCoroutine(Exhaustion());
    }

    IEnumerator Exhaustion()
    {
        yield return new WaitForSeconds(_exhaustion);
        _isExhausted = false;
    }
}
