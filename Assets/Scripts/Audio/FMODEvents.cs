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
    [SerializeField] public EventReference MainMenu;
    [SerializeField] public EventReference Boss1;
    [SerializeField] public EventReference Boss2;
    [SerializeField] public EventReference Boss3;

    [Header("SFX")]
    [SerializeField] public EventReference PlayerCrack;
    [SerializeField] public EventReference PlayerBecomesYolk;
    [SerializeField] public EventReference PlayerMeleeAttack;
    [SerializeField] public EventReference PlayerRollsAsEgg;
    [SerializeField] public EventReference PlayerMovesAsYolk;
    [SerializeField] public EventReference UIClick;
    [SerializeField] public EventReference BossLaunchProjectile;
    [SerializeField] public EventReference CarrotHits;
    [SerializeField] public EventReference ButterLands;
    [SerializeField] public EventReference SpawnEggMinions;
    [SerializeField] public EventReference EggRoll;
    [SerializeField] public EventReference EggExplodes;
    [SerializeField] public EventReference SpawnButter;
    [SerializeField] public EventReference ContinuallyBurn;
    [SerializeField] public EventReference EvansRequest;
    
   
    public static FMODEvents instance { get; private set; }

    /// <summary>
    /// Creates a singleton instance of this script
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}