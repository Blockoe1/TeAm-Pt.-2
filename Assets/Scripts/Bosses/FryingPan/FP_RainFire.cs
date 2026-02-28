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
    [SerializeField, ShowIf("ShowShotDelay"), AllowNesting] private float shotDelay;
    [SerializeField] private float postShotWait;


    public override IEnumerator ActionRoutine()
    {
        for (int i = 0; i < shotCount; i++)
        {
            shooter.RainingDown(rainingLimitsMin, rainingLimitsMax);
            yield return new WaitForSeconds(shotDelay);
        }

        yield return new WaitForSeconds(postShotWait);
        Phase.NextAction();
    }
}
