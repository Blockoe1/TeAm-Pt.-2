using NaughtyAttributes;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
public class Nips : MonoBehaviour
{
    public int spinAmount;

    private void Update()
    {
        transform.Rotate(0, 0, spinAmount * Time.deltaTime);
    }
}
