using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class BossState
{
    private BossController boss;

    private Coroutine stateRoutine;

    #region Properties
    protected BossController Boss => boss;
    #endregion

    public virtual void Initialize(BossController boss)
    {
        this.boss = boss;
    }

    /// <summary>
    /// Setup the state routine.
    /// </summary>
    public virtual void OnStateEnter()
    {
        stateRoutine = boss.StartCoroutine(StateRoutine());
    }

    public virtual void OnStateExit()
    {
        if (stateRoutine != null)
        {
            boss.StopCoroutine(stateRoutine);
            stateRoutine = null;
        }
    }

    public virtual IEnumerator StateRoutine()
    {
        stateRoutine = null;
        yield break;
    }
}
