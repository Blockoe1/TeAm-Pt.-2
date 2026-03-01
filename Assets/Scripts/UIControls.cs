/* Author Name:     Cade Naylor           
 * Created Date:    2/27/2026
 * Modified Date:   2/27/2026
 * Description:     Controls UI functionality
 * */
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControls : MonoBehaviour
{
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _levelSelect;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;


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

    public void LevelSelect()
    {
        ClickAnything();
        _levelSelect.SetActive(true);
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
        _levelSelect.SetActive(false);
        _mainCanvas.SetActive(true);
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


    private void ClickAnything()
    {
        if(FindFirstObjectByType<AudioManager>()!=null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
        }
    }


    public void UpdateAudio()
    {
        AudioManager.instance.MasterVolume = masterSlider.value;
        AudioManager.instance.MusicVolume = sfxSlider.value;
        AudioManager.instance.SfxVolume = musicSlider.value;
        AudioManager.instance.UpdateVolume();
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
