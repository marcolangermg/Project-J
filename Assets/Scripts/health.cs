using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damageAmount, DamageType damageType);
}

public class Health : MonoBehaviour, IDamageable
{
    public float fireResistance = 1.0f;
    public float waterResistance = 1.0f;
    public float earthResistance = 1.0f;
    public float airResistance = 1.0f;
    public float energyResistance = 1.0f;
    public float physicalResistance = 1.0f;

public float currentHealth = 100.0f;

    public void TakeDamage(float damageAmount, DamageType damageType)
    {
        float resistance = 1.0f;
        switch (damageType)
        {
            case DamageType.Fire:
                resistance = fireResistance;
                break;
            case DamageType.Water:
                resistance = waterResistance;
                break;
            case DamageType.Earth:
                resistance = earthResistance;
                break;
            case DamageType.Air:
                resistance = airResistance;
                break;
            case DamageType.Energy:
                resistance = energyResistance;
                break;
            case DamageType.Physical:
                resistance = physicalResistance;
                break;
        }

        float finalDamage = damageAmount * (1.0f - (resistance * 0.01f));
        currentHealth -= Mathf.Round(finalDamage * 100.0f) / 100.0f;

        if (currentHealth <= 0)
        {
            this.gameObject.GetComponent<dropItems>().Drop();
            Destroy(gameObject);
        }
    }
}