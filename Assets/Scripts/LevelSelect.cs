using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManagerScr.PlaySound("LevelSelector");
    }
    private void OnDestroy()
    {
        AudioManagerScr.StopSound("LevelSelector");
    }

    public void BackBtn(){
        SceneManager.LoadScene("Main Menu");
    }
    
    public void LevelBtn(int i){
        SceneManager.LoadScene("Level " + i);
    }

    public void TestBtn(){
        SceneManager.LoadScene("Test");
    }

    public void LabBtn()
    {
        SceneManager.LoadScene("Laboratory");
    }

    public void CPSTestBtn()
    {
        SceneManager.LoadScene("CPSTest");
    }
}
