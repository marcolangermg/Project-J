using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public RangedAttack rangedAttack;
    public float attackRadius = 1.0f;
    public float attackRate = 1.0f;
    private float nextAttackTime;
    public DamageType damageType = DamageType.Physical;
    public float damageAmount = 10.0f;
    public AttackType attackType = AttackType.Melee;
    public Transform target;

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if(attackType == AttackType.Melee)
        {
            PerformMeleeAttack();
        }

        if(attackType == AttackType.Ranged)
        {
            PerformRangedAttack();
        }
    }

    private void PerformMeleeAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (Collider2D hit in hitColliders)
        {

            if (hit != null && hit.tag == "Player")
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageAmount, damageType);
                }
            }
        }
    }

    private void PerformRangedAttack()
    {
        if(rangedAttack == null) {
            return;
        }

        Vector3 shootDirection = (target.position - transform.position).normalized;
        GameObject projectile = Instantiate(rangedAttack.projectilePrefab, transform.position, Quaternion.identity);
        
        Projectile pj = projectile.GetComponent<Projectile>();
        pj.targetTag = "Player";
        pj.damageAmount = damageAmount;
        pj.damageType = damageType;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(shootDirection * rangedAttack.shootForce, ForceMode2D.Impulse);
        StartCoroutine(DestroyProjectileAfterTime(projectile, rangedAttack.maxDistance / rangedAttack.shootForce));
    }

    private IEnumerator DestroyProjectileAfterTime(GameObject projectile, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(projectile);
    }

}
