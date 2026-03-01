using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMngr : MonoBehaviour
{
    private static GameMngr inst;

    public static string prevScene = "VSMixingBowl";

    [SerializeField] private bool _retryScene;

    [SerializeField] private SpriteRenderer _bgObject;
    [SerializeField] private Sprite[] _bgPhaseSprites;

    #region GS
    public static GameMngr Inst { get => inst; set => inst = value; }
    #endregion

    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        if (_retryScene)
            prevScene = SceneManager.GetActiveScene().name;
    }

    [HideInInspector]
    public void SetPhaseBG(int phase)
    {
        Debug.Log(phase);
        _bgObject.sprite = _bgPhaseSprites[phase];
    }
}
