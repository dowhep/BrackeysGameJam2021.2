using UnityEngine;

public class PauseScr : MonoBehaviour
{
    public static bool isPause = false;
    [SerializeField] private float slowMoSpeed = 5f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            if (isPause)
            {
                AudioManagerScr.PauseSound("Theme1");
                AudioManagerScr.StopSound("Unpause");
                AudioManagerScr.PlaySound("Pause");
            }
            else
            {
                AudioManagerScr.UnpauseSound("Theme1");
                AudioManagerScr.StopSound("Pause");
                AudioManagerScr.PlaySound("Unpause");
            }
        }
        if (isPause)
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = Mathf.Clamp01(Time.timeScale - slowMoSpeed * Time.unscaledDeltaTime);
                PauseMenu.started = true;
            }
        }
        else
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale = Mathf.Clamp01(Time.timeScale + slowMoSpeed * Time.unscaledDeltaTime);
            }
        }
    }
}
