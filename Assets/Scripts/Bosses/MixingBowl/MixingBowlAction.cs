using System.Collections;
using UnityEngine;

// Base class for mixing bowl actions since they all have uniform movement behavior.
[System.Serializable]
[DropdownGroup("Mixing Bowl")]
public abstract class MixingBowlAction : BossAction
{
    #region CONSTS
    private const float PRE_SHOT_WAIT = 0.5f;
    private const float POST_SHOT_WAIT = 0.5f;
    #endregion

    [SerializeField] protected ProjectileShooter shooter;
    [SerializeField] protected float shotPower;

    private MixingBowlMovement bowlMovement;

    private bool reachedPoint;

    public override void Initialize(BossController boss, BossPhase phase)
    {
        base.Initialize(boss, phase);
        bowlMovement = boss.GetComponent<MixingBowlMovement>();
    }

    public override void OnActionBegin()
    {
        base.OnActionBegin();
        Boss.movement.OnReachPoint += OnReachPoint;
    }
    public override void OnActionExit()
    {
        base.OnActionExit();
        Boss.movement.OnReachPoint -= OnReachPoint;
    }

    public override IEnumerator ActionRoutine()
    {
        reachedPoint = false;
        bowlMovement.MoveToNext();
        yield return new WaitUntil(() => reachedPoint);
        yield return new WaitForSeconds(PRE_SHOT_WAIT);
        yield return Boss.StartCoroutine(ShotRoutine());
        yield return new WaitForSeconds(POST_SHOT_WAIT);

        Phase.SetRandomAction();
    }

    private void OnReachPoint(Vector2 reachedPos)
    {
        reachedPoint = true;
    }

    protected abstract IEnumerator ShotRoutine();
}
