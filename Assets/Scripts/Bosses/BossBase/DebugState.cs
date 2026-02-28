using UnityEngine;

[System.Serializable]
public class DebugState : BossState
{
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("Debug state entered.");
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        Debug.Log("Debug state exited.");
    }
}
