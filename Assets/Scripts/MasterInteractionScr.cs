using UnityEngine;

public class MasterInteractionScr : MonoBehaviour
{
    public Color allColor = new Color(0f, 0f, 0f, 1f);
    public Interactions interactions;
    public Vector2 target;
    public float moveTime;
    public Interactions.interactionTypes intType;
    // Start is called before the first frame update
    void Start()
    {
        interactions.SetUp(target, moveTime, intType);
    }
    private void OnDrawGizmos()
    {
        Transform doorTrans = interactions.door.transform;
        Gizmos.color = new Color(1f - allColor.r, 1f - allColor.g, 1f - allColor.b, allColor.a);
        Gizmos.DrawLine(doorTrans.position, doorTrans.position + new Vector3(target.x, target.y));
        Gizmos.DrawSphere(doorTrans.position + new Vector3(target.x, target.y), 0.2f);
        Gizmos.DrawLine(new Vector2(doorTrans.position.x - doorTrans.localScale.x / 2, doorTrans.position.y - doorTrans.localScale.y / 2),
            new Vector2(doorTrans.position.x + doorTrans.localScale.x / 2, doorTrans.position.y - doorTrans.localScale.y / 2));
        Gizmos.DrawLine(new Vector2(doorTrans.position.x - doorTrans.localScale.x / 2, doorTrans.position.y - doorTrans.localScale.y / 2),
            new Vector2(doorTrans.position.x - doorTrans.localScale.x / 2, doorTrans.position.y + doorTrans.localScale.y / 2));
        Gizmos.DrawLine(new Vector2(doorTrans.position.x - doorTrans.localScale.x / 2, doorTrans.position.y + doorTrans.localScale.y / 2),
            new Vector2(doorTrans.position.x + doorTrans.localScale.x / 2, doorTrans.position.y + doorTrans.localScale.y / 2));
        Gizmos.DrawLine(new Vector2(doorTrans.position.x + doorTrans.localScale.x / 2, doorTrans.position.y - doorTrans.localScale.y / 2),
            new Vector2(doorTrans.position.x + doorTrans.localScale.x / 2, doorTrans.position.y + doorTrans.localScale.y / 2));
    }
    private void OnValidate()
    {
        interactions.gameObject.GetComponent<SpriteRenderer>().color = allColor;
        interactions.door.GetComponent<SpriteRenderer>().color = allColor;
    }
}
