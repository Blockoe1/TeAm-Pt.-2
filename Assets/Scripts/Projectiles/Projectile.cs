using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected bool despawnOnCollision;
    [SerializeField] protected float despawnTime = 10f;
    protected enum ProjectileType
    {
        FlamingButter, EggRoll, Other
    }

    [SerializeField] private ProjectileType projectileType;
    [field: SerializeField] protected Rigidbody2D rb { get; private set; }

    protected Coroutine lifetimeRoutine;

    public event Action<Projectile> OnDespawn;
    private EventInstance lifetimeSound;

    private async void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        if (projectileType == ProjectileType.FlamingButter && AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.SpawnButter);
            await Task.Delay(1500);
            //lifetimeSound = RuntimeManager.CreateInstance(FMODEvents.instance.ContinuallyBurn);
            //lifetimeSound.start();
        }

        if(projectileType == ProjectileType.EggRoll && AudioManager.instance != null)
        {
            lifetimeSound = RuntimeManager.CreateInstance(FMODEvents.instance.EggRoll);
            lifetimeSound.start();
        }

    }

    public virtual void Launch(Vector2 launchVector)
    {
        rb.AddForce(launchVector, ForceMode2D.Impulse);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(launchVector.y, launchVector.x) * Mathf.Rad2Deg);
        lifetimeRoutine = StartCoroutine(Lifetime());
    }

    protected virtual IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(despawnTime);
        Despawn();
    }

    public void Despawn()
    {
        lifetimeSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        OnDespawn?.Invoke(this);
        if (lifetimeRoutine != null)
        {
            StopCoroutine(lifetimeRoutine);
            lifetimeRoutine = null;
        }
    }

    private void OnDestroy()
    {
        lifetimeSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // On collision with anything, destroy the projectile.
        if (FindFirstObjectByType<AudioManager>() != null && projectileType != ProjectileType.EggRoll)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.CarrotHits);
        }
        if (despawnOnCollision)
        {
            Despawn();
        }
    }
}
