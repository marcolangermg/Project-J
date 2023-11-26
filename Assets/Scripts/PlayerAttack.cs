using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public enum AttackType
{
    Melee,
    Ranged
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
                Debug.Log("chegou");
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
            Debug.Log("chegou2");
        }
    }

    private void PerformMeleeAttack(AttackProperties attack)
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
        Vector2 attackEnd = (Vector2)transform.position + direction * attack.attackRadius;

        Debug.DrawLine(transform.position, attackEnd, Color.red, 1f);

        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, attackEnd);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit != null && hit.collider.tag != "Player")
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attack.damageAmount, attack.damageType);
                }
            }
        }
    }

    private void PerformRangedAttack(AttackProperties attack)
    {
        Debug.Log("chegou3");
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
