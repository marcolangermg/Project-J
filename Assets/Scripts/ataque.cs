using UnityEngine;
using UnityEngine.InputSystem;

public enum DamageType
{
    Fire,
    Water,
    Earth,
    Air,
    Energy,
    Physical
}

[System.Serializable]
public class AttackProperties
{
    public float attackForce = 10.0f;
    public float attackRadius = 1.0f;
    public float attackRate = 1.0f;
    public KeyCode attackKey;
    public DamageType damageType = DamageType.Physical;
    public float damageAmount = 10.0f;
    private float nextAttackTime;

    public bool CanAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            return true;
        }
        return false;
    }
}

public class ataque : MonoBehaviour
{
    public AttackProperties[] attackProperties;

    private Vector2 lastMoveDirection;

    private void Update()
    {
        GetDirection();
        for (int i = 0; i < attackProperties.Length; i++)
        {
            Debug.Log(666);
            if (Input.GetKeyDown(attackProperties[i].attackKey) && attackProperties[i].CanAttack())
            {
                Debug.Log(123);
                PerformAttack(attackProperties[i]);
            }
        }
    }

    private void PerformAttack(AttackProperties attack)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, lastMoveDirection, attack.attackRadius);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.tag != "Player")
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attack.damageAmount, attack.damageType);
                }
            }
        }
    }

    private void GetDirection()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0.0f || vertical != 0.0f)
        {
            Vector2 direction = new Vector2(horizontal, vertical);
            direction.Normalize();
            lastMoveDirection = direction;
        }
    }
}
