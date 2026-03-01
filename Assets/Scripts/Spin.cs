using NaughtyAttributes;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
public class Spin : MonoBehaviour
{
    public int spinAmount;
    [SerializeField] private Animator anim;
    [SerializeField] private bool isBoss;
    [ShowIf(nameof(isBoss)), SerializeField] public EventReference MovementSound;

    private Coroutine MoveSound;
    private EventInstance move;
    private void OnEnable()
    {
        if (anim != null)
        {
            anim.SetBool("IsSpinning", true);
        }
    }
    private void OnDisable()
    {
        if (anim != null)
        {
            anim.SetBool("IsSpinning", false);
        }
    }

    private void Update()
    {
        if(isBoss && MoveSound == null)
        {
            MoveSound = StartCoroutine(PlaySound());
        }
        transform.Rotate(0, 0, spinAmount * Time.deltaTime);
    }


    /// <summary>
    /// This is so scuffed but it works
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlaySound()
    {
        while (true)
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayOneShot(MovementSound);
            }

            yield return new WaitForSeconds(.3f);
        }



    }
}
