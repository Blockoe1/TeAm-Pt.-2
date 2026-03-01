using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    public void OnMainMenu()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuit()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
        }
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
