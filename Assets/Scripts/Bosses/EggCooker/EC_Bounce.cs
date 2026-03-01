using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Egg Cooker")]
public class EC_Bounce : BossAction
{
    [SerializeField] private ProjectileShooter shooter;
    [SerializeField] private float meteorHeight;
    [SerializeField] private float meteorOffset;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private Vector2 bounceAngleRange;

    private Transform target;
    private bool isState;
    private Vector2 rotation;
    private Vector2 direction;

    public override void OnActionBegin()
    {
        base.OnActionBegin();
        Boss.Movement.OnCollide += Bounce;
        target = Boss.Movement.TrackingTarget;
        Boss.Movement.TrackingTarget = null;
        direction = Boss.ToPlayerN;
    }

    public override IEnumerator ActionRoutine()
    {
        isState = true;
        while (isState)
        {
            Boss.Movement.TargetVelocity = direction * bounceSpeed;

            yield return new WaitForFixedUpdate();
        }

    }

    public override void OnActionExit()
    {
        base.OnActionExit();
        Boss.Movement.TargetVelocity = Vector2.zero;
        Boss.Movement.OnCollide -= Bounce;
        isState = false;
        Boss.Movement.TrackingTarget = target;
    }

    private void Bounce(Collision2D obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            //Boss.Movement.Rb.rotation = Boss.Movement.Rb.rotation + 180;
            direction = -direction;
        }
        else
        {
            Vector2 spawnPos = (Vector2)Boss.playerTransform.position + 
                new Vector2(Random.Range(-meteorOffset, meteorOffset), Random.Range(-meteorOffset, meteorOffset));

            //Boss.Movement.Rb.rotation = Mathf.Atan2(Boss.ToPlayerN.y, Boss.ToPlayerN.x) * Mathf.Rad2Deg;
            direction = Boss.ToPlayerN;

            shooter.Shoot(Vector2.down * meteorHeight, meteorHeight,
                spawnPos + Vector2.up * meteorHeight);
        } 
    }
}
