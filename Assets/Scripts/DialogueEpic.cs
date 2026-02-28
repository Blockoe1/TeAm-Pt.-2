using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueEpic : MonoBehaviour
{
    [SerializeField] private Image _mask;
    [SerializeField] private TMP_Text _text;

    [SerializeField] private float _verticalTime;
    [SerializeField] private float _pauseTime;
    [SerializeField] private float _swishWaitInterval;
    [SerializeField] private float _swishAmplitude;

    private float[] textOffsets = { -1, -.9f, -.8f, -.7f, -.6f, -.5f, -.4f, -.3f, -.2f, -.1f, 0,
        .1f, .2f, .3f, .4f, .5f, .6f, .7f, .8f, .9f, 1, .9f, .8f, .7f, .6f, .5f, .4f, .3f, .2f, .1f, 0,
        -.1f, -.2f, -.3f, -.4f, -.5f, -.6f, -.7f, -.8f, -.9f };
    //private float[] textOffsets = { -1, -0.75f, -0.5f, -0.25f, 0, 0.25f, 0.5f, 0.75f, 1, 0.75f, 0.5f, 0.25f, 0, -0.25f, -0.5f, -0.75f };
    //private float[] textOffsets = { 0, 0.25f, 0.5f, 0.75f, 1 };

    public Conversation test;

    void Start()
    {
        StartCoroutine(SayWords());
    }

    private IEnumerator SayWords()
    {
        yield return SayWord(test.TheWords[0]);
    }

    private IEnumerator SayWord(Conversation.Line line)
    {
        _mask.GetComponent<Mask>().enabled = true;
        _mask.enabled = true;
        _text.transform.eulerAngles = new Vector3(0, 0, 90);
        _text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1000);
        _text.text = line.OneMeaslyLine;

        yield return null;
        _text.GetComponent<RectTransform>().anchoredPosition = _mask.GetComponent<RectTransform>().anchoredPosition + 
            (new Vector2(0, _mask.GetComponent<RectTransform>().sizeDelta.y / 2) +
            new Vector2(0, _text.GetComponent<RectTransform>().sizeDelta.x / 2)) *
            (line.Direction == Conversation.Direction.Top ? 1 : -1);
        Vector2 startPos = _text.GetComponent<RectTransform>().anchoredPosition;
        Vector2 endPos = _mask.GetComponent<RectTransform>().anchoredPosition;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / _verticalTime;
            _text.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        _mask.GetComponent<Mask>().enabled = false;
        _mask.enabled = false;

        yield return new WaitForSeconds(_pauseTime);

        int degrees = 90 + (line.SpinCount * 360);
        float degreesPerSecond = degrees / line.SpinTime;
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / line.SpinTime;
            _text.transform.eulerAngles -= new Vector3(0, 0, degreesPerSecond * Time.deltaTime);
            yield return null;
        }

        _text.transform.eulerAngles = Vector3.zero;

        yield return TextSwish();
    }

    public IEnumerator TextSwish()
    {
        string original = _text.text;
        int initialOffsetIndex = 0;
        
        while (true)
        {
            string current = string.Empty;
            int index = 0;
            int offsetIndex = initialOffsetIndex;
            while (index < original.Length)
            {
                current += "<voffset=" + (Mathf.Sin(textOffsets[offsetIndex] * 2 * Mathf.PI * Mathf.Deg2Rad) * _swishAmplitude) + "em>" + original[index];
                index++;
                offsetIndex = (offsetIndex + 1) % textOffsets.Length;
                if (index == 1) print("<noparse>" + current + "</noparse>");
            }

            _text.text = current;
            initialOffsetIndex = (offsetIndex + 1) % textOffsets.Length;

            yield return new WaitForSeconds(_swishWaitInterval);
        }

    }
}
