using UnityEngine;

public class EggMeteor : MonoBehaviour
{
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private Projectile proj;
    [SerializeField] private ProjectileShooter shooter;
    [SerializeField] private float shotForce;
    [SerializeField] private int shotCount;
    [SerializeField] private float shotAngle;

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
        shooter.Shoot(0, shotForce, projectilePoint.position, shotCount, shotAngle);
    }
}
