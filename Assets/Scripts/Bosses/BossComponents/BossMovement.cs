using System;
using UnityEngine;
[RequireComponent (typeof(Rigidbody2D))]
public class BossMovement : MonoBehaviour
{
    #region CONSTS
    private const float MOVE_LEEWAY = 0.5f;
    #endregion

    [SerializeField] private Transform trackingTarget;
    [SerializeField] private float toTargetSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float angleSmoothTime;
    [SerializeField] private float angleMaxSpeed;

    private Vector2 moveTarget;
    public Vector2 TargetVelocity { get; set; }
    private bool isMovingToPos;
    private float angleVelocity;

    private Rigidbody2D rb;

    public event Action<Vector2> OnReachPoint;

    #region Properties
    public Rigidbody2D Rb => rb;
    public Transform TrackingTarget
    {
        get { return trackingTarget; }
        set { trackingTarget = value; }
    }
    #endregion

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
        if (trackingTarget != null)
        {
            Vector2 trackTargetTo = (Vector2)trackingTarget.position - rb.position;
            // Make the boss point towards the tracked target.
            float targetAngle = Mathf.Atan2(trackTargetTo.y, trackTargetTo.x) * Mathf.Rad2Deg;
            rb.rotation = Mathf.SmoothDampAngle(rb.rotation, targetAngle, ref angleVelocity, angleSmoothTime, angleMaxSpeed);
        }

        Vector2 targetVelocity;
        if (isMovingToPos)
        {
            Vector2 direction = moveTarget - rb.position;
            targetVelocity = direction.normalized * toTargetSpeed;

            if (direction.magnitude < MOVE_LEEWAY)
            {
                TargetVelocity = Vector2.zero;
                OnReachPoint?.Invoke(moveTarget);
                isMovingToPos = false;
            }
        }
        else
        {

            targetVelocity = Quaternion.Euler(0, 0, rb.rotation) * TargetVelocity;
        }
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }

    public void SnapToTarget()
    {
        if (trackingTarget != null)
        {
            Vector2 trackTargetTo = (Vector2)trackingTarget.position - rb.position;
            rb.rotation = Mathf.Atan2(trackTargetTo.y, trackTargetTo.x) * Mathf.Rad2Deg;
        }
    }
}
