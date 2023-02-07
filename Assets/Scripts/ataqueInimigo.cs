using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ataqueInimigo : MonoBehaviour
{
    public float attackForce = 10.0f;
    public float attackRadius = 1.0f;
    public float attackRate = 1.0f;
    private float nextAttackTime;

    public DamageType damageType = DamageType.Physical;
    public float damageAmount = 10.0f;

    void Start()
    {
        
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
}
