using NaughtyAttributes;
using UnityEngine;

public class ShatterTransition : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private float shatterForce;
    [SerializeField] private Vector3 explodeOffset;
    [SerializeField, ReadOnly] private Rigidbody[] shardRigidbodies;

    [Button]
    private void GetRigidbodies()
    {
        shardRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    public void HideBackground()
    {
        background.SetActive(false);
    }

    [Button]
    public void Shatter()
    {
        foreach(var rb in shardRigidbodies)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(shatterForce, transform.position + explodeOffset, 10f, 0, ForceMode.Impulse);
        }
    }
}
