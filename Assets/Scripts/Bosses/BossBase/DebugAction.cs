using UnityEngine;

[System.Serializable]
public class DebugAction : BossAction
{
    public override void OnActionBegin()
    {
        base.OnActionBegin();
        Debug.Log("Debug state entered.");
    }
    public override void OnActionExit()
    {
        base.OnActionExit();
        Debug.Log("Debug state exited.");
    }
}
