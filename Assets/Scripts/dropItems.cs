using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropItems : MonoBehaviour
{
    public List<item> itensToDrop = new List<item>();
    public float dropChance;

    public void Drop()
    {
        float randomValue = Random.value;

        foreach (item item in itensToDrop)
        {
            if (randomValue <= dropChance)
            {
                Instantiate(item, transform.position, Quaternion.identity);
                return;
            }
        }
    }
}
