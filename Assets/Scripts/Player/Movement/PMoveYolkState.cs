using NUnit.Framework.Constraints;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMoveYolkState : PMoveBaseSt
{
    private PMoveStateMngr m;

    private bool waitingForSound;
    private bool dashCooldown;

    public PMoveYolkState(PMoveStateMngr m)
    {
        this.m = m;
    }
    public override void EnterState()
    {
        m.Dash.performed += Dash_performed;

        m.CurOC = m.YolkAnimOCs;
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
    }

    private void Move()
    {
        //Move
        m.Rb2d.linearVelocity = (m.MoveDirection * m.YolkMoveSpeed);

        //Animimation
        m.Anim.SetBool("IS_MOVING", m.IsMoving());

        CheckForSound(m.IsMoving());
    }

    private async void CheckForSound(bool moving)
    {
        if ( !moving || waitingForSound)
        {
            return;
        }

        waitingForSound = true;
        await Task.Delay((int)Random.Range(10, 150));

        AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerMovesAsYolk);

        await Task.Delay(60);

        waitingForSound = false;
        
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        if (dashCooldown) { return; }
        m.rs.PlayerIsRolling = true;
        m.StartCoroutine(DashRoutine());
        m.Anim.SetTrigger("DASH");
        m.Health.IFrames(m.YolkDashDuration);
        m.StartCoroutine(DashCooldown(m.YolkDashCooldown));
    }
    private void Dash()
    {
        //Move
        m.Rb2d.linearVelocity = (m.YolkDashSpeed * m.FaceDirection);
    }
    private IEnumerator DashRoutine()
    {
        m.IsDashing = true;
        yield return new WaitForSeconds(m.YolkDashDuration);
        m.IsDashing = false;
    }

    private IEnumerator DashCooldown(float time)
    {
        dashCooldown = true;
        yield return new WaitForSeconds(time);
        dashCooldown = false;
        m.rs.PlayerIsRolling = false;
    }
}
