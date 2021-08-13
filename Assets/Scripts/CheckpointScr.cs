using System;
using UnityEngine;


public class CheckpointScr : MonoBehaviour
{
    public Color ResetColor = new Color(1f, 1f, 0.63f, 0.8f);
    public Color CPedColor = new Color(1f, 1f, 0.63f, 0.2f);

    private SpriteRenderer spriterenderer;
    private bool isActivated = false;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().color = ResetColor;
    }

    private void Start()
    {
        GameMaster.checkpoints.Add(this);
        spriterenderer = GetComponent<SpriteRenderer>();
        ResetCP();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated)
        {
            GameObject GO = collision.gameObject;
            if (GO.CompareTag("Player"))
            {
                ActivateCP();
                GameMaster.Checkpointed(this, transform.position,
                    GO.GetComponent<MovementScr>().characterAccesses,
                    GO.GetComponent<Rigidbody2D>().gravityScale,
                    GO.GetComponent<MovementScr>().whichCharIsOn);
            }
        }
    }

    private void ActivateCP()
    {
        isActivated = true;
        spriterenderer.color = CPedColor;
    }

    public void ResetCP()
    {
        isActivated = false;
        spriterenderer.color = ResetColor;
    }

    // unused for now
    private Color SetColorAlpha(Color color, float v)
    {
        Color temp = color;
        temp.a = v;
        return temp;
    }
}
