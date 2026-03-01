using System;
using UnityEngine;
using FMODUnity;
using System.Collections;
using FMOD.Studio;
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

    private Coroutine sillySolution;

    [SerializeField] public EventReference MovementSound;
    [SerializeField] public EventReference Ambience;
    private EventInstance ambience;

    private Vector2 moveTarget;
    public Vector2 TargetVelocity { get; set; }
    private bool isMovingToPos;
    private float angleVelocity;

    private Rigidbody2D rb;

    public event Action<Vector2> OnReachPoint;

    public event Action<Collision2D> OnCollide;

    private Coroutine sounds;

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
        if (sounds == null)
        {
            sounds = StartCoroutine(BossNoise());
        }
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
            if (sillySolution == null)
            {
                sillySolution = StartCoroutine(PlaySound());
            }

        }
        else
        {

            targetVelocity = Quaternion.Euler(0, 0, rb.rotation) * TargetVelocity;

            if (sillySolution != null)
            {
                StopCoroutine(sillySolution);
                sillySolution = null;
            }
        }
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }

    public void SnapRotation()
    {
        if (trackingTarget != null)
        {
            Vector2 trackTargetTo = (Vector2)trackingTarget.position - rb.position;
            rb.rotation = Mathf.Atan2(trackTargetTo.y, trackTargetTo.x) * Mathf.Rad2Deg;
        }
    }

    /// <summary>
    /// This is so scuffed but it works
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlaySound()
    {
        while (true)
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayOneShot(MovementSound);
            }

            yield return new WaitForSeconds(.1f);
        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollide?.Invoke(collision);
    }

    private void OnDestroy()
    {
        rb.linearVelocity = Vector2.zero;
        StopCoroutine(sounds);
        ambience.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }


    private IEnumerator BossNoise()
    {
        ambience = RuntimeManager.CreateInstance(Ambience);
        while(true)
        {
            ambience.start();
            yield return new WaitForSeconds(UnityEngine.Random.Range(.3f, 10f));
            ambience.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            yield return new WaitForSeconds(UnityEngine.Random.Range(.3f, 10f));
        }
    }

}
