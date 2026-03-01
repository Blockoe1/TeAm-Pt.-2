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
    [SerializeField] private float telegraphTime;
    [SerializeField] private BossTelegrapher[] telegraphers;

    private bool ShowShotDelay => shotCount > 1;
    public override IEnumerator ActionRoutine()
    {
        for (int i = 0; i < shotCount; i++)
        {
            for (int j = 0; j < shotAmount; j++)
            {
                telegraphers[j].ToggleLine(true);
            }

            float telegraphTimer = telegraphTime;
            while (telegraphTimer > 0)
            {
                float stepAngle = shotAmount > 1 ? spread / (shotAmount - 1) : 0;
                float startingAngle = Mathf.Atan2(Boss.ToPlayerN.y, Boss.ToPlayerN.x) * Mathf.Rad2Deg;
                for (int j = 0; j < shotAmount; j++)
                {
                    float angle = startingAngle - (spread / 2) + (stepAngle * j);
                    Vector2 launchVector = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    telegraphers[j].Direction = launchVector;
                }
                telegraphTimer -= Time.deltaTime;
                yield return null;
            }

            for (int j = 0; j < telegraphers.Length; j++)
            {
                telegraphers[j].ToggleLine(false);
            }

            shooter.Shoot(Boss.ToPlayerN, shotPower, shotAmount, spread);
            yield return new WaitForSeconds(shotDelay);
        }

        yield return new WaitForSeconds(postShotWait);
        Phase.NextAction();
    }
}
