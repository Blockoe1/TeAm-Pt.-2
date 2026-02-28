using UnityEngine;
using static Conversation;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject _dialoguePrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RunDialogue(Conversation c)
    {
        var epic = Instantiate(_dialoguePrefab, Camera.main.transform).GetComponent<DialogueEpic>();
        epic.StartCoroutine(epic.SayWords(c));
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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
