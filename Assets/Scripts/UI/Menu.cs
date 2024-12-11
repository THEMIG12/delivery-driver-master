using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartFunc()
    {
        Debug.Log("Start");
        SceneManager.LoadScene("TopDown Controller Last");
    }

    public void ExitFunc()
    {
        Application.Quit();
    }
}
