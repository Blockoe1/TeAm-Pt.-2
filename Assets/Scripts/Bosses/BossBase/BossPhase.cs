using System;
using UnityEngine;

[System.Serializable]
public class BossPhase
{
    [field: SerializeField] public float healthThreshold { get; private set; }
    [field: SerializeField] public bool transitionInstant { get; private set; }
    [SerializeReference, ClassDropdown(typeof(BossAction))] private BossAction[] actions;

    private BossAction currentAction;

    private BossController boss;

    #region Properties
    public float HealthThreshold => healthThreshold;
    #endregion

    public void Initialize(BossController boss)
    {
        this.boss = boss;
        foreach (var state in actions)
        {
            state.Initialize(boss, this);
        }
    }

    public void OnPhaseEnter()
    {
        SetActionInternal(actions[0]);
    }

    public void OnPhaseExit()
    {
        SetActionInternal(null);
    }

    #region State Management
    public void SetRandomAction()
    {
        SetAction(actions[UnityEngine.Random.Range(0, actions.Length)]);
    }
    public T GetAction<T>(int index = 0) where T : BossAction
    {
        T state = (T)Array.Find(actions, item => item.GetType() == typeof(T));
        return state;
    }


    public void SetAction(BossAction action)
    {
        // If there is a queued phase transition, set it and 
        if (boss.QueuedPhaseTransition())
        {
            return;
        }
        SetActionInternal(action);
    }
    private void SetActionInternal(BossAction action)
    {
        currentAction?.OnActionExit();
        currentAction = action;
        currentAction?.OnActionBegin();
    }

    public T SetAction<T>(int index = 0) where T : BossAction
    {
        T action = GetAction<T>();
        SetAction(action);
        return action;
    }
    #endregion
}
