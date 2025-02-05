using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Player : MonoBehaviour
{
    [SerializeField] private float deadzone = 0.3f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int health = 3;
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private LifeUI lifeUI;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private string collideWithTag = "Untagged";

    [SerializeField] private Laser _laser;

    


    private float lastShootTimestamp = Mathf.NegativeInfinity;

    private void Awake()
    {
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
                EventManager.Instance.onLaserActivation.Invoke();
                _laser.OnActivation();
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                EventManager.Instance.onLaserRelease.Invoke();
                _laser.OnActivation();
            }
        }

        

    }

    void Shoot()
    {
        EventManager.Instance.onPlayerShoot.Invoke();
        Instantiate(bulletPrefab, shootAt.position, Quaternion.identity);
        lastShootTimestamp = Time.time;
    }
    void UpdateHealth()
    {
        EventManager.Instance.onPlayerDamageTaken.Invoke();
        health--;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        lifeUI.UpdateDisplay();
        if (health == 0)
        {
            EventManager.Instance.onPlayerDeath.Invoke();
            GameManager.Instance.PlayGameOver();
        }
        else
        {
            EventManager.Instance.onPlayerRespawn.Invoke();
            StartCoroutine(Respawn());
        }
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.transform.position = new Vector3(0, -4, 0);
        yield return null;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != collideWithTag) { return; }

        UpdateHealth();
    }
}
