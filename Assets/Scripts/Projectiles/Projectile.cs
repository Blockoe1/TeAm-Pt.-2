using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region CONST
    private const float DESPAWN_TIME = 10f;
    #endregion
    protected Rigidbody2D rb { get; private set; }

    private Coroutine lifetimeRoutine;

    public Action<Projectile> despawnAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

    protected void Despawn()
    {
        despawnAction?.Invoke(this);
        if (lifetimeRoutine != null)
        {
            StopCoroutine(lifetimeRoutine);
            lifetimeRoutine = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On collision with anything, destroy the projectile.
        Despawn();
    }
}
