using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class BossAction
{
    private BossController boss;
    private BossPhase phase;

    private Coroutine stateRoutine;

    #region Properties
    protected BossController Boss => boss;
    protected BossPhase Phase => phase;

    #endregion

    public virtual void Initialize(BossController boss, BossPhase phase)
    {
        this.boss = boss;
        this.phase = phase;
    }

    public virtual void Deinitialize() { }

    /// <summary>
    /// Setup the state routine.
    /// </summary>
    public virtual void OnActionBegin()
    {
        stateRoutine = boss.StartCoroutine(ActionRoutine());
    }

    public virtual void OnActionExit()
    {
        if (stateRoutine != null)
        {
            boss.StopCoroutine(stateRoutine);
            stateRoutine = null;
        }
    }

    public virtual IEnumerator ActionRoutine()
    {
        stateRoutine = null;
        yield break;
    }
}
