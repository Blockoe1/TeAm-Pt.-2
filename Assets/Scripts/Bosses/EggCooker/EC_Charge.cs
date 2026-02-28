using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Egg Cooker")]
public class EC_Charge : BossAction
{
    [SerializeField] private float postMoveWait;
    private EggCookerMovement eggCookerMovement;

    private bool reachedPoint;

    public override void Initialize(BossController boss, BossPhase phase)
    {
        base.Initialize(boss, phase);
        eggCookerMovement = boss.GetComponent<EggCookerMovement>();
    }

    /// <summary>
    /// If we came from a movement state, instantly transition.
    /// </summary>
    /// <param name="previousAction"></param>
    /// <returns></returns>
    public override bool CheckFirstAction(BossAction previousAction)
    {
        if (previousAction is EC_Charge)
        {
            Phase.NextAction();
            return false;
        }
        else
        {
            return true;
        }
    }

    public override IEnumerator ActionRoutine()
    {
        reachedPoint = false;
        eggCookerMovement.Charge();
        yield return new WaitUntil(() => reachedPoint);
        yield return new WaitForSeconds(postMoveWait);

        Phase.NextAction();
    }
}
