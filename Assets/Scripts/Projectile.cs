using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount = 0.0f;
    public DamageType damageType = DamageType.Physical;
    public string targetTag;

    private Vector3 startPosition;

    void Start()
    {
        BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Health health = other.GetComponent<Health>();
            health.TakeDamage(damageAmount, damageType);
            Destroy(gameObject);
        }
    }
}
