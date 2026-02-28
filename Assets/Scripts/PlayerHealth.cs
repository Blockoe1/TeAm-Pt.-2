using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 3;
    public eggform form = eggform.whole;
    private bool iFrames = false;
    public float iFramesTime;


    public enum eggform
    {
        whole, cracked, yolk
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (iFrames == false && collision.CompareTag("Damage"))
        {
            StartCoroutine(Damaged());
        }
    }

    IEnumerator Damaged()
    {
        iFrames = true;
        health -= 1;
        switch(health)
        {
            case 2:
                form = eggform.cracked; 
                break;
            case 1:
                form = eggform.yolk;
                PMoveStateMngr.Inst.SwitchState(PMoveStateMngr.Inst.YolkState);
                break;
        }
        yield return new WaitForSeconds(iFramesTime);
        iFrames = false;
       
    }
}
