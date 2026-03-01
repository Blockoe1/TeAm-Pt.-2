using UnityEngine;
using UnityEngine.Events;

public class PDamage : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private bool _pan;
    [SerializeField] private UnityEvent OnPanHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BossHealth boss))
        {
            boss.Damage(_damage);

            if(_pan)
            {
                ParticleMngr.Inst.Play("PAN_HIT", transform.position, transform.rotation);
                OnPanHit?.Invoke();
            }

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
