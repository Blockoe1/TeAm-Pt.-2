using UnityEngine;

public class MixingBowlMovement : MonoBehaviour
{
    [SerializeField] private Transform[] movePoints;

    private Animator anim;
    private BossMovement movement;
    private int currentPoint = -1;

    private void Awake()
    {
        movement = GetComponent<BossMovement>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetFloat("Speed", movement.Rb.linearVelocity.magnitude);
    }

    public void MoveToNext()
    {
        currentPoint = (currentPoint + 1) % movePoints.Length;
        movement.SetMoveTarget(movePoints[currentPoint].position);
    }
}
