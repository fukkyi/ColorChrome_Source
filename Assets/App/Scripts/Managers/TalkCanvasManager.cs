using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: かなり簡易的に作ったクラス
/// </summary>
public class TalkCanvasManager : BaseSceneManager<TalkCanvasManager>
{
    public bool isEnableScenario = false;

    [SerializeField]
    private GameObject findKeoScenario = null;
    [SerializeField]
    private GameObject findHyoukaScenario = null;
    [SerializeField]
    private GameObject winHyoukaScenario = null;
    [SerializeField]
    private GameObject backKeoScenario = null;
    [SerializeField]
    private GameObject killEnemyScenario = null;
    [SerializeField]
    private GameObject backKeo2Scenario = null;
    [SerializeField]
    private GameObject happyEpilogueScenario = null;
    [SerializeField]
    private GameObject badEpilogueScenario = null;
    [SerializeField]
    private float playingBgmVolume = 0.3f;

    private void ShowScenario(GameObject scenarioObj)
    {
        if (scenarioObj == null) return;
        if (isEnableScenario) return;

        GameObject generatedSceneraio = Instantiate(scenarioObj, transform);
        StartCoroutine(WaitForScenario(generatedSceneraio));
    }

    public void ShowFindKeoScenario()
    {
        ShowScenario(findKeoScenario);
    }

    public void ShowFindHyoukaScenario()
    {
        ShowScenario(findHyoukaScenario);
    }

    public void ShowWinHyoukaScenario()
    {
        ShowScenario(winHyoukaScenario);
    }

    public void ShowBackKeoScenario()
    {
        ShowScenario(backKeoScenario);
    }

    public void ShowKillEnemyScenario()
    {
        ShowScenario(killEnemyScenario);
    }

    public void ShowBackKeo2Scenario()
    {
        ShowScenario(backKeo2Scenario);
    }

    public void ShowHappyEpilogueScenario()
    {
        ShowScenario(happyEpilogueScenario);
    }

    public void ShowBadEpilogueScenario()
    {
        ShowScenario(badEpilogueScenario);
    }

    /// <summary>
    /// シナリオが終了するまで待つ
    /// </summary>
    /// <param name="scenarioObj"></param>
    /// <returns></returns>
    private IEnumerator WaitForScenario(GameObject scenarioObj)
    {
        AudioManager.Instance.ChangeVolumeCurrentBGM(playingBgmVolume);

        isEnableScenario = true;

        Time.timeScale = 0;

        while(!scenarioObj.activeSelf)
        {
            yield return null;
        }

        while(scenarioObj.activeSelf)
        {
            yield return null;
        }

        Time.timeScale = 1.0f;
        Destroy(scenarioObj);

        isEnableScenario = false;

        AudioManager.Instance.ChangeVolumeCurrentBGM(SoundObject.MaxVolume);
    }
}
