using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMngr : MonoBehaviour
{
    public static string prevScene = "VSMixingBowl";

    [SerializeField] private bool _retryScene;

    private void Start()
    {
        if (_retryScene)
            prevScene = SceneManager.GetActiveScene().name;
    }
}
