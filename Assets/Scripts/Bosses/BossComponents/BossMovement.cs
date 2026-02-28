using System;
using UnityEngine;
using FMODUnity;
using System.Collections;
[RequireComponent (typeof(Rigidbody2D))]
public class BossMovement : MonoBehaviour
{
    #region CONSTS
    private const float MOVE_LEEWAY = 0.5f;
    #endregion

    [SerializeField] private Transform trackingTarget;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;

    private Coroutine sillySolution;

    [SerializeField] public EventReference MovementSound;

    private Vector2 moveTarget;
    public Vector2 TargetVelocity { get; set; }
    private bool isMovingToPos;

    private Rigidbody2D rb;

    public event Action<Vector2> OnReachPoint;

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
            rb.rotation = Mathf.Atan2(trackTargetTo.y, trackTargetTo.x) * Mathf.Rad2Deg;
        }

        Vector2 targetVelocity;
        if (isMovingToPos)
        {
            Vector2 direction = moveTarget - rb.position;
            targetVelocity = direction.normalized * maxSpeed;

            if (direction.magnitude < MOVE_LEEWAY)
            {
                TargetVelocity = Vector2.zero;
                OnReachPoint?.Invoke(moveTarget);
                isMovingToPos = false;
                
            }

            if (sillySolution != null)
            {
                StopCoroutine(sillySolution);
                sillySolution = null;
            }
        }
        else
        {

            targetVelocity = Quaternion.Euler(0, 0, rb.rotation) * TargetVelocity;

            if (sillySolution == null)
            {
                sillySolution = StartCoroutine(PlaySound());
            }
        }

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, acceleration);
    }
    public void Charge()
    {
        Debug.Log("Charge");
        transform.LookAt(transform.position);
        rb.AddForce(transform.right * 1000);
    }

    /// <summary>
    /// This is so scuffed but it works
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlaySound()
    {
        AudioManager.instance.PlayOneShot(MovementSound);

        while(true)
        {
            yield return null;
        }


    }
}
