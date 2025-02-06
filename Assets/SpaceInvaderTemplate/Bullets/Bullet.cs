using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float _speed = 4f;
    [SerializeField] Vector3 startVelocity;
    [SerializeField] List<GameObject> effects;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = startVelocity;
        Destroy(gameObject, 8f);
        if (GameManager.Instance._playerBulletTrailVFX)
        {
            foreach (GameObject g in effects)
            {
                g.SetActive(true);
            }
        }
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    
    public void SetVelocity(Vector3 velocity)
    {
        if (rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = velocity * _speed;
    }

    public void OnCollide()
    {
        Debug.Log("Bullet collided with something");
        Destroy(gameObject);
    }
}
