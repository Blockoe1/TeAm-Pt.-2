using UnityEngine;

public class PMoveRollSt : PMoveBaseSt
{
    PMoveStateMngr m;

    private float accelAmount, deccelAmount;

    public PMoveRollSt(PMoveStateMngr m)
    {
        this.m = m;
        accelAmount = (50 * m.AccelerationSpeed) / m.RollSpeed;
        deccelAmount = (50 * m.DeccelerationSpeed) / m.RollSpeed;
    }
    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void FixedUpdateState()
    {
        Move();
    }

    private void Move()
    {
        Vector2 targetSpeed = m.MoveDirection * m.RollSpeed;
        targetSpeed = new Vector2(Mathf.Lerp(m.Rb2d.linearVelocity.x, targetSpeed.x, 1), Mathf.Lerp(m.Rb2d.linearVelocity.y, targetSpeed.y, 1));

        float accelRateX = (Mathf.Abs(targetSpeed.x) > 0.01f) ? accelAmount : deccelAmount;
        float accelRateY = (Mathf.Abs(targetSpeed.y) > 0.01f) ? accelAmount : deccelAmount;

        Vector2 speedDifference = new Vector2(targetSpeed.x - m.Rb2d.linearVelocity.x, targetSpeed.y - m.Rb2d.linearVelocity.y);
        Vector2 movement = speedDifference * new Vector2(accelRateX, accelRateY);
        Debug.Log(movement);
        m.Rb2d.AddForce(movement, ForceMode2D.Force);
        //m.Rb2d.linearVelocity = m.MoveDirection * m.RollSpeed;
    }
}
