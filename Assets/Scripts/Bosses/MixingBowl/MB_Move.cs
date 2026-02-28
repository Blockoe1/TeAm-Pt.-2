using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Mixing Bowl")]
public class MB_Move : BossAction
{
    [SerializeField] private float postMoveWait;
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

    private void OnReachPoint(Vector2 reachedPos)
    {
        reachedPoint = true;
    }

    public override IEnumerator ActionRoutine()
    {
        reachedPoint = false;
        bowlMovement.MoveToNext();
        yield return new WaitUntil(() => reachedPoint);
        yield return new WaitForSeconds(postMoveWait);

        Phase.NextAction();
    }
}
