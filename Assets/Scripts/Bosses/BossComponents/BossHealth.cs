using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private UnityEvent<int> OnDamage;
    [SerializeField] private UnityEvent OnDeath;

    private int health;

    private void Awake()
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
