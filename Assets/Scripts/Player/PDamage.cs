using UnityEngine;

public class PDamage : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BossHealth boss))
        {
            boss.Damage(_damage);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerMeleeAttack);
        }
    }
}
