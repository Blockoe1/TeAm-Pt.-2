using System.Collections;
using UnityEngine;

public class EggBomb : MonoBehaviour
{
    #region CONSTS
    private const float EXPLODE_THRESHOLD = 1f;
    #endregion

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ProjectileShooter shooter;
    [SerializeField] private Projectile proj;
    [SerializeField] private float shotForce;
    [SerializeField] private int shotCount;
    [SerializeField] private float shotAngle;
    [SerializeField] private float explodeDelay;

    private void OnEnable()
    {
        proj.OnDespawn += Explode;
    }
    private void OnDisable()
    {
        proj.OnDespawn -= Explode;
    }

    private void Explode(Projectile proj)
    {
        ParticleMngr.Inst.Play("EGG_EXPLODE_FLAT", transform.position, transform.rotation);
        shooter.Shoot(0, shotForce, shotCount, shotAngle);
    }
}
