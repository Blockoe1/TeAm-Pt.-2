using System.Collections;
using UnityEngine;

public class FallingProjectile : Projectile
{
    [SerializeField] private Collider2D hitCollider;
    [SerializeField] private SpriteRenderer telegraphSpot;
    [SerializeField] private AnimationCurve telegraphCurve;
    [SerializeField] private float offsetAmplitude;
    [SerializeField] private AnimationCurve offsetCurve;
    [SerializeField] private bool despawnOnLand;

    private Color baseColor;

    private void Awake()
    {
        baseColor = telegraphSpot.color;
    }

    public override void Launch(Vector2 launchVector)
    {
        if (rb != null)
        {
            rb.transform.position = transform.position;
        }
        telegraphSpot.transform.localPosition = launchVector;

        lifetimeRoutine = StartCoroutine(LaunchTime(launchVector));
    }

    private IEnumerator LaunchTime(Vector2 launchVector)
    {
        telegraphSpot.gameObject.SetActive(true);
        telegraphSpot.color = Color.clear;
        telegraphSpot.transform.localScale = Vector3.zero;

        Vector2 perpVector = new Vector2(launchVector.y, -launchVector.x).normalized;

        hitCollider.enabled = false ;

        float timer = 0;
        while (timer <= despawnTime)
        {
            float normalizedTime = timer / despawnTime;

            // Update the telegraphing spot;
            telegraphSpot.color = Color.Lerp(Color.clear, baseColor, telegraphCurve.Evaluate(normalizedTime));
            telegraphSpot.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, telegraphCurve.Evaluate(normalizedTime));

            // Update the position of the projectile.
            Vector2 offset =  perpVector * offsetAmplitude * offsetCurve.Evaluate(normalizedTime);
            Vector2 toVector = Vector2.Lerp(Vector2.zero, launchVector, normalizedTime);
            rb.MovePosition((Vector2)transform.position + toVector + offset);

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        hitCollider.enabled = true ;
        if(AudioManager.instance!=null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.ButterLands);
        }
        telegraphSpot.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        if (despawnOnLand)
        {
            Despawn();
        }
    }
}
