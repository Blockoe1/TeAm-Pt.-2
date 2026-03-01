using UnityEngine;

public class TriggerRelay : MonoBehaviour
{
    [SerializeField] private FallingProjectile relayTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        relayTarget.OnTriggerEnter2D(collision);
    }
}
