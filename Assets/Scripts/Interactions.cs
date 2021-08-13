using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public GameObject door;

    private Vector2 target;
    private float moveTime;
    private interactionTypes intType;

    private Vector2 originPos;
    private float currTime;
    private bool isActivated = false;

    public void SetUp(Vector2 vector, float move, interactionTypes types)
    {
        target = vector;
        moveTime = move;
        intType = types;
    }
    private void Start()
    {
        originPos = door.transform.localPosition;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveTime <= 0.1f)
        {
            door.transform.localPosition = isActivated ? target : originPos;
            return;
        }
        if (isActivated)
        {
            currTime = Mathf.Min(currTime + Time.fixedDeltaTime, moveTime);
        } else
        {
            currTime = Mathf.Max(currTime - Time.fixedDeltaTime, 0f);
        }
        door.transform.localPosition = (currTime / moveTime) * target + (1f - currTime / moveTime) * originPos;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (intType)
            {
                case interactionTypes.Destroy:
                    Destroy(transform.parent.gameObject);
                    break;
                case interactionTypes.Hold:
                    isActivated = true;
                    break;
                case interactionTypes.Switch:
                    isActivated = !isActivated;
                    break;
                default:
                    break;
            }
            AudioManagerScr.PlaySound("Door");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && intType == interactionTypes.Hold)
        {
            isActivated = false;
        }
    }
    public enum interactionTypes
    {
        Destroy,
        Hold,
        Switch
    }
}
