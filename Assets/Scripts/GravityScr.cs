using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScr : MonoBehaviour
{
    public float gravityChange = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = gravityChange;
        }
    }
}
