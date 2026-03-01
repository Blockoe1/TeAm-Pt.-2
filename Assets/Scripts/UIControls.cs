/* Author Name:     Cade Naylor           
 * Created Date:    2/27/2026
 * Modified Date:   2/27/2026
 * Description:     Controls UI functionality
 * */
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : MonoBehaviour
{
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _options;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private Toggle tobyToggle;


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

        if (!PlayerPrefs.HasKey("toby")) PlayerPrefs.SetString("toby", "F");
        tobyToggle.isOn = PlayerPrefs.GetString("toby") == "T";

        masterSlider.value = AudioManager.instance.MasterVolume;
        sfxSlider.value = AudioManager.instance.SfxVolume;
        musicSlider.value = AudioManager.instance.MusicVolume;
    }

    public void CreditClick()
    {
        ClickAnything();
        _credits.SetActive(true);
        _mainCanvas.SetActive(false);
    }

    public void OptionsClick()
    {
        ClickAnything();
        _options.SetActive(true);
        _mainCanvas.SetActive(false);
    }

    public void Back()
    {
        ClickAnything();
        _credits.SetActive(false);
        _options.SetActive(false);
        _mainCanvas.SetActive(true);
    }

    public void TobyToggle()
    {
        PlayerPrefs.SetString("toby", tobyToggle.isOn ? "T" : "F");
    }

    private void ClickAnything()
    {
        if(FindFirstObjectByType<AudioManager>()!=null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
        }
    }
    public void Quit()
    {
        ClickAnything();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }
}
