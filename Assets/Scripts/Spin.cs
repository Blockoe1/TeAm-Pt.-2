using UnityEngine;

public class Spin : MonoBehaviour
{
    public int spinAmount;
    private void Update()
    {
        transform.Rotate(0, 0, spinAmount * Time.deltaTime);
    }
}
