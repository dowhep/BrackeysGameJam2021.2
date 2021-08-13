using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScr : MonoBehaviour
{
    public GameObject messageTxt;

    private void Start()
    {
        messageTxt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            messageTxt.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            messageTxt.SetActive(false);
        }
    }
}
