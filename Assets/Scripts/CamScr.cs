using UnityEngine;
using UnityEngine.UI;

public class CamScr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerGO;
    public Button toggleBtn;
    public Slider zoomSlider;
    public Vector3 camOffset = new Vector3(0, 0, -10);
    public float smoothTime = 50f;
    public float freecamSmoothTime = 15f;
    public float lookAhead = 0.5f;
    public float mouseSensitivity = 6f;
    public float defaultShake = 1f;
    public float freecamLimit = 500f;
    public float minCamSize = 5f;
    public float maxCamSize = 20f;
    public float defaultCamSize = 10f;
    public float zoomSpeed = 1f;
    public float smoothZoom = 1f;

    [SerializeField]
    private float returnSpeed = 1f;

    public static bool isFreecaming { get; private set; } = false;

    private Camera cam;
    private Transform playerTrans;
    private Rigidbody2D rig;
    private Vector3 refVel;
    private Vector3 startPos;
    private float targetCamSize;
    void Start()
    {
        isFreecaming = false;
        cam = GetComponent<Camera>();
        playerTrans = playerGO.GetComponent<Transform>();
        refVel = (playerTrans.position + camOffset - transform.position) * returnSpeed;
        rig = playerGO.GetComponent<Rigidbody2D>();
        targetCamSize = defaultCamSize;
        zoomSlider.gameObject.SetActive(false);
        toggleBtn.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseScr.isPause)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                toggleFreecam();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (isFreecaming)
                {
                    toggleFreecam();
                }
            }
        }
        Cursor.lockState = (isFreecaming && !PauseScr.isPause) ? CursorLockMode.Locked : CursorLockMode.None;
    }
    public void toggleFreecam()
    {
        isFreecaming = !isFreecaming;

        if (isFreecaming)
        {
            startPos = transform.position;
        } else
        {
            targetCamSize = defaultCamSize;
            refVel = (playerTrans.position + camOffset - transform.position) * returnSpeed;
        }

        zoomSlider.gameObject.SetActive(isFreecaming);
        toggleBtn.gameObject.SetActive(!isFreecaming);
    }
    private void LateUpdate()
    {
        if (isFreecaming)
        {
            Vector3 change = new Vector3(Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * targetCamSize,
                Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * targetCamSize);
            startPos += change;
            startPos.x = Mathf.Clamp(startPos.x, playerTrans.position.x - freecamLimit, playerTrans.position.x + freecamLimit);
            startPos.y = Mathf.Clamp(startPos.y, playerTrans.position.y - freecamLimit, playerTrans.position.y + freecamLimit);
            smoothMoveCam(startPos, freecamSmoothTime * Time.deltaTime);
            targetCamSize = Mathf.Clamp(targetCamSize + zoomSpeed * Input.mouseScrollDelta.y, minCamSize, maxCamSize);
        }
    }
    private void FixedUpdate()
    {
        if (!isFreecaming)
        {
            Vector3 randomVector = new Vector3(Random.value - 0.5f, Random.value - 0.5f) * defaultShake;
            Vector3 desiredPosition = new Vector3(rig.velocity.x * lookAhead, rig.velocity.y * lookAhead) + playerTrans.position + camOffset + randomVector;
            smoothMoveCam(desiredPosition, smoothTime * Time.fixedDeltaTime);
        }
        zoom();
    }

    private void smoothMoveCam(Vector3 target, float timeSmooth)
    {
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, target, ref refVel, timeSmooth);
        transform.position = smoothedPosition;
    }

    private void zoom()
    {
        float smoothedZoom = cam.orthographicSize + Time.fixedDeltaTime * (targetCamSize - cam.orthographicSize) * smoothZoom;
        cam.orthographicSize = smoothedZoom;
        zoomSlider.value = targetCamSize * 10;
    }
}
