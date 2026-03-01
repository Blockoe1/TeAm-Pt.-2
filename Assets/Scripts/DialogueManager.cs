using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject _dialoguePrefab;
    [SerializeField] private GameObject _dialogueCamera;

    [SerializeField] private List<Speaker> _speakers;
    private Dictionary<SpeakerName, Speaker> nameToSpeaker;

    public enum SpeakerName { Undefined, Eggy, MixingBowl, FryingPan, HappyPan, EggCooker, HappyCooker }

    [Serializable]
    public class Speaker
    {
        public SpeakerName Name;
        public TMP_FontAsset Font;
    }

    void Awake()
    {
        /*if (Instance == null)
        {*/
            Instance = this;

            nameToSpeaker = new Dictionary<SpeakerName, Speaker>();
            foreach (Speaker s in _speakers)
            {
                nameToSpeaker.Add(s.Name, s);
            }
        /*}
        else
        {
            Destroy(gameObject);
        }*/
    }

    public void EnableDialogueCamera()
    {
        _dialogueCamera.SetActive(true);
    }

    public void DisableDialogueCamera()
    {
        _dialogueCamera.SetActive(false);
    }

    public IEnumerator RunDialogue(Conversation c)
    {
        var epic = Instantiate(_dialoguePrefab, Camera.main.transform).GetComponent<DialogueEpic>();
        epic.transform.localPosition = new Vector3(0, 0, 10);
        epic.StartCoroutine(epic.SayWords(c));
        yield return new WaitUntil(() => epic == null);
    }

    public Speaker GetSpeaker(SpeakerName name)
    {
        nameToSpeaker.TryGetValue(name, out var speaker);
        return speaker;
    }

    public string Eggify(string s)
    {
        for (int i = 0; i < s.Length - 2; i++)
        {
            if ((s.Substring(i, 3).ToLower() == "egg") && ((s.Substring(i).Length < 11) || (s.Substring(i, 11).ToLower() != "EggGradient")))
            {
                s = s.Substring(0, i) + "<gradient=\"EggGradient\">" + s.Substring(i, 3) + "</gradient>" + s.Substring(i + 3);
                i += 30;
            }
        }
        return s;
    }

    /*private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }*/
}
