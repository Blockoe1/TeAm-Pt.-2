using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform projectileParent;

    private readonly Queue<Projectile> projectilePool = new();

    public void Shoot(Vector2 direction, float power, int shotAmount = 1, float spreadAngle = 0)
    {
        Shoot(direction, power, transform.position, shotAmount, spreadAngle);
    }

    public void Shoot(float startingAngle, float power, int shotAmount = 1, float spreadAngle = 0)
    {
        Shoot(startingAngle, power, transform.position, shotAmount, spreadAngle);
    }

    public void Shoot(Vector2 direction, float power, Vector2 spawnLocation, int shotAmount = 1, float spreadAngle = 0)
    {
        float startingAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Shoot(startingAngle, power, transform.position, shotAmount, spreadAngle);
    }

    public void Shoot(float startingAngle, float power, Vector2 spawnLocation, int shotAmount = 1, float spreadAngle = 0)
    {
        float stepAngle = shotAmount > 1 ? spreadAngle / (shotAmount - 1) : 0;
        
        Debug.Log(startingAngle);

        for (int i = 0; i < shotAmount; i++)
        {
            float angle = startingAngle - (spreadAngle / 2) + (stepAngle * i);
            Debug.Log(angle);
            Vector2 launchVector = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            Projectile projectile = GetProjectile();
            projectile.transform.position = spawnLocation;
            projectile.transform.eulerAngles = new Vector3(0, 0, angle);
            projectile.gameObject.SetActive(true);

            projectile.SetDespawnAction(ReturnProjectile);
            projectile.Launch(launchVector.normalized * power);
        } 
    }

    public void RainingDown(Vector2 minValues, Vector2 maxValues)
    {
        Projectile projectile = GetProjectile();
        projectile.transform.position = new Vector2(UnityEngine.Random.Range(minValues.x,maxValues.x),UnityEngine.Random.Range(minValues.y,maxValues.y));
        projectile.gameObject.SetActive(true);
    }

    #region Object Pooling
    private Projectile GetProjectile()
    {
        Projectile toReturn = projectilePool.Count > 0 ? projectilePool.Dequeue() : 
            Instantiate(projectilePrefab, projectileParent);
        return toReturn;
    }
    private void ReturnProjectile(Projectile toReturn)
    {
        projectilePool.Enqueue(toReturn);
        toReturn.gameObject.SetActive(false);
    }
    #endregion
}
