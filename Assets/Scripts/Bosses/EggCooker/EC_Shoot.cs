using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Egg Cooker")]
public class EC_Shoot : BossAction
{
    [SerializeField] private ProjectileShooter shooter;
    [SerializeField] private float shootDelay;
    [SerializeField] private float shootPower;
    public override IEnumerator ActionRoutine()
    {
        yield return new WaitForSeconds(shootDelay);
        shooter.Shoot(Boss.ToPlayer, shootPower);
        Phase.NextAction();
    }
}
