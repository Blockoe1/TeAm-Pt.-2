using UnityEngine;

public class PanMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private BossMovement movement;

    private void Awake()
    {
        movement = GetComponent<BossMovement>();
    }

    private void FixedUpdate()
    {
        movement.SnapRotation();
        movement.TargetVelocity = Vector2.up * moveSpeed;
    }

    private void OnDisable()
    {
        movement.TargetVelocity = Vector2.zero;
    }
}
