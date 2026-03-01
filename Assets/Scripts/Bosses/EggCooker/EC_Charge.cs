using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[System.Serializable]
[DropdownGroup("Egg Cooker")]
public class EC_Charge : BossAction
{
    [SerializeField] private float chargeSpeed;
    [SerializeField] private int chargeNumber = 1;
    [SerializeField] private float chargeTime = 1;
    [SerializeField] private float chargeDelay = 1;

    private bool showChargeDelay => chargeNumber > 1;

    public override IEnumerator ActionRoutine()
    {
        for(int i = 0; i < chargeNumber; i++)
        {
            Boss.Movement.SnapRotation();
            //Boss.Movement.Rb.AddForce(Boss.ToPlayer * dashPower, ForceMode2D.Impulse);
            float timer = chargeTime;

            Transform trackTarget = Boss.Movement.TrackingTarget;
            Boss.Movement.TrackingTarget = null;
            Vector2 chargeDir = Boss.ToPlayerN;
            while (timer > 0)
            {
                Boss.Movement.TargetVelocity = chargeDir * chargeSpeed;
                timer -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Boss.Movement.TrackingTarget = trackTarget;
            Boss.Movement.TargetVelocity = Vector2.zero;

            yield return new WaitForSeconds(chargeDelay);
        }

        Phase.NextAction();
    }

    //public void Charge()
    //{
    //    Debug.Log("Charge");
    //    transform.LookAt(transform.position);
    //    rb.AddForce(transform.right * 1000);
    //}
}
