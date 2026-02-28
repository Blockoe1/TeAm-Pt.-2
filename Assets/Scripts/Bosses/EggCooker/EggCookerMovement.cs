using UnityEngine;

public class EggCookerMovement : MonoBehaviour
{
    private BossMovement movement;
    private Rigidbody2D rb;

    private void Awake()
    {
        movement = GetComponent<BossMovement>();
    }

    public void Charge()
    {
        movement.Charge();
    }
}
