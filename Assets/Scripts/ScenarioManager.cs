using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScenarioManager : MonoBehaviour
{
    public enum Scenario { Undefined, MixingBowl = 1, FryingPan = 2, EggCooker  = 3}

    [SerializeField] private Scenario _scenario;
    [SerializeField] private Vector2 _eggyDialoguePos;
    [SerializeField] private Vector2 _bossDialoguePos;
    [SerializeField] private Conversation _beforeBossAppears;
    [SerializeField] private Conversation _beforeFightBegins;
    [SerializeField] private Conversation _afterFightEnds;
    [SerializeField] private Conversation _afterBossDies;
    [SerializeField, Scene] private string _nextScene;

    private bool bossIsDead;

    void Start()
    {
        StartCoroutine(SceneLogic());
    }

    private IEnumerator SceneLogic()
    {
        InputSystem.actions.Disable();
        var player = FindAnyObjectByType<PlayerHealth>();
        var boss = FindAnyObjectByType<BossController>();

        // Disable spin script
        if (_scenario == Scenario.FryingPan) FindAnyObjectByType<Spin>().enabled = false;

        // Dialogue before boss appears
        if (_beforeBossAppears != null)
        {
            boss.gameObject.SetActive(false);
            yield return DialogueManager.Instance.RunDialogue(_beforeBossAppears);
            boss.gameObject.SetActive(true);
        }

        // Initial exchange with boss
        yield return DialogueManager.Instance.RunDialogue(_beforeFightBegins);
        DialogueManager.Instance.DisableDialogueCamera();
        yield return WaitForCameraSwitch();
        InputSystem.actions.Enable();
        boss.Startup();
        if (_scenario == Scenario.FryingPan) FindAnyObjectByType<Spin>().enabled = true;

        // Boss fight happens
        yield return new WaitUntil(() => bossIsDead);
        DialogueManager.Instance.EnableDialogueCamera();

        // Cripple the characters
        InputSystem.actions.Disable();
        var bossObject = boss.gameObject;
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

        // Move characters back
        float t = 0;
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
