using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor;

    private SpriteRenderer rend;
    private Color baseColor;

    private Coroutine flashRoutine;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        baseColor = rend.color;
    }

    public void Flash(float flashTime)
    {
        if (!gameObject.activeInHierarchy) { return;  }
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
        flashRoutine = StartCoroutine(FlashRoutine(flashTime));
    }

    private IEnumerator FlashRoutine(float flashTime)
    {
        if (flashTime == 0) { yield break; }
        float timer = flashTime;
        rend.color = flashColor;
        while (timer > 0)
        {
            Color color = rend.color;
            color = Color.Lerp(baseColor, flashColor, timer / flashTime);
            rend.color = color;

            timer -= Time.deltaTime;
            yield return null;
        }
    }
}
