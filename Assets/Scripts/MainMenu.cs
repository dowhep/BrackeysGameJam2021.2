using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider slider;
    public bool isMainMenu = true;
    public void ValueChangeCheck()
    {
        AudioManagerScr.UpdateVolume(slider.value);
    }
    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Volume", 1f);
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        if (isMainMenu)
        {
            AudioManagerScr.PlaySound("MainMenu");
        }
    }
    private void OnDestroy()
    {
        AudioManagerScr.StopSound("MainMenu");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level Selector");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
