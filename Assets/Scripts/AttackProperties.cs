using UnityEngine;

[System.Serializable]
public class AttackProperties: MonoBehaviour
{
    public RangedAttack rangedAttack;
    public float attackRadius = 1.0f;
    public float attackRate = 1.0f;
    public KeyCode attackKey;
    public DamageType damageType = DamageType.Physical;
    public float damageAmount = 10.0f;
    public AttackType attackType = AttackType.Melee;
    public float nextAttackTime;

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
