using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PSwingNShoot : MonoBehaviour
{
    private InputAction swingAction;
    private Coroutine bufferCoroutine;

    private bool swinging = false;
    private bool swingLeft = false;

    [SerializeField] private Animator animator;

    [SerializeField] private int _bufferInputTime;
    [SerializeField] private ProjectileShooter _projectileShooter;

    [SerializeField] private Transform _panPivotTransform;

    [SerializeField] private float _shootPower = 5;
    [SerializeField] private int _shotCount = 1;
    [SerializeField] private float _shotAngle = 0;

    private void Start()
    {
        swingAction = InputSystem.actions.FindAction("CLICK");

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
        _panPivotTransform.gameObject.SetActive(true);
        animator.SetTrigger("ATTACK");
        animator.SetBool("ATTACK_LEFT", !animator.GetBool("ATTACK_LEFT"));
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
        _panPivotTransform.gameObject.SetActive(false);
        animator.ResetTrigger("ATTACK");
        swinging = false;
    }

    public void ShootEgg()
    {
        Vector2 direction = _panPivotTransform.transform.up;
        _projectileShooter.Shoot(direction, _shootPower, _shotCount, _shotAngle);
    }
}
