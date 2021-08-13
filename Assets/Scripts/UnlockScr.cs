using UnityEngine;

public class UnlockScr : MonoBehaviour
{
    [SerializeField]
    private bool[] unlockChars = new bool[4];
    [SerializeField]
    private int forcedChar;
    [SerializeField]
    private float altitude = 1f;
    [SerializeField]
    private float period = 1f;
    [SerializeField]
    private float rotationSpeed = 1f;
    private float rotationAngle = 0f;

    private Vector2 origin;

    private void Start()
    {
        origin = transform.position;
    }
    private void FixedUpdate()
    {
        transform.position = new Vector2(0, altitude * Mathf.Sin(Time.time / period)) + origin;
        rotationAngle += rotationSpeed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MovementScr movement = collision.gameObject.GetComponent<MovementScr>();
            movement.characterAccesses = unlockChars;
            if (forcedChar != 0)
            {
                movement.switchToCharacter(forcedChar);
            }
            AudioManagerScr.PlaySound("Unlock");
            Destroy(gameObject);
        }
    }
}
