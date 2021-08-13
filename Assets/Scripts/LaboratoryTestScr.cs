using UnityEngine;
using UnityEngine.SceneManagement;

public class LaboratoryTestScr : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelLoaderScr.Instance.LoadLevel(1 - SceneManager.GetActiveScene().buildIndex);
        }
    }
}
