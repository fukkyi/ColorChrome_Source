using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: 試遊会用の仮クラス
/// </summary>
public class TalkCanvasManager : BaseSceneManager<TalkCanvasManager>
{
    public bool isEnableScenario = false;

    [SerializeField]
    private GameObject prologueScenario = null;
    [SerializeField]
    private GameObject midwayScenario = null;
    [SerializeField]
    private GameObject clearScenario = null;

    private void ShowScenario(GameObject scenarioObj)
    {
        if (scenarioObj == null) return;
        if (isEnableScenario) return;

        GameObject generatedSceneraio = Instantiate(scenarioObj, transform);
        StartCoroutine(WaitForScenario(generatedSceneraio));
    }

    public void ShowPrologueScenario()
    {
        ShowScenario(prologueScenario);
    }

    public void ShowMidwayScenario()
    {
        ShowScenario(midwayScenario);
    }

    public void ShowClearScenario()
    {
        ShowScenario(clearScenario);
    }

    /// <summary>
    /// シナリオが終了するまで待つ
    /// </summary>
    /// <param name="scenarioObj"></param>
    /// <returns></returns>
    private IEnumerator WaitForScenario(GameObject scenarioObj)
    {
        isEnableScenario = true;

        Time.timeScale = 0;

        while(!scenarioObj.activeSelf)
        {
            Debug.Log(scenarioObj.activeSelf);
            yield return null;
        }

        while(scenarioObj.activeSelf)
        {
            Debug.Log(scenarioObj.activeSelf);
            yield return null;
        }

        Time.timeScale = 1.0f;
        Destroy(scenarioObj);

        isEnableScenario = false;
    }
}
