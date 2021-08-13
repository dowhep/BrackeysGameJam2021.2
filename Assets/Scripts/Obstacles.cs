using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            Respawn();
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player")
        {
            if (!MovementScr.isFinished)
            {
                AudioManagerScr.StopSound("Theme1");
                AudioManagerScr.PlaySound("Death");
                dead = true;
                collider.gameObject.SetActive(false);
            }
        } 
    }
    void Respawn()
    {
        dead = false;
        LevelLoaderScr.Instance.DeathReloadLevel();
    }
}
