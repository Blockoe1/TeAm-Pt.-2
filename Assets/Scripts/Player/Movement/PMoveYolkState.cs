using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

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
        m.Rb2d.linearVelocity = (m.MoveDirection * m.YolkMoveSpeed);
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        m.StartCoroutine(DashCooldown());
    }
    private void Dash()
    {
        m.Rb2d.linearVelocity = (m.YolkDashSpeed * m.transform.up);
    }
    private IEnumerator DashCooldown()
    {
        isDashing = true;
        yield return new WaitForSeconds(m.YolkDashDuration);
        isDashing = false;
    }
}
