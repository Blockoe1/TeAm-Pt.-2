using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PMoveStateMngr : MonoBehaviour
{
    private static PMoveStateMngr inst;

    private InputAction move;
    private Vector2 moveDirection;
    private Vector2 faceDirection = Vector2.right;

    private InputAction dash;
    private bool isDashing;

    [Header("Egg State")]
    [SerializeField][MinValue(0)] private float _eggMoveSpeed;
    [SerializeField][MinValue(0)] private float _accelerationSpeed;
    [SerializeField][MinValue(0)] private float _deccelerationSpeed;
    [SerializeField][MinValue(0)] private float _eggDashSpeed;
    [SerializeField] private AnimatorOverrideController _eggAnimOC;

    [Header("Yolk State")]
    [SerializeField][MinValue(0)] private float _yolkMoveSpeed;
    [SerializeField][MinValue(0)] private float _yolkDashSpeed;
    [SerializeField][MinValue(0)] private float _yolkDashDuration;
    [SerializeField] private AnimatorOverrideController _yolkAnimOC;

    private Rigidbody2D rb2d;
    private Animator anim;

    private PMoveEggState eggState;
    private PMoveYolkState yolkState;

    private PMoveBaseSt currentSt;

    #region GS
    public Rigidbody2D Rb2d { get => rb2d; set => rb2d = value; }
    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public float EggMoveSpeed { get => _eggMoveSpeed; set => _eggMoveSpeed = value; }
    public float AccelerationSpeed { get => _accelerationSpeed; set => _accelerationSpeed = value; }
    public float DeccelerationSpeed { get => _deccelerationSpeed; set => _deccelerationSpeed = value; }
    public InputAction Dash { get => dash; set => dash = value; }
    public float EggDashSpeed { get => _eggDashSpeed; set => _eggDashSpeed = value; }
    public float YolkMoveSpeed { get => _yolkMoveSpeed; set => _yolkMoveSpeed = value; }
    public float YolkDashSpeed { get => _yolkDashSpeed; set => _yolkDashSpeed = value; }
    public float YolkDashDuration { get => _yolkDashDuration; set => _yolkDashDuration = value; }
    public static PMoveStateMngr Inst { get => inst; set => inst = value; }
    public PMoveYolkState YolkState { get => yolkState; set => yolkState = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public InputAction Move { get => move; set => move = value; }
    public AnimatorOverrideController YolkAnimOC { get => _yolkAnimOC; set => _yolkAnimOC = value; }
    public AnimatorOverrideController EggAnimOC { get => _eggAnimOC; set => _eggAnimOC = value; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public Vector2 FaceDirection { get => faceDirection; set => faceDirection = value; }
    #endregion

    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        move = InputSystem.actions.FindAction("MOVE");
        dash = InputSystem.actions.FindAction("DASH");

        eggState = new PMoveEggState(this);
        yolkState = new PMoveYolkState(this);

        currentSt = eggState;
        currentSt.EnterState();
    }

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        if (Mathf.Abs(moveDirection.x) > 0.5f || Mathf.Abs(moveDirection.y) > 0.5f)
            faceDirection = moveDirection;

        DetermineAnimation();

        currentSt.FixedUpdateState();
    }

    [HideInInspector]
    public void SwitchState(PMoveBaseSt state)
    {
        state.ExitState();
        currentSt = state;
        currentSt.EnterState();
    }

    public void Buttered()
    {
        AccelerationSpeed *= .5f;
        DeccelerationSpeed *= .5f;
    }

    private void DetermineAnimation()
    {
        anim.SetInteger("MOVE_DIRECTION_ID", 0); //Side

        Debug.Log(moveDirection);
        if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y)) //Front/Back
            anim.SetInteger("MOVE_DIRECTION_ID", (moveDirection.y > 0)? -1 : 1);
    }
}
