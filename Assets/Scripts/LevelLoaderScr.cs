using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScr : MonoBehaviour
{
    public static LevelLoaderScr Instance { get; private set; }
    public Animator transition;
    public float transitionTime = 1f;
    public float deathRespawnDelay = 1f;
    public float finalFinishTransitionTime = 6f;

    private bool isLoading = false;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AudioManagerScr.PlaySound("Theme1");
    }
    public void LoadNextLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadLevel(int levelIndex)
    {
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadLevelCoroutine(levelIndex));
        }
    }
    public void LoadLevelSelector()
    {
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadLevelSelectorCoroutine());
        }
    }
    public void LoadFinalFinish()
    {
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(FinalFinishCoroutine());
        }
    }
    public void ReloadLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void DeathReloadLevel()
    {
        Invoke("ReloadLevel", deathRespawnDelay);
    }
    IEnumerator LoadLevelCoroutine(int levelIndex)
    {
        transition.SetTrigger("Start");
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
        operation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(transitionTime);
        operation.allowSceneActivation = true;
        if (!operation.isDone)
        {
            yield return null;
        }
    }IEnumerator LoadLevelSelectorCoroutine()
    {
        transition.SetTrigger("Start");
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level Selector", LoadSceneMode.Single);
        operation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(transitionTime);
        operation.allowSceneActivation = true;
        if (!operation.isDone)
        {
            yield return null;
        }
    }
    IEnumerator FinalFinishCoroutine()
    {
        transition.SetTrigger("FinalFinish");
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level Selector", LoadSceneMode.Single);
        operation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(finalFinishTransitionTime);
        operation.allowSceneActivation = true;
        if (!operation.isDone)
        {
            yield return null;
        }
    }
}
