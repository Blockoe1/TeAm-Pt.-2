using UnityEngine;

public class PDamage : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private bool _pan;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BossHealth boss))
        {
            boss.Damage(_damage);

            if(_pan)
                ParticleMngr.Inst.Play("PAN_HIT", transform.position, transform.rotation);

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerMeleeAttack);
            }
        }
    }
}
