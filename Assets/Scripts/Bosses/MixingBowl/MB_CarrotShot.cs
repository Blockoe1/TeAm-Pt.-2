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

    private bool ShowShotDelay => shotCount > 1;
    public override IEnumerator ActionRoutine()
    {
        for(int i = 0; i < shotCount; i++)
        {
            shooter.Shoot(Boss.ToPlayerN, shotPower);
            yield return new WaitForSeconds(shotDelay);
        }

        yield return new WaitForSeconds(postShotWait);
        Phase.NextAction();
    }
}
