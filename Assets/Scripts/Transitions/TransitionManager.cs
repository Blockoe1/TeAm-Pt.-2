using GraffitiGala;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private ShatterTransition shatterTransitionPrefab;
    [SerializeField] private float shaterLifetime;

    private static TransitionManager instance;
    private CameraScreenshot screenshot;

    private bool isTransitioning;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Duplicate transition manager.");
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            screenshot = GetComponent<CameraScreenshot>();
            instance = this;
        }
    }

    [Button]
    private void DebugTransition()
    {
        TransitionToScene(SceneManager.GetActiveScene().name);
    }

    public static void TransitionToScene(string sceneName)
    {
        if (instance != null)
        {
            if(instance.isTransitioning) { return; }
            instance.StartCoroutine(instance.TransitionRoutine(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        isTransitioning = true;

        // Screenshot and display the screen fracture.
        screenshot.ScreenshotDrawing();

        ShatterTransition transition = Instantiate(shatterTransitionPrefab, transform);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(() => loadOp.isDone);

        Debug.Log("SHattering");
        // Shatter the screen.
        transition.Shatter();
        transition.HideBackground();

        isTransitioning = false;
        yield return new WaitForSecondsRealtime(shaterLifetime);

        Destroy(transition.gameObject);
    }
}
