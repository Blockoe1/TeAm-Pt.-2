using NaughtyAttributes;
using UnityEngine;

public class ShatterForce : MonoBehaviour
{
    [SerializeField] private float shatterForce;
    [SerializeField] private Vector3 explodeOffset;
    [SerializeField, ReadOnly] private Rigidbody[] shardRigidbodies;

    [Button]
    private void GetRigidbodies()
    {
        shardRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    [Button]
    public void Shatter()
    {
        foreach(var rb in shardRigidbodies)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(shatterForce, transform.position + explodeOffset, 10f);
        }
    }
}
