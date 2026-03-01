/* Author Name:     Cade Naylor           
 * Created Date:    2/27/2026
 * Modified Date:   2/27/2026
 * Description:     A base class to handle Audio
 * */
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private Bus masterBus;
    private Bus sfxBus;
    private Bus musicBus;

    [Range(0, 1), HideInInspector]
    public float MasterVolume = 1f;
    [Range(0, 1), HideInInspector]
    public float SfxVolume = 1f;
    [Range(0, 1), HideInInspector]
    public float MusicVolume= 1f;

    private EventInstance backgroundMusic;


    /// <summary>
    /// Gets a reference to busses
    /// </summary>
    private void Start()
    {
        if (instance != null)
        {
            Debug.Log("h");
            Debug.Log("There is more than one AudioManager in the scene");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        masterBus = RuntimeManager.GetBus("bus:/");
        sfxBus = RuntimeManager.GetBus("bus:/SFX Bus");
        musicBus = RuntimeManager.GetBus("bus:/Music Bus");

        UpdateVolume();
        backgroundMusic = RuntimeManager.CreateInstance(FMODEvents.instance.MainMenu);
        backgroundMusic.start();

        SceneManager.activeSceneChanged += SceneManagerActiveSceneChanged;
    }

    /// <summary>
    /// Run when active scene is changed
    /// </summary>
    /// <param name="unused">Unused?</param>
    /// <param name="nextScene">Appears to be the scene that is changed to</param>
    private void SceneManagerActiveSceneChanged(Scene unused, Scene nextScene)
    {
        switch (nextScene.name)
        {
            case "MainMenu":
                ChangeBackgroundMusic(0);
                break;
            case "VSMixingBowl":
                ChangeBackgroundMusic(1);
                break;
            case "VSFryingPan":
                ChangeBackgroundMusic(2);
                break;
            case "VSEggCooker":
                ChangeBackgroundMusic(3);
                break;
        }
    }

    /// <summary>
    /// Play a one shot clip at the camera 
    /// </summary>
    /// <param name="sound"></param>
    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    /// <summary>
    /// Plays a sound at a provided position
    /// </summary>
    /// <param name="sound">Event reference</param>
    /// <param name="worldPos">Position to play the sound at</param>
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    /// <summary>
    /// Create an instance of the events
    /// </summary>
    /// <param name="eventReference"></param>
    /// <returns></returns>
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        //eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.GetComponent<Transform>(), gameObject.GetComponent<Rigidbody>()));
        return eventInstance;
    }

    /// <summary>
    /// Updates the volumes to match the current volumes
    /// </summary>
    public void UpdateVolume()
    {
        Debug.Log(SfxVolume);
        masterBus.setVolume(MasterVolume);
        sfxBus.setVolume(SfxVolume);
        musicBus.setVolume(MusicVolume);
    }

    /// <summary>
    /// Stops any background music when it's destroyed
    /// </summary>
    private void OnDestroy()
    {
        backgroundMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void SetPhase3()
    {
        backgroundMusic.setParameterByName("Phase3", 1);
    }

    public void ChangeBackgroundMusic(int level)
    {
        backgroundMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        switch (level)
        {
            case 0:
                backgroundMusic = RuntimeManager.CreateInstance(FMODEvents.instance.MainMenu);
                break;
            case 1:
                backgroundMusic = RuntimeManager.CreateInstance(FMODEvents.instance.Boss1);
                break;
            case 2:
                backgroundMusic = RuntimeManager.CreateInstance(FMODEvents.instance.Boss2);
                break;
            case 3:
                backgroundMusic = RuntimeManager.CreateInstance(FMODEvents.instance.Boss3);
                break;
        }

        backgroundMusic.start();
    }
}