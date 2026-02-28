using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueEpic : MonoBehaviour
{
    [SerializeField, ShowIf("_runOnStart")] private Conversation _conversation;

    [SerializeField] private Image _mask;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _continue;

    [SerializeField] private bool _runOnStart;

    private bool continueTime;

    private float[] textOffsets = { -1, -.9f, -.8f, -.7f, -.6f, -.5f, -.4f, -.3f, -.2f, -.1f, 0,
        .1f, .2f, .3f, .4f, .5f, .6f, .7f, .8f, .9f, 1, .9f, .8f, .7f, .6f, .5f, .4f, .3f, .2f, .1f, 0,
        -.1f, -.2f, -.3f, -.4f, -.5f, -.6f, -.7f, -.8f, -.9f };
    //private float[] textOffsets = { -1, -0.75f, -0.5f, -0.25f, 0, 0.25f, 0.5f, 0.75f, 1, 0.75f, 0.5f, 0.25f, 0, -0.25f, -0.5f, -0.75f };
    //private float[] textOffsets = { 0, 0.25f, 0.5f, 0.75f, 1 };

    void Start()
    {
        if (_runOnStart)
        {
            StartCoroutine(SayWords(_conversation));
        }
    }

    public IEnumerator SayWords(Conversation c)
    {
        foreach (var word in c.TheWords)
        {
            yield return SayWord(word);
        }

        Destroy(gameObject);
    }

    private IEnumerator SayWord(Conversation.Line line)
    {
        continueTime = false;
        _continue.gameObject.SetActive(false);

        _text.text = line.OneMeaslyLine;

        if (line.EnterVertically)
        {
            _mask.GetComponent<Mask>().enabled = true;
            _mask.enabled = true;
            _text.transform.eulerAngles = new Vector3(0, 0, 90);
            _text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1000);

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
                t += Time.deltaTime / line.VerticalTime;
                _text.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }

            _mask.GetComponent<Mask>().enabled = false;
            _mask.enabled = false;
        }
        else
        {
            _text.GetComponent<RectTransform>().anchoredPosition = _mask.GetComponent<RectTransform>().anchoredPosition;
        }

        yield return new WaitForSeconds(line.PauseTime);

        if (line.Spin)
        {
            int degrees = 90 + (line.SpinCount * 360);
            float degreesPerSecond = degrees / line.SpinTime;
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / line.SpinTime;
                _text.transform.eulerAngles -= new Vector3(0, 0, degreesPerSecond * Time.deltaTime);
                yield return null;
            }
        }

        _text.transform.eulerAngles = Vector3.zero;

        Coroutine swish = null;
        if (line.Swish == true)
        {
            swish = StartCoroutine(TextSwish(line));
        }

        _continue.gameObject.SetActive(true);
        yield return new WaitUntil(() => continueTime);

        if (swish != null)
        {
            StopCoroutine(swish);
        }
    }

    public IEnumerator TextSwish(Conversation.Line line)
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
                current += "<voffset=" + (Mathf.Sin(textOffsets[offsetIndex] * 2 * Mathf.PI * Mathf.Deg2Rad) * line.SwishAmplitude) + "em>" + original[index];
                index++;
                offsetIndex = (offsetIndex + 1) % textOffsets.Length;
                //if (index == 1) print("<noparse>" + current + "</noparse>");
            }

            //print("<noparse>" + current + "</noparse>");
            _text.text = current;
            initialOffsetIndex = (initialOffsetIndex + 1) % textOffsets.Length;

            yield return new WaitForSeconds(line.SwishWaitInterval);
        }

    }

    private void Update()
    {
        print(Time.deltaTime);
    }

    public void Continue()
    {
        continueTime = true;
    }
}
