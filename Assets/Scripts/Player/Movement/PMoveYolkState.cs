using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMoveYolkState : PMoveBaseSt
{
    private PMoveStateMngr m;

    private bool isDashing;

    public PMoveYolkState(PMoveStateMngr m)
    {
        this.m = m;
    }
    public override void EnterState()
    {
        m.Dash.performed += Dash_performed;

        m.Anim.runtimeAnimatorController = m.YolkAnimOC;
    }

    public override void ExitState()
    {
        m.Dash.performed -= Dash_performed;
    }

    public override void FixedUpdateState()
    {
        if (!isDashing)
            Move();
        else
            Dash();
    }

    private void Move()
    {
        //Move
        m.Rb2d.linearVelocity = (m.MoveDirection * m.YolkMoveSpeed);

        //Animimation
        m.Anim.SetBool("IS_MOVING", (Mathf.Abs(m.Rb2d.linearVelocity.x) > 0.25f || Mathf.Abs(m.Rb2d.linearVelocity.y) > 0.25f) ? true : false);
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        m.StartCoroutine(DashCooldown());
    }
    private void Dash()
    {
        //Move
        m.Rb2d.linearVelocity = (m.YolkDashSpeed * m.transform.up);

        //Animation
        m.Anim.SetTrigger("DASH");
    }
    private IEnumerator DashCooldown()
    {
        isDashing = true;
        yield return new WaitForSeconds(m.YolkDashDuration);
        isDashing = false;
    }
}
