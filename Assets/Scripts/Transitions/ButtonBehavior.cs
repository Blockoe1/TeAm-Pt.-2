using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LevelOne()
    {
        SceneManager.LoadScene("VSMixingBowl");
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene("VSFryingPan");
    }

    public void LevelThree()
    {
        SceneManager.LoadScene("VSEggCooker");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
