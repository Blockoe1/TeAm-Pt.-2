using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] private float hitIFrames;
    [SerializeField] private UnityEvent<int> OnDamage;
    [SerializeField] private UnityEvent OnDeath;

    public int health;
    private bool iFrames = false;

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
        IFrames(hitIFrames);
        if (health <= 0)
        {
            // Death
            OnDeath?.Invoke();
        }
    }

    public void IFrames(float time)
    {
        if (iFrames) { return; }
        StartCoroutine(IFramesRoutine(time));
    }

    private IEnumerator IFramesRoutine(float time)
    {
        iFrames = true;
        yield return new WaitForSeconds(time);
        iFrames = false;
    }

    [Button]
    private void DebugDamage()
    {
        Damage(10);
        Debug.Log(health);
    }
}
