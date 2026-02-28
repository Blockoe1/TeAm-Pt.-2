using UnityEngine;
using UnityEngine.InputSystem;

public class PPanPivot : MonoBehaviour
{
    private void FixedUpdate()
    {
        Rotate();
    }
    private void Rotate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.up = mousePos - new Vector2(transform.position.x, transform.position.y);
    }
}
