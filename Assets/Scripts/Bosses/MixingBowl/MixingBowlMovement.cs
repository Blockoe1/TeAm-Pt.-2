using UnityEngine;

public class MixingBowlMovement : MonoBehaviour
{
    [SerializeField] private Transform[] movePoints;

    private BossMovement movement;
    private int currentPoint = -1;

    private void Awake()
    {
        movement = GetComponent<BossMovement>();
    }

    public void MoveToNext()
    {
        currentPoint = (currentPoint + 1) % movePoints.Length;
        movement.SetMoveTarget(movePoints[currentPoint].position);
    }
}
