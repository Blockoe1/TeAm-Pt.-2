using System.Collections;
using UnityEngine;

public class KillButter : MonoBehaviour
{
    void Start()
    {
        print("KILL BUTTER");
        StartCoroutine(Killer());   
    }

    public IEnumerator Killer()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
    }
}
