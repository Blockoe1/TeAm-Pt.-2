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

        m.Anim.runtimeAnimatorController = m.YolkAnimOCs[0];
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

        DetermineAnimationDirection();
    }

    private void Move()
    {
        //Move
        m.Rb2d.linearVelocity = (m.MoveDirection * m.YolkMoveSpeed);

        //Animimation
        m.Anim.SetBool("IS_MOVING", m.IsMoving());//(Mathf.Abs(m.Rb2d.linearVelocity.x) > 0.25f || Mathf.Abs(m.Rb2d.linearVelocity.y) > 0.25f) ? true : false);
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

    private void DetermineAnimationDirection()
    {
        if (Mathf.Abs(m.MoveDirection.x) < Mathf.Abs(m.MoveDirection.y)) //Front/Back
            m.Anim.runtimeAnimatorController = m.YolkAnimOCs[(m.MoveDirection.y > 0) ? 2 : 0];
        else //Side
        {
            m.Anim.runtimeAnimatorController = m.YolkAnimOCs[1];
            m.SpriteRen.flipX = (m.MoveDirection.x > 0) ? true : false;
        }
    }
}
