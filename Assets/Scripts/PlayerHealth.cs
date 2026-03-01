using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private int health = 3;
    public eggform form = eggform.whole;
    private bool iFrames = false;
    public float iFramesTime;

    [SerializeField] private float _deathDelay;


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

    public void DealDamage()
    {
        if (iFrames == false)
        {
            StartCoroutine(Damaged());

            //Debug.Log(collision.ClosestPoint(transform.position));
            ParticleMngr.Inst.Play("P_HIT", transform.position, transform.rotation);
        }
    }

    IEnumerator Damaged()
    {
        iFrames = true;
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
        yield return new WaitForSeconds(iFramesTime);
        iFrames = false;
       
    }

    private IEnumerator DeathDelay()
    {
        InputSystem.actions.Disable();
        ParticleMngr.Inst.Play("P_DIE", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_deathDelay);
        InputSystem.actions.Enable();
        SceneManager.LoadScene("DeathScene");
    }
}
