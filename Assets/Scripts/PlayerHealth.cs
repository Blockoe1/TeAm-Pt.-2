using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private int health = 3;
    public eggform form = eggform.whole;
    private bool iFrames = false;
    public float iFramesTime;

    [SerializeField] private float _deathDelay;
    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnDie;

    private Rigidbody2D rb;


    public enum eggform
    {
        whole, cracked, yolk
    }

    //We should be calling this on the enemy's projectile and body...
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (iFrames == false && collision.CompareTag("Damage"))
    //    {
    //        StartCoroutine(Damaged());

    //        Debug.Log(collision.ClosestPoint(transform.position));
    //        ParticleMngr.Inst.Play("YOLK", transform.position, transform.rotation);
    //    }
    //}

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void DealDamage()
    {
        if (iFrames == false)
        {
            Damaged();

            //Debug.Log(collision.ClosestPoint(transform.position));
            ParticleMngr.Inst.Play("P_HIT", transform.position, transform.rotation);
        }
    }

    void Damaged()
    {
        health -= 1;

        switch(health)
        {
            case 2:
                PMoveStateMngr.Inst.CurOC = PMoveStateMngr.Inst.CrackedAnimOCs;
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerCrack);
                }
                form = eggform.cracked; 
                break;
            case 1:
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerBecomesYolk);
                }
                
                form = eggform.yolk;
                PMoveStateMngr.Inst.SwitchState(PMoveStateMngr.Inst.YolkState);
                break;
            case 0:
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerBecomesYolk);
                }
                StartCoroutine(DeathDelay());
                break;
        }
        OnTakeDamage?.Invoke();
        IFrames(iFramesTime);
    }

    public void IFrames(float time)
    {
        if (iFrames) { return; }
        StartCoroutine(IFramesRoutine(time));
    }

    private IEnumerator IFramesRoutine(float time)
    {
        iFrames = true;
        rb.excludeLayers = rb.excludeLayers | LayerMask.GetMask("Boss");
        yield return new WaitForSeconds(time);
        rb.excludeLayers = rb.excludeLayers & ~LayerMask.GetMask("Boss");
        iFrames = false;
    }

    private IEnumerator DeathDelay()
    {
        InputSystem.actions.Disable();
        ParticleMngr.Inst.Play("P_DIE", transform.position, Quaternion.identity);
        OnDie?.Invoke();
        yield return new WaitForSeconds(_deathDelay);
        InputSystem.actions.Enable();
        SceneManager.LoadScene("DeathScene");
    }
}
