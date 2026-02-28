using UnityEngine;

public class ButterSpread : MonoBehaviour
{
    public GameObject butter;

    private void Update()
    {
        if (FindAnyObjectByType<BossHealth>().health < FindAnyObjectByType<BossHealth>().maxHealth * 2 / 3)
        {
            butter.SetActive(true);
        }
    }
}
