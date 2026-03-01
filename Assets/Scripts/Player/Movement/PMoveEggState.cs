using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class PMoveEggState : PMoveBaseSt
{
    PMoveStateMngr m;
    private bool butter;
    private bool dashCooldown;

    private float accelAmount, deccelAmount;

    private EventInstance moveEggRoll;
    private bool started = false;
    bool referenceGrabbed = false;

    //public bool PlayerHasMoved;
    [HideInInspector] public bool PlayerHasDashed;

    public PMoveEggState(PMoveStateMngr m)
    {
        this.m = m;
        accelAmount = (50 * m.AccelerationSpeed) / m.EggMoveSpeed;
        deccelAmount = (50 * m.DeccelerationSpeed) / m.EggMoveSpeed;
    }
    public override void EnterState()
    {
        m.Dash.performed += Dash_performed;

        m.CurOC = m.WholeAnimOCs;
    }

    public override void ExitState()
    {
        m.Dash.performed -= Dash_performed;
        moveEggRoll.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public override void FixedUpdateState()
    {
        Move();
    }

    private void Move()
    {
        if(!referenceGrabbed)
        {
            moveEggRoll = RuntimeManager.CreateInstance(FMODEvents.instance.PlayerRollsAsEgg);
            referenceGrabbed = true;
        }

        if(!started && m.IsMoving())
        {
            moveEggRoll.start();
            started = true;
        }

        //PlayerHasMoved = true;

        //Move
        Vector2 targetSpeed = m.MoveDirection * m.EggMoveSpeed;
        targetSpeed = new Vector2(Mathf.Lerp(m.Rb2d.linearVelocity.x, targetSpeed.x, 1), Mathf.Lerp(m.Rb2d.linearVelocity.y, targetSpeed.y, 1));

        float accelRateX = (Mathf.Abs(targetSpeed.x) > 0.01f) ? accelAmount : deccelAmount;
        float accelRateY = (Mathf.Abs(targetSpeed.y) > 0.01f) ? accelAmount : deccelAmount;

        Vector2 speedDifference = new Vector2(targetSpeed.x - m.Rb2d.linearVelocity.x, targetSpeed.y - m.Rb2d.linearVelocity.y);
        Vector2 movement = speedDifference * new Vector2(accelRateX, accelRateY);
        m.Rb2d.AddForce(movement, ForceMode2D.Force);

        //Animimation
        m.Anim.SetBool("IS_MOVING", m.IsMoving());
        if(!m.IsMoving())
        {
            moveEggRoll.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            started = false;
        }
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        PlayerHasDashed = true;
        if (dashCooldown) return;
        m.rs.PlayerIsRolling = true;
        //Move
        m.Rb2d.AddForce(m.EggDashSpeed * m.FaceDirection, ForceMode2D.Impulse);
        m.StartCoroutine(DashTrail());

        //Animation
        m.Anim.SetTrigger("DASH");
        m.Health.IFrames(m.EggDashFrames);
        m.StartCoroutine(DashCooldown(m.EggDashCooldown));
    }
    
    private IEnumerator DashTrail()
    {
        m.DashTrail.enabled = true;
        yield return new WaitForSeconds(m.EggDashCooldown);
        m.DashTrail.enabled = false;
    }

    private IEnumerator DashCooldown(float time)
    {
        dashCooldown = true;

        yield return new WaitForSeconds(time);
        dashCooldown = false;
        m.rs.PlayerIsRolling = false;
    }
}
