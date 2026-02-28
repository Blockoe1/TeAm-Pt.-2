using UnityEngine;
[RequireComponent (typeof(Rigidbody2D))]
public class BossMovement : MonoBehaviour
{
    #region CONSTS
    private const float MOVE_LEEWAY = 0.5f;
    #endregion

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;

    private Vector2 moveTarget;
    public Vector2 TargetVelocity { get; set; }
    private bool isMovingToPos;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetMoveTarget(Vector2 pos)
    {
        moveTarget = pos;
        isMovingToPos = true;
    }

    private void FixedUpdate()
    {
        if (isMovingToPos)
        {
            Vector2 direction = moveTarget - rb.position;
            TargetVelocity = direction.normalized * maxSpeed;

            if (direction.magnitude < MOVE_LEEWAY)
            {
                TargetVelocity = Vector2.zero;
                isMovingToPos = false;
            }
        }

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, TargetVelocity, acceleration);
    }
}
