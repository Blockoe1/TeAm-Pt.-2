using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
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
    [Tooltip("0 = FRONT\n1 = SIDE\n2 = BACK")]
    [SerializeField] private AnimatorOverrideController[] _wholeAnimOCs;
    [Tooltip("0 = FRONT\n1 = SIDE\n2 = BACK")]
    [SerializeField] private AnimatorOverrideController[] _crackedAnimOCs;

    [Header("Yolk State")]
    [SerializeField][MinValue(0)] private float _yolkMoveSpeed;
    [SerializeField][MinValue(0)] private float _yolkDashSpeed;
    [SerializeField][MinValue(0)] private float _yolkDashDuration;
    [Tooltip("0 = FRONT\n1 = SIDE\n2 = BACK")]
    [SerializeField] private AnimatorOverrideController[] _yolkAnimOCs;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRen;
    private Animator anim;

    private AnimatorOverrideController[] curOC;

    private PMoveEggState eggState;
    private PMoveYolkState yolkState;
    protected

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
    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public Vector2 FaceDirection { get => faceDirection; set => faceDirection = value; }
    public AnimatorOverrideController[] YolkAnimOCs { get => _yolkAnimOCs; set => _yolkAnimOCs = value; }
    public SpriteRenderer SpriteRen { get => spriteRen; set => spriteRen = value; }
    public AnimatorOverrideController[] WholeAnimOCs { get => _wholeAnimOCs; set => _wholeAnimOCs = value; }
    public AnimatorOverrideController[] CrackedAnimOCs { get => _crackedAnimOCs; set => _crackedAnimOCs = value; }
    public AnimatorOverrideController[] CurOC { get => curOC; set => curOC = value; }
    #endregion

    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
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

        currentSt.FixedUpdateState();

        anim.runtimeAnimatorController = curOC[DetermineAnimationDirection()];
    }

    [HideInInspector]
    public void SwitchState(PMoveBaseSt state)
    {
        state.ExitState();
        currentSt = state;
        currentSt.EnterState();
    }

    [HideInInspector]
    public bool IsMoving()
    {
        return (Mathf.Abs(rb2d.linearVelocity.x) > 0.25f || Mathf.Abs(rb2d.linearVelocity.y) > 0.25f) ? true : false;
    }

    [HideInInspector]
    public void Buttered()
    {
        AccelerationSpeed *= .5f;
        DeccelerationSpeed *= .5f;
    }

    [HideInInspector]
    public int DetermineAnimationDirection()
    {
        if (Mathf.Abs(faceDirection.x) < Mathf.Abs(faceDirection.y)) //Front/Back
            return (faceDirection.y > 0) ? 2 : 0;
        else //Side
        {
            spriteRen.flipX = (faceDirection.x > 0) ? true : false;
            return 1;
        }
    }
}
