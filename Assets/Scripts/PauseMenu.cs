using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausedMenuUI;
    public GameObject OptionMenuUI;
    public static bool started = true;

    void Update()
    {
        Paused();
    }

    public void BackBtn(){
        AudioManagerScr.StopSound("Theme1");
        SceneManager.LoadScene("Level Selector");
        PauseScr.isPause = false;
        GameMaster.Reset();
    }
    public void RestartBtn()
    {
        AudioManagerScr.StopSound("Theme1");
        PauseScr.isPause = false;
        LevelLoaderScr.Instance.ReloadLevel();
    }
    public void ResumeBtn()
    {
        PauseScr.isPause = false;
    }
    void Paused(){
        if(PauseScr.isPause){
            if(started){
                PausedMenuUI.SetActive(true);
            }
            started = false;
        }
        else{
            PausedMenuUI.SetActive(false);
            OptionMenuUI.SetActive(false);
        }
    }
}
