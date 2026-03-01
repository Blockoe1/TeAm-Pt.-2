using System.Collections;
using UnityEngine;

public class EggBomb : MonoBehaviour
{
    #region CONSTS
    private const float EXPLODE_THRESHOLD = 0.5f;
    #endregion

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ProjectileShooter shooter;
    [SerializeField] private Projectile proj;
    [SerializeField] private float shotForce;
    [SerializeField] private int shotCount;
    [SerializeField] private float shotAngle;
    [SerializeField] private float explodeDelay;

    private bool isExploded;

    private void OnEnable()
    {
        proj.OnDespawn += Explode;
    }
    private void OnDisable()
    {
        proj.OnDespawn -= Explode;
    }

    private void FixedUpdate()
    {
        Debug.Log(rb.linearVelocity.magnitude);
        if (rb.linearVelocity.magnitude < EXPLODE_THRESHOLD && !isExploded)
        {
            StartCoroutine(ExplodeDelayRoutine());
            isExploded = true;
        }
    }

    private IEnumerator ExplodeDelayRoutine()
    {
        yield return new WaitForSeconds(explodeDelay);
        proj.Despawn();
    }

    private void Explode(Projectile proj)
    {
        shooter.Shoot(0, shotForce, shotCount, shotAngle);
    }
}
