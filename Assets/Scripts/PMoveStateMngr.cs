using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PMoveStateMngr : MonoBehaviour
{
    private InputAction move;
    private Vector2 moveDirection;

    private InputAction dash;
    private Vector2 faceDirection = Vector2.one;

    [Header("Roll State")]
    [SerializeField][MinValue(0)] private float _rollSpeed;
    [SerializeField][MinValue(0)] private float _accelerationSpeed;
    [SerializeField][MinValue(0)] private float _deccelerationSpeed;

    [Header("Dash")]
    [SerializeField][MinValue(0)] private float _rollDashSpeed;


    private Rigidbody2D rb2d;

    private PMoveRollSt rollSt;

    private PMoveBaseSt currentSt;

    #region GS
    public Rigidbody2D Rb2d { get => rb2d; set => rb2d = value; }
    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public float RollSpeed { get => _rollSpeed; set => _rollSpeed = value; }
    public float AccelerationSpeed { get => _accelerationSpeed; set => _accelerationSpeed = value; }
    public float AccelerationSpeed1 { get => _accelerationSpeed; set => _accelerationSpeed = value; }
    public float DeccelerationSpeed { get => _deccelerationSpeed; set => _deccelerationSpeed = value; }
    public InputAction Dash { get => dash; set => dash = value; }
    public float RollDashSpeed { get => _rollDashSpeed; set => _rollDashSpeed = value; }
    public Vector2 FaceDirection { get => faceDirection; set => faceDirection = value; }
    #endregion

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        move = InputSystem.actions.FindAction("MOVE");
        dash = InputSystem.actions.FindAction("DASH");

        rollSt = new PMoveRollSt(this);

        currentSt = rollSt;
        currentSt.EnterState();
    }

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();

        if (moveDirection.x != 0 && moveDirection.y != 0)
            faceDirection = moveDirection;

        currentSt.FixedUpdateState();
    }

    private void Update()
    {
        Rotate();
    }

    [HideInInspector]
    public void SwitchState(PMoveBaseSt state)
    {
        state.ExitState();
        currentSt = state;
        currentSt.EnterState();
    }

    private void Rotate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.up = mousePos - new Vector2(transform.position.x, transform.position.y);
    }
}
