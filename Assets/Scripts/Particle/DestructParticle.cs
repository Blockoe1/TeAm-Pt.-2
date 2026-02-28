using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestructParticle : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    private void FixedUpdate()
    {
        if (!particleSystem.isPlaying)
            Destroy(gameObject);
    }
}
