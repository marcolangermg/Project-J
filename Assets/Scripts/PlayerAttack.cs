using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public enum AttackType
{
    Melee,
    Ranged
}

[System.Serializable]
public class AttackProperties
{
    public RangedAttack rangedAttack;
    public float attackRadius = 1.0f;
    public float attackRate = 1.0f;
    public KeyCode attackKey;
    public DamageType damageType = DamageType.Physical;
    public float damageAmount = 10.0f;
    public AttackType attackType = AttackType.Melee;
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

public class PlayerAttack : MonoBehaviour
{
    public AttackProperties[] attackProperties;

    private Vector2 lastMoveDirection;

    private void Update()
    {
        GetDirection();
        for (int i = 0; i < attackProperties.Length; i++)
        {
            if (Input.GetKey(attackProperties[i].attackKey) && attackProperties[i].CanAttack())
            {
                PerformAttack(attackProperties[i]);
            }
        }
    }

    private void PerformAttack(AttackProperties attack)
    {
        if(attack.attackType == AttackType.Melee)
        {
            PerformMeleeAttack(attack);
        }

        if(attack.attackType == AttackType.Ranged)
        {
            PerformRangedAttack(attack);
        }
    }

    private void PerformMeleeAttack(AttackProperties attack)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attack.attackRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit != null && hit.tag != "Player")
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attack.damageAmount, attack.damageType);
                }
            }
        }
    }

    private void PerformRangedAttack(AttackProperties attack)
    {
        RangedAttack rangedAttack = attack.rangedAttack;
        if(rangedAttack == null) {
            return;
        }

        Vector3 shootDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        GameObject projectile = Instantiate(rangedAttack.projectilePrefab, transform.position, Quaternion.identity);

        Projectile pj = projectile.GetComponent<Projectile>();
        pj.targetTag = "Enemy";
        pj.damageAmount = attack.damageAmount;
        pj.damageType = attack.damageType;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(shootDirection * rangedAttack.shootForce, ForceMode2D.Impulse);
        StartCoroutine(DestroyProjectileAfterTime(projectile, rangedAttack.maxDistance / rangedAttack.shootForce));
    }

    private IEnumerator DestroyProjectileAfterTime(GameObject projectile, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(projectile);
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
