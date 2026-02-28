using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Mixing Bowl")]
public class MB_EggRollShot : BossAction
{
    [SerializeField] protected ProjectileShooter shooter;
    [SerializeField] protected float shotPower;
    [SerializeField] private float eggRollStartOffset;
    [SerializeField] private float postShotWait;
    public override IEnumerator ActionRoutine()
    {
        Vector2 toCenter = (Vector3.zero - Boss.transform.position).normalized;
        toCenter = Mathf.Abs(toCenter.x) > Mathf.Abs(toCenter.y) ? new Vector2(toCenter.x, 0) : 
            new Vector2(0, toCenter.y);
        toCenter = toCenter.normalized;
        shooter.Shoot(toCenter, shotPower, (Vector2)Boss.transform.position - (toCenter * eggRollStartOffset));
        yield return new WaitForSeconds(postShotWait);
        Phase.NextAction();
    }
}
