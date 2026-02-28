using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private UnityEvent<int> OnDamage;

    private int health;

    private void Awake()
    {
        health = maxHealth;
    }

    public void Damage(int damage)
    {
        health -= damage;
        Debug.Log(health);
        OnDamage?.Invoke(health);
        if (health <= 0)
        {
            // Death
        }
    }

    [Button]
    private void DebugDamage()
    {
        Damage(10);
    }
}
