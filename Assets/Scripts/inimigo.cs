using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;
    public float maintainDistance = 2.0f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > maintainDistance)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        }
        if (distance < maintainDistance)
        {
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position = Vector3.Lerp(transform.position, transform.position + direction, speed * Time.deltaTime);
        }
    }
}
