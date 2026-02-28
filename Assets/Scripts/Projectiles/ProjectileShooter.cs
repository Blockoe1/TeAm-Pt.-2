using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private ProjectileBase projectilePrefab;
    [SerializeField] private Transform projectileParent;

    private readonly Queue<ProjectileBase> projectilePool = new();

    public void Launch(Vector2 mainLaunchVector)
    {
        Launch(mainLaunchVector, transform.position);
    }

    public void Launch(Vector2 mainLaunchVector, Vector2 spawnLocation, int shotAmount = 1, float spreadAngle = 0)
    {
        float stepAngle = spreadAngle / shotAmount;
        float startingAngle = Mathf.Atan2(mainLaunchVector.y, mainLaunchVector.x);

        for(int i = 0; i < shotAmount; i++)
        {
            float angle = startingAngle - (spreadAngle / 2) + (stepAngle * i);
            Vector2 launchVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            ProjectileBase projectile = GetProjectile();
            projectile.transform.position = spawnLocation;
            projectile.gameObject.SetActive(true);

            projectile.SetDespawnAction(ReturnProjectile);
            projectile.Launch(launchVector.normalized * mainLaunchVector.magnitude);
        } 
    }

    #region Object Pooling
    private ProjectileBase GetProjectile()
    {
        ProjectileBase toReturn = projectilePool.Count > 0 ? projectilePool.Dequeue() : 
            Instantiate(projectilePrefab, projectileParent);
        return toReturn;
    }
    private void ReturnProjectile(ProjectileBase toReturn)
    {
        projectilePool.Enqueue(GetProjectile());
        toReturn.gameObject.SetActive(false);
    }
    #endregion
}
