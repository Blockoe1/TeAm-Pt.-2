using UnityEngine;

public class PDamage : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BossHealth boss))
        {
            print("Form: " + FindAnyObjectByType<PlayerHealth>().form);
            if (FindAnyObjectByType<PlayerHealth>().form == PlayerHealth.eggform.yolk)
            {
                boss.Damage(_damage * 2);
            }
            else
            {
                boss.Damage(_damage);
            }
            if (AudioManager.instance != null && !gameObject.name.Contains("Projectile"))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerMeleeAttack);
            }
        }
    }
}
