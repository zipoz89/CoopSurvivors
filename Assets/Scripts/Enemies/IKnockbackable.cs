using UnityEngine;

public interface IKnockbackable
{
    public void ApplyKnockBack(float knockBackForce, Vector2 direction);
}