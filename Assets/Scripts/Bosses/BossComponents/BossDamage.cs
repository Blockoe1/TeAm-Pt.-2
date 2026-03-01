using UnityEngine;

public class BossDamage : MonoBehaviour
{
    [SerializeField] private bool _isProjectile = false;
    [SerializeField] private bool isSolidButter;
    /// <summary>
    /// If player touches the boss. For physical objects
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isProjectile)
        {
            return;
        }

        if (isSolidButter && FindFirstObjectByType<ROllStorage>().PlayerIsRolling)
        {
            Destroy(gameObject);
        }


        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().DealDamage();
        }
    }

    /// <summary>
    /// For non-physical objects (projectiles)
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSolidButter && FindFirstObjectByType<ROllStorage>().PlayerIsRolling)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().DealDamage();
        }
    }
}
