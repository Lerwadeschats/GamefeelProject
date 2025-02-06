using System.Collections;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private Rigidbody2D rb;
    private BonusType _bonusType;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * 1f;
        Destroy(gameObject, 25f);
    }

    public void Initialize(BonusType bonusType)
    {
        _bonusType = bonusType;
    }
    
    public BonusType GetBonusType()
    {
        return _bonusType;
    }
    
    public void OnCollide()
    {
        Debug.Log("Bonus collided with Player");
    }
}

public enum BonusType
{
    ExtraLife,
    // Shield,
    ProjectileCount,
    SideProjectile,
    HomingMissile
}