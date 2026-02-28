using System;
using UnityEngine;

[System.Serializable]
public class BossPhase
{
    [field: SerializeField] public float healthThreshold { get; private set; }
    [field: SerializeField] public bool transitionInstant { get; private set; }
    [SerializeReference, ClassDropdown(typeof(BossAction))] private BossAction[] actions;

    private BossAction currentAction;
    private int currentActionIndex;

    private BossController boss;
    public int phaseIndex { get; private set; }

    #region Properties
    public float HealthThreshold => healthThreshold;
    #endregion

    public void Initialize(BossController boss, int index)
    {
        this.boss = boss;
        this.phaseIndex = index;
        foreach (var state in actions)
        {
            state.Initialize(boss, this);
        }
    }

    public void Deinitialize()
    {
        foreach (var state in actions)
        {
            state.Deinitialize();
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
        currentActionIndex = UnityEngine.Random.Range(0, actions.Length);
        SetAction(actions[currentActionIndex]);
    }
    public void NextAction()
    {
        SetAction(actions[(currentActionIndex + 1) % actions.Length]);
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
        currentActionIndex = Array.IndexOf(actions, action);
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
