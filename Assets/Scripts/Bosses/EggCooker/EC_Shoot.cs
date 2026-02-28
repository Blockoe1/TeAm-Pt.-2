using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Egg Cooker")]
public class EC_Shoot : BossAction
{
    [SerializeField] private ProjectileShooter shooter;
    [SerializeField] private float shootPower;
    [SerializeField] private float preDelay;
    [SerializeField] private float postDelay;
    public override IEnumerator ActionRoutine()
    {
        yield return new WaitForSeconds(preDelay);
        shooter.Shoot(Boss.ToPlayer, shootPower);
        yield return new WaitForSeconds(postDelay);
        Phase.NextAction();
    }
}
