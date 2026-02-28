using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject _dialoguePrefab;

    void Start()
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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
