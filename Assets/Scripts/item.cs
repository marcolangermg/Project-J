using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum itemType
{
    Peitoral,
    Capacete,
    Botas,
    Luvas,
    Bracadeiras,
    Perneiras,
    ArmaUmaMao,
    ArmaDuasMaos
}

public class item : MonoBehaviour
{
    public bool onGround = true;
    public Transform player;
    private bool withPlayer;
    public float timeToChange = 2.0f;

    public string itemName;
    public int itemID;
    public itemType itemType;
    public float fireResistance = 0.0f;
    public float waterResistance = 0.0f;
    public float earthResistance = 0.0f;
    public float airResistance = 0.0f;
    public float energyResistance = 0.0f;
    public float physicalResistance = 0.0f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (onGround)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                withPlayer = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (onGround)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                withPlayer = false;
            }
        }
    }

    void Update()
    {
        timeToChange -= Time.deltaTime;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!onGround)
        {
            transform.position = player.position;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            withPlayer = false;
        }
        if (onGround)
        {
            this.gameObject.GetComponent<Collider2D>().enabled = true;
        }

        if (withPlayer)
        {
            if(timeToChange <= 0)
            {
                if (Input.GetKey("e"))
                {
                    player.gameObject.GetComponent<Inventory>().AddItem(this);
                    onGround = false;
                    timeToChange = 1;
                }
            }
        }
    }
}