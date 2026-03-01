using GraffitiGala;
using NaughtyAttributes;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private ShatterTransition shatterTransitionPrefab;
    [SerializeField] private float baseShatterDelay = 2;
    [SerializeField] private float shaterLifetime;
    [SerializeField] private RectTransform zoomImage;
    [SerializeField] private GameObject fillImage;
    [SerializeField] private float zoomTime;
    [SerializeField] private float zoomMaxScale;
    [SerializeField] private float zoomMinScale;

    private static TransitionManager instance;
    private CameraScreenshot screenshot;

    private bool isTransitioning;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Duplicate transition manager.");
            Destroy(gameObject);
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
        ShatterTransition(SceneManager.GetActiveScene().name);
    }

    public static void ZoomTransition(string sceneName)
    {
        if (instance != null)
        {
            if (instance.isTransitioning) { return; }
            instance.StartCoroutine(instance.ZoomRoutine(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public static void ShatterTransition(string sceneName)
    {
        if (instance != null)
        {
            if(instance.isTransitioning) { return; }
            instance.StartCoroutine(instance.ShatterRoutine(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private IEnumerator ShatterRoutine(string sceneName)
    {
        isTransitioning = true;

        // Screenshot and display the screen fracture.
        screenshot.ScreenshotDrawing();

        ShatterTransition transition = Instantiate(shatterTransitionPrefab, transform);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(() => loadOp.isDone);
        yield return new WaitForSecondsRealtime(baseShatterDelay);

        Debug.Log("SHattering");
        // Shatter the screen.
        transition.Shatter();
        transition.HideBackground();

        isTransitioning = false;
        yield return new WaitForSecondsRealtime(shaterLifetime);

        Destroy(transition.gameObject);
    }

    private IEnumerator ZoomRoutine(string sceneName)
    {
        isTransitioning = true;

        zoomImage.gameObject.SetActive(true);
        SetZoom(zoomMaxScale);
        yield return StartCoroutine(ZoomRoutine(zoomMinScale));

        fillImage.SetActive(true);
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(() => loadOp.isDone);
        fillImage.SetActive(false);

        yield return StartCoroutine(ZoomRoutine(zoomMaxScale));
        zoomImage.gameObject.SetActive(false);

        isTransitioning = false;
    }

    private IEnumerator ZoomRoutine(float targetScale)
    {
        float step = Mathf.Abs((targetScale - zoomImage.localScale.x) / zoomTime * Time.unscaledDeltaTime);
        //Debug.Log(step);

        while (Mathf.Abs(zoomImage.localScale.x - targetScale) > 0.1f)
        {
            SetZoom(Mathf.MoveTowards(zoomImage.localScale.x, targetScale, step));
            yield return null;
        }
        SetZoom(targetScale);
    }

    private void SetZoom(float zoom)
    {
        zoomImage.localScale = Vector3.one * zoom;
    }
}
