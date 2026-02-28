using System.Collections;
using UnityEngine;

[System.Serializable]
public class MB_EggRollShot : MixingBowlAction
{
    [SerializeField] private float eggRollStartOffset;
    protected override IEnumerator ShotRoutine()
    {
        Vector2 toCenter = (Vector3.zero - Boss.transform.position).normalized;
        shooter.Shoot(toCenter, shotPower, (Vector2)Boss.transform.position - (toCenter * eggRollStartOffset));
        yield return null;
    }
}
