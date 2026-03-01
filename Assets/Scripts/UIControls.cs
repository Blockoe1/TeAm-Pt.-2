/* Author Name:     Cade Naylor           
 * Created Date:    2/27/2026
 * Modified Date:   2/27/2026
 * Description:     Controls UI functionality
 * */
using UnityEditor;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _levelSelect;


    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    /// <summary>
    /// Sets initial canvas states
    /// </summary>

    public void TransitionToScene(string scene)
    {
        TransitionManager.ZoomTransition(scene);
    }


    private void Start()
    {
        _mainCanvas.SetActive(true);
        _credits.SetActive(false);
        _options.SetActive(false);
    }

    public void CreditClick()
    {
        _credits.SetActive(true);
        _mainCanvas.SetActive(false);
    }

    public void OptionsClick()
    {
        _options.SetActive(true);
        _mainCanvas.SetActive(false);
    }

    public void LevelSelectClick()
    {
        _levelSelect.SetActive(true);
        _mainCanvas.SetActive(false);
    }

    public void Back()
    {
        _credits.SetActive(false);
        _options.SetActive(false);
        _levelSelect.SetActive(false);
        _mainCanvas.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
