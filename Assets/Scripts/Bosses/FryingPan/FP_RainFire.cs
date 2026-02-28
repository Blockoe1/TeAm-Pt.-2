using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("FryingPan")]
public class FP_RainFire : BossAction
{
    [SerializeField] protected Vector2 rainingLimitsMin;
    [SerializeField] protected Vector2 rainingLimitsMax;

    [SerializeField] protected int shotCount;
    [SerializeField] protected ProjectileShooter shooter;
    [SerializeField] protected int shotAmount;
    [SerializeField, ShowIf("ShowShotDelay"), AllowNesting] private float shotDelay;
    [SerializeField] private float postShotWait;
    private bool ShowShotDelay => shotAmount > 1;

    public override IEnumerator ActionRoutine()
    {
        for (int i = 0; i < shotAmount; i++)
        {
            shooter.RainingDown(rainingLimitsMin, rainingLimitsMax);
            yield return new WaitForSeconds(shotDelay);
        }

        yield return new WaitForSeconds(postShotWait);
        Phase.NextAction();
    }
}
