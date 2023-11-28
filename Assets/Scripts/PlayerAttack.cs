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
    //modificaroes de ataque
    public bool playerAttackModificator_shootReturns;
    public bool playerAttackModificator_shootDisperse;

    private Vector2 lastMoveDirection;

    private void Update()
    {
        GetDirection();
        //aguarda as teclas de ativação da lista de ataques
        for (int i = 0; i < attackProperties.Length; i++)
        {
            if (Input.GetKey(attackProperties[i].attackKey) && attackProperties[i].CanAttack())
            {
                PerformAttack(attackProperties[i]);
            }
        }
    }

    //verifica o tipo de ataque
    private void PerformAttack(AttackProperties attack)
    {
        if (attack.attackType == AttackType.Melee)
        {
            PerformMeleeAttack(attack);
        }

        if (attack.attackType == AttackType.Ranged)
        {
            PerformRangedAttack(attack);
        }
    }

    //movimentação do personagem
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

    //executa um ataque corporal
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

    //executa um ataque de longo alcance
    private void PerformRangedAttack(AttackProperties attack)
    {
        RangedAttack rangedAttack = attack.rangedAttack;
        if (rangedAttack == null)
        {
            return;
        }

        Vector3 shootDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

        //verifica se o poder que divide os projeteis deve ser lançado
        if (rangedAttack.seDivide || playerAttackModificator_shootDisperse)
        {
            float angleOffset = 15f;

            SpawnProjectile(transform.position, Quaternion.Euler(0, 0, 0), shootDirection, rangedAttack, attack);

            SpawnProjectile(transform.position, Quaternion.Euler(0, 0, angleOffset), Quaternion.Euler(0, 0, -angleOffset) * shootDirection, rangedAttack, attack);

            SpawnProjectile(transform.position, Quaternion.Euler(0, 0, -angleOffset), Quaternion.Euler(0, 0, angleOffset) * shootDirection, rangedAttack, attack);
        }
        else
        {
            SpawnProjectile(transform.position, Quaternion.identity, shootDirection, rangedAttack, attack);
        }
    }

    //lança os projeteis
    private void SpawnProjectile(Vector3 spawnPosition, Quaternion rotation, Vector3 shootDirection, RangedAttack rangedAttack, AttackProperties attack)
    {
        GameObject projectile = Instantiate(rangedAttack.projectilePrefab, spawnPosition, rotation);

        Projectile pj = projectile.GetComponent<Projectile>();
        pj.targetTag = "Enemy";
        pj.damageAmount = attack.damageAmount;
        pj.damageType = attack.damageType;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * rangedAttack.shootForce;

        StartCoroutine(MoveAndDestroyProjectile(rb, projectile, rangedAttack.maxDistance, rangedAttack.voltaSpeed, rangedAttack.volta));
    }

    //destroi após alcançar o objetivo
    private IEnumerator MoveAndDestroyProjectile(Rigidbody2D rb, GameObject projectile, float maxDistance, float voltaSpeed, bool mustReturn)
    {
        float initialDistance = Vector2.Distance(transform.position, projectile.transform.position);

        while (Vector2.Distance(transform.position, projectile.transform.position) < maxDistance + initialDistance)
        {
            yield return null;
        }

        //verifica se o poder que retorna o projetil ao player deve ser ativado
        if (mustReturn || playerAttackModificator_shootReturns)
        {
            StartCoroutine(ReturnProjectile(rb, projectile, voltaSpeed));
        }
        else
        {
            Destroy(projectile);
        }
    }

    //move o projetil na direção do player antes de destrui-lo
    private IEnumerator ReturnProjectile(Rigidbody2D rb, GameObject projectile, float voltaSpeed)
    {
        while (Vector2.Distance(transform.position, projectile.transform.position) > 0.1f)
        {
            Vector2 directionToPlayer = (Vector2)transform.position - (Vector2)projectile.transform.position;
            rb.velocity = directionToPlayer.normalized * voltaSpeed;
            yield return null;
        }

        Destroy(projectile);
    }


}
