using System;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [field: SerializeField] public Transform playerTransform { get; private set; }
    [SerializeReference, ClassDropdown(typeof(BossState))] private BossState[] states;

    private BossState currentState;

    // Component References
    public BossMovement movement { get; private set; }

    /// <summary>
    /// Initialize all states.
    /// </summary>
    private void Awake()
    {
        // Get Components
        movement = GetComponent<BossMovement>();

        foreach(var state in states)
        {
            state.Initialize(this);
        }
    }

    private void Start()
    {
        SetState(states[0]);
    }

    public T GetState<T>(int index = 0) where T : BossState
    {
        T state = (T)Array.Find(states, item => item.GetType() == typeof(T));
        return state;
    }

    public void SetState(BossState state)
    {
        currentState?.OnStateExit();
        currentState = state;
        currentState?.OnStateEnter();
    }

    public T SetState<T>(int index = 0) where T: BossState
    {
        T state = GetState<T>();
        SetState(state);
        return state;
    }
}
