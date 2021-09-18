using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSetter : MonoBehaviour
{
    [SerializeField]
    private Enemy[] bossSpawnEnemies = null;

    public CountableMission mushKillMission;
    public Mission mushVillageMission;
    public Mission killBossMission;

    private void Start()
    {
        MissionManager.Instance.ResetMission();
        MissionManager.Instance.AddMission(mushKillMission);
    }

    public void AddMushVillageMission()
    {
        TalkCanvasManager.Instance.ShowMidwayScenario();
        MissionManager.Instance.AddMission(mushVillageMission);
    }

    public void AddKillBossMission()
    {
        foreach(Enemy enemy in bossSpawnEnemies)
        {
            enemy.gameObject.SetActive(true);
        }

        GameSceneUIManager.Instance.BossHPGauge.ShowGauge();

        MissionManager.Instance.AddMission(killBossMission);
    }

    public void ComplateKillBossMission()
    {
        MissionManager.Instance.ComplateMission(MissionName.KillMushBoss);
        StartCoroutine(ComplateGame());
    }

    private IEnumerator ComplateGame()
    {
        GameSceneManager.Instance.isGameClear = true;

        yield return new WaitForSeconds(3.0f);

        TalkCanvasManager.Instance.ShowClearScenario();

        yield return new WaitForSeconds(0.5f);

        SceneTransitionManager.Instance.StartTransitionByName(SceneTransitionManager.ClearSceneName);
        AudioManager.Instance.StopCurrentBGMWithFade();
    }
}
