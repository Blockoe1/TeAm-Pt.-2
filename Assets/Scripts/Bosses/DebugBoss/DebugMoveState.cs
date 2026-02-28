using System.Collections;
using UnityEngine;

[System.Serializable]
public class DebugMoveState : BossState
{
    [SerializeField] private ProjectileShooter shooter;
    [SerializeField] private Transform[] moveTargets;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Boss.movement.OnReachPoint += Shoot;
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        Boss.movement.OnReachPoint -= Shoot;
    }

    public override IEnumerator StateRoutine()
    {
        while (true)
        {
            //yield return new WaitForFixedUpdate();
            Boss.movement.SetMoveTarget(moveTargets[Random.Range(0, moveTargets.Length)].position);
            yield return new WaitForSeconds(5f);
        }
    }

    private void Shoot(Vector2 reachedPoint)
    {
        Vector2 shootVector = Boss.playerTransform.position - Boss.transform.position;
        shooter.Launch(shootVector, 5, 8, 360);
    }
}
