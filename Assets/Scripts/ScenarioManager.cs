using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScenarioManager : MonoBehaviour
{
    public enum Scenario { Undefined, MixingBowl = 1, FryingPan = 2, EggCooker = 3, Intro }

    [SerializeField] private Scenario _scenario;
    [SerializeField] private Vector2 _eggyDialoguePos;
    [SerializeField] private Vector2 _bossDialoguePos;
    [SerializeField] private Conversation _beforeBossAppears;
    [SerializeField] private Conversation _beforeFightBegins;
    [SerializeField] private Conversation _afterFightEnds;
    [SerializeField] private Conversation _afterBossDies;
    [SerializeField] private SpriteRenderer _bossDeathFlash;
    [SerializeField] private float _bossDeathTime;
    [SerializeField, Scene] private string _nextScene;

    private bool bossIsDead;

    void Start()
    {
        StartCoroutine(SceneLogic());
    }

    private IEnumerator SceneLogic()
    {
        if (_scenario == Scenario.Intro)
        {
            yield return IntroScene();
        }
        else
        {
            yield return BossScene();
        }
    }

    private IEnumerator IntroScene()
    {
        yield return new WaitForSeconds(1);
        yield return DialogueManager.Instance.RunDialogue(_beforeFightBegins);
        TransitionManager.ZoomTransition(_nextScene);
    }

    private IEnumerator BossScene()
    {
        InputSystem.actions.Disable();
        var player = FindAnyObjectByType<PlayerHealth>();
        var boss = FindAnyObjectByType<BossController>();

        // Face upward
        FindAnyObjectByType<PMoveStateMngr>().ForceUpwardFace(true);

        // Disable spin script
        if (_scenario == Scenario.FryingPan) FindAnyObjectByType<Spin>().enabled = false;

        // Dialogue before boss appears
        if (_beforeBossAppears != null)
        {
            boss.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            yield return DialogueManager.Instance.RunDialogue(_beforeBossAppears);
            boss.gameObject.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(1);
        }

        // Initial exchange with boss
        yield return DialogueManager.Instance.RunDialogue(_beforeFightBegins);
        DialogueManager.Instance.DisableDialogueCamera();
        yield return WaitForCameraSwitch();
        InputSystem.actions.Enable();
        FindAnyObjectByType<PMoveStateMngr>().ForceUpwardFace(false);
        boss.Startup();
        if (_scenario == Scenario.FryingPan) FindAnyObjectByType<Spin>().enabled = true;

        // Boss fight happens
        yield return new WaitUntil(() => bossIsDead);

        // Cripple the characters
        InputSystem.actions.Disable();
        var bossObject = boss.gameObject;
        Destroy(boss.GetComponent<BossMovement>());
        boss.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        Destroy(boss);
        if (_scenario == Scenario.FryingPan)
        {
            for (int i = 1; i < bossObject.transform.parent.childCount; i++)
            {
                Destroy(bossObject.transform.parent.GetChild(i).gameObject);
            }
            FindAnyObjectByType<Spin>().enabled = false;
        }
        else
        {
            for (int i = 0; i < bossObject.transform.childCount; i++)
            {
                Destroy(bossObject.transform.GetChild(i).gameObject);
            }
        }

        // Boss death flash
        _bossDeathFlash.gameObject.SetActive(true);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / (_bossDeathTime / 2);
            _bossDeathFlash.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t);
            yield return null;
        }
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / (_bossDeathTime / 2);
            _bossDeathFlash.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), t);
            yield return null;
        }
        _bossDeathFlash.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        DialogueManager.Instance.EnableDialogueCamera();
        FindAnyObjectByType<PMoveStateMngr>().ForceUpwardFace(true);

        // Move characters back
        t = 0;
        var playerPos = player.transform.position;
        Vector3 bossPos;
        if (_scenario == Scenario.FryingPan) bossPos = bossObject.transform.parent.position;
        else bossPos = bossObject.transform.position;
        while (t < 1)
        {
            t += Time.deltaTime;
            player.transform.position = Vector2.Lerp(playerPos, _eggyDialoguePos, t);
            bossObject.transform.position = Vector2.Lerp(bossPos, _bossDialoguePos, t);
            yield return null;
        }

        // Post-fight dialogue
        yield return new WaitUntil(() => !CinemachineBrain.GetActiveBrain(0).IsBlending);
        yield return DialogueManager.Instance.RunDialogue(_afterFightEnds);

        // Boss dies
        ParticleMngr.Inst.Play("BOSS" + (int)_scenario + "_DIE", bossObject.transform.position, Quaternion.identity);
        bossObject.SetActive(false);
        yield return new WaitForSeconds(2);
        yield return DialogueManager.Instance.RunDialogue(_afterBossDies);

        TransitionManager.ShatterTransition(_nextScene);
    }

    public void BossIsDead()
    {
        bossIsDead = true;
    }

    private IEnumerator WaitForCameraSwitch()
    {
        yield return new WaitUntil(() => CinemachineBrain.GetActiveBrain(0).IsBlending);
        yield return new WaitUntil(() => !CinemachineBrain.GetActiveBrain(0).IsBlending);
    }
}
