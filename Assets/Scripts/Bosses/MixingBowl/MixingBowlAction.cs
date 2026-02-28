using UnityEngine;

// Base class for mixing bowl actions since they all have uniform movement behavior.
public abstract class MixingBowlAction : BossAction
{
    [SerializeField] private ProjectileShooter shooter;
    public override void OnActionBegin()
    {
        base.OnActionBegin();
        Boss.movement.OnReachPoint += Shoot;
    }
    public override void OnActionExit()
    {
        base.OnActionExit();
        Boss.movement.OnReachPoint -= Shoot;
    }

    private void Shoot(Vector2 reachedPos)
    {

    }
}
