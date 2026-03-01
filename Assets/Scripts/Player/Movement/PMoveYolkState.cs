using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMoveYolkState : PMoveBaseSt
{
    private PMoveStateMngr m;

    public PMoveYolkState(PMoveStateMngr m)
    {
        this.m = m;
    }
    public override void EnterState()
    {
        m.Dash.performed += Dash_performed;
    }

    public override void ExitState()
    {
        m.Dash.performed -= Dash_performed;
    }

    public override void FixedUpdateState()
    {
        if (!m.IsDashing)
            Move();
        else
            Dash();

        m.Anim.runtimeAnimatorController = m.YolkAnimOCs[m.DetermineAnimationDirection()];
    }

    private void Move()
    {
        //Move
        m.Rb2d.linearVelocity = (m.MoveDirection * m.YolkMoveSpeed);

        //Animimation
        m.Anim.SetBool("IS_MOVING", m.IsMoving());
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        m.StartCoroutine(DashCooldown());
        m.Anim.SetTrigger("DASH");
    }
    private void Dash()
    {
        //Move
        m.Rb2d.linearVelocity = (m.YolkDashSpeed * m.FaceDirection);
    }
    private IEnumerator DashCooldown()
    {
        m.IsDashing = true;
        yield return new WaitForSeconds(m.YolkDashDuration);
        m.IsDashing = false;
    }
}
