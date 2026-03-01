using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMngr : MonoBehaviour
{
    private static GameMngr inst;

    public static string prevScene = "VSMixingBowl";

    [SerializeField] private bool _retryScene;

    private InputAction escAction;

    public bool GamePaused = false;
    [SerializeField] private GameObject canvas;

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
        escAction = InputSystem.actions.FindAction("ESCAPE");
        if (_retryScene)
            prevScene = SceneManager.GetActiveScene().name;

        escAction.performed += EscAction_performed;
    }

    private void EscAction_performed(InputAction.CallbackContext obj)
    {
        GamePaused = !GamePaused;
        if(GamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
            canvas.SetActive(GamePaused);
    }

    [HideInInspector]
    public void SetPhaseBG(int phase)
    {
        if (phase < 0) return;
        _bgObject.sprite = _bgPhaseSprites[phase];
    }
}
