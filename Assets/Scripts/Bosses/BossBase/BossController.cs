using System;
using UnityEditor.Search;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [field: SerializeField] public Transform playerTransform { get; private set; }
    [SerializeField] private BossPhase[] phases;

    private bool queuedPhase;
    private int queuedPhaseIndex;
    private int currentPhase;

    // Component References
    public BossMovement movement { get; private set; }

    /// <summary>
    /// Initialize all states.
    /// </summary>
    private void Awake()
    {
        // Get Components
        movement = GetComponent<BossMovement>();

        foreach(var phase in phases)
        {
            phase.Initialize(this);
        }
    }
    private void OnDestroy()
    {
        foreach (var phase in phases)
        {
            phase.Deinitialize();
        }
    }

    private void Start()
    {
        SetPhase(0);
    }

    /// <summary>
    /// Checks if the next phase should be transitioned to.
    /// </summary>
    public void QueryPhase(int currentHealth)
    {
        Debug.Log(currentHealth < phases[currentPhase + 1].healthThreshold);
        if (currentPhase + 1 < phases.Length && currentHealth < phases[currentPhase + 1].healthThreshold)
        {
            if (phases[currentPhase + 1].transitionInstant)
            {
                SetPhase(currentPhase + 1);
            }
            else
            {
                queuedPhase = true;
                queuedPhaseIndex = currentPhase + 1;
            }
        }
    }

    public bool QueuedPhaseTransition()
    {
        Debug.Log("Transitioning Phase.");
        if (queuedPhase)
        {
            
            queuedPhase = false;
            SetPhase(queuedPhaseIndex);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Sets a phase as the active phase.
    /// </summary>
    /// <param name="phase"></param>
    private void SetPhase(int phase)
    {
        phases[currentPhase]?.OnPhaseExit();
        currentPhase = phase;
        phases[currentPhase]?.OnPhaseEnter();    
    }
}
