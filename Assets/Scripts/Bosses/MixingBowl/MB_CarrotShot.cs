using System.Collections;
using UnityEngine;

[System.Serializable]
public class MB_CarrotShot : MixingBowlAction
{
    [SerializeField] private float shotCount;
    [SerializeField] private float shotDelay;
    protected override IEnumerator ShotRoutine()
    {
        for(int i = 0; i < shotCount; i++)
        {
            shooter.Shoot(Boss.ToPlayer, shotPower);
            yield return new WaitForSeconds(shotDelay);
        }
    }
}
