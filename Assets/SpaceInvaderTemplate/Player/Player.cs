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
    [SerializeField] int projectiles = 1;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private int maxProjectiles = 3;
    [SerializeField] float projectileOffset = 0.5f;
    [SerializeField] bool sideProjectile = false;
    [SerializeField] Vector3 SideDirection = new Vector3(1, 0.85f, 0);
    [SerializeField] private Transform shootAtRight = null;
    [SerializeField] private Transform shootAtLeft = null;
    [SerializeField] bool homingProjectile = false;


    private float lastShootTimestamp = Mathf.NegativeInfinity;
    
    public static Player Instance { get => instance; }

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
        float move = Input.GetAxis("Horizontal");
        if (Mathf.Abs(move) < deadzone) { return; }

        move = Mathf.Sign(move);
        float delta = move * speed * Time.deltaTime;
        transform.position = GameManager.Instance.KeepInBounds(transform.position + Vector3.right * delta);
    }

    void UpdateActions()
    {
        if (    Input.GetKey(KeyCode.Space) 
            &&  Time.time > lastShootTimestamp + shootCooldown )
        {
            Shoot();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _laser.OnActivation();
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                _laser.OnCancel();
            }
        }

        

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
        lastShootTimestamp = Time.time;
    }
    
    void UpdateHealth()
    {
        _onDamageTaken.Invoke();
        health--;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        lifeUI.UpdateDisplay();
        if (health == 0)
        {
            _onDeath.Invoke();
            GameManager.Instance.PlayGameOver();
        }
        else
        {
            _onRespawn.Invoke();
            StartCoroutine(Respawn());
        }
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.transform.position = new Vector3(0, -4, 0);
        ResetProjectilesConfig();
        yield return null;
    }

    private void ResetProjectilesConfig()
    {
        projectiles = 1;
        sideProjectile = false;
        homingProjectile = false;
    }
    
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
}
