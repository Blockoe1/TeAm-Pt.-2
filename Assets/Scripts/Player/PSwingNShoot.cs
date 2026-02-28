using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PSwingNShoot : MonoBehaviour
{
    private InputAction swingAction;
    private Coroutine bufferCoroutine;

    private bool swinging = false;
    private bool swingLeft = false;

    //[SerializeField] private Animator animator;

    [SerializeField] private int _bufferInputTime;

    private void Start()
    {
        swingAction = InputSystem.actions.FindAction("Click");

        swingAction.started += SwingAction_started;
        swingAction.canceled += SwingAction_canceled;
    }

    private void SwingAction_canceled(InputAction.CallbackContext obj)
    {
        if(bufferCoroutine != null)
        {
            StopCoroutine(bufferCoroutine);
            bufferCoroutine = null;
        }
    }

    private void SwingAction_started(InputAction.CallbackContext obj)
    {
        if(bufferCoroutine != null)
        {
            return;
        }

        bufferCoroutine = StartCoroutine(BufferAction());
    }

    private IEnumerator Swing()
    {
        swingLeft = !swingLeft;
        swinging = true;
        //animator.Play(swingLeft ? "Swing" : "SwingBack", 1);
        yield return null;
    }

    private IEnumerator BufferAction()
    {
        int bufferTime = _bufferInputTime;
        while (bufferTime > 0)
        {
            if(!swinging)
            {
                StartCoroutine(Swing());
                bufferCoroutine = null;
                yield break;
            }
            yield return null;
            bufferTime -= 1;
        }

        bufferCoroutine = null;
    }

    private void OnDestroy()
    {
        swingAction.started -= SwingAction_started;
        swingAction.canceled -= SwingAction_canceled;
    }

    public void SwingStop()
    {
        swinging = false;
    }
}
