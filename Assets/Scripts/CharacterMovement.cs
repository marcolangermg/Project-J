using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float dashSpeed = 20.0f;
    public float dashDuration = 0.5f;

    private Rigidbody2D rb;
    private float dashTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= dashTime)
        {
            StartCoroutine(Dash(movement));
        }
        else
        {
            rb.velocity = movement * speed;
        }
    }

    IEnumerator Dash(Vector2 direction)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < dashDuration)
        {
            rb.velocity = direction * dashSpeed;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}