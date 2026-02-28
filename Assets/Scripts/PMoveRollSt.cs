using UnityEngine;

public class PMoveRollSt : PMoveBaseSt
{
    PMoveStateMngr m;

    public PMoveRollSt(PMoveStateMngr m)
    {
        this.m = m;
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
        m.MoveDirection = m.Move.ReadValue<Vector2>();

        m.Rb2d.linearVelocity = m.MoveDirection * m.RollSpeed;
    }
}
