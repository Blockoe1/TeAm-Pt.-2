using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected bool despawnOnCollision;
    protected enum ProjectileType
    {
        FlamingButter, Other
    }

    [SerializeField] private ProjectileType projectileType;
    #region CONST
    private const float DESPAWN_TIME = 10f;
    #endregion
    [field: SerializeField] protected Rigidbody2D rb { get; private set; }

    private Coroutine lifetimeRoutine;

    public event Action<Projectile> OnDespawn;

    private void Awake()
    {
        if(projectileType == ProjectileType.FlamingButter && AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.IgniteOnFire);
        }
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Launch(Vector2 launchVector)
    {
        rb.AddForce(launchVector, ForceMode2D.Impulse);
        lifetimeRoutine = StartCoroutine(Lifetime());
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(DESPAWN_TIME);
        Despawn();
    }

    public void Despawn()
    {
        OnDespawn?.Invoke(this);
        if (lifetimeRoutine != null)
        {
            StopCoroutine(lifetimeRoutine);
            lifetimeRoutine = null;
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // On collision with anything, destroy the projectile.
        if (despawnOnCollision)
        {
            Despawn();
        }
    }
}
