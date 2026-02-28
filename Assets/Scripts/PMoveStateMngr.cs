using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PMoveStateMngr : MonoBehaviour
{
    private InputAction move;
    private Vector2 moveDirection;

    [Header("Roll State")]
    [SerializeField][MinValue(0)] private float _rollSpeed;

    private Rigidbody2D rb2d;

    private PMoveRollSt rollSt;

    private PMoveBaseSt currentSt;

    #region GS
    public Rigidbody2D Rb2d { get => rb2d; set => rb2d = value; }
    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public InputAction Move { get => move; set => move = value; }
    public float RollSpeed { get => _rollSpeed; set => _rollSpeed = value; }
    #endregion

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        move = InputSystem.actions.FindAction("MOVE");

        rollSt = new PMoveRollSt(this);

        currentSt = rollSt;
    }

    private void FixedUpdate()
    {
        currentSt.FixedUpdateState();
    }
}
