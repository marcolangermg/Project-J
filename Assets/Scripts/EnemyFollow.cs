using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;
    public float speed = 2f;
    public float distance = 3f;
    public float safeWallDistance = 0.1f;
    private Vector3 direction;
    private float distanceToTarget;

    private void Update()
    {
        target = GameObject.Find("Player").transform;
        direction = (target.position - transform.position).normalized;
        distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Verifica se há colisões com paredes
        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, direction, distanceToTarget, LayerMask.GetMask("Wall"));
        if (hitWall.collider != null)
        {
            float distanceToWall = Vector3.Distance(transform.position, hitWall.point);
            if (distanceToWall < safeWallDistance)
            {
                // Move o inimigo na direção oposta para mantê-lo a uma distância segura da parede
                Vector3 avoidWallDirection = -direction;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + avoidWallDirection, speed * Time.deltaTime);
                return;
            }

            // Calcula a direção para contornar a parede
            Vector3 avoidDirection = new Vector3(-direction.y, direction.x);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + avoidDirection, speed * Time.deltaTime);
            return;
        }

        // Verifica se há colisões com outros inimigos
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject && hit.gameObject.tag == "Enemy")
            {
                // Calcula a direção para contornar o inimigo
                Vector3 avoidDirection = (transform.position - hit.transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + avoidDirection, speed * Time.deltaTime);
                return;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position - (direction * distance), speed * Time.deltaTime);
    }
}
