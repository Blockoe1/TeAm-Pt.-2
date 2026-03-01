using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] private UnityEvent<int> OnDamage;
    [SerializeField] private UnityEvent OnDeath;

    public int health;

    private void Awake()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public void Damage(int damage)
    {
        health -= damage;
        OnDamage?.Invoke(health);
        if (health <= 0)
        {
            // Death
            OnDeath?.Invoke();
        }
    }

    [Button]
    private void DebugDamage()
    {
        Damage(10);
        Debug.Log(health);
    }
}
