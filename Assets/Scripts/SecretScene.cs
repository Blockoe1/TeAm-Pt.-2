using UnityEngine;

public class SecretScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.EvansRequest);
    }
}
