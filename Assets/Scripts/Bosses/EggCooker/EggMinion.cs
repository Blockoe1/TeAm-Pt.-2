using UnityEngine;

public class EggMinion : Projectile
{
    [SerializeField] private float acceleration;
    [SerializeField] private float angleSmoothTime;
    [SerializeField] private float angleMaxSpeed;

    private float travelSpeed;
    private float angleVelocity;
    private float angle;
    private Transform trackingTarget;
    private BossHealth health;

    private void Awake()
    {
        health = GetComponent<BossHealth>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            trackingTarget = player.transform;
        }
        
    }

    public override void Launch(Vector2 launchVector)
    {
        base.Launch(launchVector);
        travelSpeed = launchVector.magnitude;
        health.ResetHealth();
    }

    private void FixedUpdate()
    {
        if (trackingTarget != null)
        {
            Vector2 trackTargetTo = (Vector2)trackingTarget.position - rb.position;
            // Make the boss point towards the tracked target.
            float targetAngle = Mathf.Atan2(trackTargetTo.y, trackTargetTo.x) * Mathf.Rad2Deg;
            angle = Mathf.SmoothDampAngle(angle, targetAngle, ref angleVelocity, angleSmoothTime, angleMaxSpeed);
        }

        Vector2 targetVelocity;
        targetVelocity = Quaternion.Euler(0, 0, angle) * Vector2.right * travelSpeed;
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (despawnOnCollision && collision.CompareTag("Player"))
        {
            // Egg minions only despawn on contact with player.
            Despawn();
        }
    }
}
