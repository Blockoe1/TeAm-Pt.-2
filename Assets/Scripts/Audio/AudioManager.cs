/* Author Name:     Cade Naylor           
 * Created Date:    2/27/2026
 * Modified Date:   2/27/2026
 * Description:     A base class to handle Audio
 * */
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private Bus masterBus;
    private Bus sfxBus;
    private Bus musicBus;

    [Range(0, 1)]
    public float MasterVolume;
    [Range(0, 1)]
    public float SfxVolume;
    [Range(0, 1)]
    public float MusicVolume;

    private EventInstance backgroundMusic;

    /// <summary>
    /// Called on the game's start
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one AudioManager in the scene");
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    /// <summary>
    /// Gets a reference to busses
    /// </summary>
    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        //sfxBus = RuntimeManager.GetBus("bus:/SFX");
       // musicBus = RuntimeManager.GetBus("bus:/BGM");

        UpdateVolume();
        backgroundMusic = RuntimeManager.CreateInstance(FMODEvents.instance.GameBGM);
        backgroundMusic.start();
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
        //masterBus.setVolume(masterVolume);
        //sfxBus.setVolume(sfxVolume);
        //musicBus.setVolume(musicVolume);
    }

    /// <summary>
    /// Stops any background music when it's destroyed
    /// </summary>
    private void OnDestroy()
    {
        backgroundMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}