using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("FryingPan")]
public class FP_FireButter : BossAction
{
    [SerializeField] protected ProjectileShooter shooter;
    [SerializeField] protected float shotPower;
    [SerializeField] private int shotAmount;
    [SerializeField] private int shotCount;
    [SerializeField] protected int spread;
    [SerializeField, ShowIf("ShowShotDelay"), AllowNesting] private float shotDelay;
    [SerializeField] private float postShotWait;

    private bool ShowShotDelay => shotCount > 1;
    public override IEnumerator ActionRoutine()
    {
        for (int i = 0; i < shotCount; i++)
        {
            shooter.Shoot(Boss.ToPlayer, shotPower, shotAmount, spread);
            yield return new WaitForSeconds(shotDelay);
        }

        yield return new WaitForSeconds(postShotWait);
        Phase.NextAction();
    }
}
