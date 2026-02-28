/* Author Name:     Cade Naylor           
 * Created Date:    2/27/2026
 * Modified Date:   2/27/2026
 * Description:     Stores all Event References
 * */
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] public EventReference GameBGM;

    [Header("SFX")]
    [SerializeField] public EventReference PlayerCrack;
   
    public static FMODEvents instance { get; private set; }

    /// <summary>
    /// Creates a singleton instance of this script
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one FMODEvents in the scene");
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}