using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Egg Cooker")]
public class EC_Charge : BossAction
{
    [SerializeField] private float dashPower;
    [SerializeField] private int chargeNumber = 1;
    [SerializeField] private float chargeDelay = 0;
    [SerializeField] private float postChargeWait = 0.5f;

    private bool showChargeDelay => chargeNumber > 1;

    public override IEnumerator ActionRoutine()
    {
        for(int i = 0; i < chargeNumber; i++)
        {
            Boss.Movement.SnapToTarget();
            Boss.Movement.Rb.AddForce(Boss.ToPlayer * dashPower, ForceMode2D.Impulse);
            Transform trackTarget = Boss.Movement.TrackingTarget;
            yield return new WaitForSeconds(chargeDelay);
            Boss.Movement.TrackingTarget = trackTarget;

        }
        yield return new WaitForSeconds(postChargeWait);

        Phase.NextAction();
    }

    //public void Charge()
    //{
    //    Debug.Log("Charge");
    //    transform.LookAt(transform.position);
    //    rb.AddForce(transform.right * 1000);
    //}
}
