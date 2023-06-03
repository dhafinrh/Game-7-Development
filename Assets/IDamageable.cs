using UnityEngine;

public interface IDamageable
{
    public float Health
    {
        set; get;
    }
    public bool Invincible
    {
        set; get;
    }

    public void OnHit(float damage, Vector2 knockBack)
    {

    }

    public void OnHit(float damage, int crit)
    {

    }
}