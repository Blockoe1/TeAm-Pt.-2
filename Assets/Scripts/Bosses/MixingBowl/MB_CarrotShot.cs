using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Mixing Bowl")]
public class MB_CarrotShot : BossAction
{
    [SerializeField] protected ProjectileShooter shooter;
    [SerializeField] protected float shotPower;
    [SerializeField] private int shotCount;
    [SerializeField, ShowIf("ShowShotDelay"), AllowNesting] private float shotDelay;
    [SerializeField] private float postShotWait;
    [SerializeField] private float telegraphTime;

    private bool ShowShotDelay => shotCount > 1;
    public override IEnumerator ActionRoutine()
    {
        Boss.Telegraph.ToggleLine(true);
        Boss.Telegraph.TrackingTarget = Boss.playerTransform;
        yield return new WaitForSeconds(telegraphTime);
        Boss.Telegraph.ToggleLine(false);
        Boss.Telegraph.TrackingTarget = null;
        for (int i = 0; i < shotCount; i++)
        {
            shooter.Shoot(Boss.ToPlayerN, shotPower);
            yield return new WaitForSeconds(shotDelay);
        }

        yield return new WaitForSeconds(postShotWait);
        Phase.NextAction();
    }
}
