using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;
    public float speed = 2f;
    public float distance = 3f;

   private void Update()
    {
        target = GameObject.Find("Player").transform;
        Vector3 direction = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Verifica se há colisões com outros inimigos
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject && hit.gameObject.tag == "Enemy")
            {
                // Calcula a direção para contornar o inimigo
                Vector3 avoidDirection = (transform.position - hit.transform.position).normalized;
                transform.position = Vector3.Lerp(transform.position, transform.position + avoidDirection, speed * Time.deltaTime);
                return;
            }
        }

        transform.position = Vector3.Lerp(transform.position, target.position - (direction * distance), speed * Time.deltaTime);
    }
}
