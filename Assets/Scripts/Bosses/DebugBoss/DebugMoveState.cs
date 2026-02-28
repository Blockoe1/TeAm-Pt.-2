using System.Collections;
using UnityEngine;

[System.Serializable]
public class DebugMoveState : BossState
{
    [SerializeField] private Transform[] moveTargets;

    public override IEnumerator StateRoutine()
    {
        while (true)
        {
            Boss.Movement.SetMoveTarget(moveTargets[Random.Range(0, moveTargets.Length)].position);
            yield return new WaitForSeconds(5f);
        }
    }
}
