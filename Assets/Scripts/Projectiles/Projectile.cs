using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected bool despawnOnCollision;
    #region CONST
    private const float DESPAWN_TIME = 10f;
    #endregion
    [field: SerializeField] protected Rigidbody2D rb { get; private set; }

    private Coroutine lifetimeRoutine;

    public Action<Projectile> despawnAction;

    public void SetDespawnAction(Action<Projectile> despawnAction)
    {
        this.despawnAction = despawnAction;
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
        despawnAction?.Invoke(this);
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
