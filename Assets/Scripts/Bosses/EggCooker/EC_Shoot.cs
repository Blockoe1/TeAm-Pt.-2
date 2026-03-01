using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Egg Cooker")]
public class EC_Shoot : BossAction
{
    [SerializeField] private ProjectileShooter shooter;
    [SerializeField] private float shootPower;
    [SerializeField] private int shootCount = 1;
    [SerializeField] private float shootAngle;
    [SerializeField] private float preDelay;
    [SerializeField] private float postDelay;
    public override IEnumerator ActionRoutine()
    {
        yield return new WaitForSeconds(preDelay);
        shooter.Shoot(Boss.ToPlayer, shootPower, shootCount, shootAngle);
        yield return new WaitForSeconds(postDelay);
        Phase.NextAction();
    }
}
