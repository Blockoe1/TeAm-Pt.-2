using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Egg Cooker")]
public class EC_Follow : BossAction
{
    [SerializeField] private float followSpeed;
    [SerializeField] private float followTime;

    public override IEnumerator ActionRoutine()
    {
        float timer = followTime;
        while (timer > 0)
        {
            Boss.Movement.TargetVelocity = Vector2.right * followSpeed;

            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Phase.NextAction();
    }

    public override void OnActionExit()
    {
        base.OnActionExit();
        Boss.Movement.TargetVelocity = Vector2.zero;
    }
}
