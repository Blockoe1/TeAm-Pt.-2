using UnityEngine;
using UnityEngine.InputSystem;
public class PMoveEggState : PMoveBaseSt
{
    PMoveStateMngr m;
    private bool butter;

    private float accelAmount, deccelAmount;

    public PMoveEggState(PMoveStateMngr m)
    {
        this.m = m;
        accelAmount = (50 * m.AccelerationSpeed) / m.EggMoveSpeed;
        deccelAmount = (50 * m.DeccelerationSpeed) / m.EggMoveSpeed;
    }
    public override void EnterState()
    {
        m.Dash.performed += Dash_performed;

        m.Anim.runtimeAnimatorController = m.EggAnimOC;
    }

    public override void ExitState()
    {
        m.Dash.performed -= Dash_performed;
    }

    public override void FixedUpdateState()
    {
        Move();
    }
    private void Move()
    {
        //Move
        Vector2 targetSpeed = m.MoveDirection * m.EggMoveSpeed;
        targetSpeed = new Vector2(Mathf.Lerp(m.Rb2d.linearVelocity.x, targetSpeed.x, 1), Mathf.Lerp(m.Rb2d.linearVelocity.y, targetSpeed.y, 1));

        float accelRateX = (Mathf.Abs(targetSpeed.x) > 0.01f) ? accelAmount : deccelAmount;
        float accelRateY = (Mathf.Abs(targetSpeed.y) > 0.01f) ? accelAmount : deccelAmount;

        Vector2 speedDifference = new Vector2(targetSpeed.x - m.Rb2d.linearVelocity.x, targetSpeed.y - m.Rb2d.linearVelocity.y);
        Vector2 movement = speedDifference * new Vector2(accelRateX, accelRateY);
        m.Rb2d.AddForce(movement, ForceMode2D.Force);

        //Animimation
        m.Anim.SetBool("IS_MOVING", m.IsMoving());//(Mathf.Abs(m.Rb2d.linearVelocity.x) > 0.25f || Mathf.Abs(m.Rb2d.linearVelocity.y) > 0.25f) ? true : false);
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        //Move
        m.Rb2d.AddForce(m.EggDashSpeed * m.FaceDirection, ForceMode2D.Impulse);

        //Animation
        m.Anim.SetTrigger("DASH");
    }
}
