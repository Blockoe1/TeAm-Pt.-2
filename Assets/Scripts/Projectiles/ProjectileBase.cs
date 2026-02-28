using System;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public Action<ProjectileBase> despawnAction;

    public void SetDespawnAction(Action<ProjectileBase> despawnAction)
    {
        this.despawnAction = despawnAction;
    }

    public abstract void Launch(Vector2 launchVector);


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
