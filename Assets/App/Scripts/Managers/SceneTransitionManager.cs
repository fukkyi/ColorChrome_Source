using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public readonly static string GameSceneName = "BetaScene";
    public readonly static string TitleSceneName = "TitleScene";
    public readonly static string ClearSceneName = "ClearScene";
    public readonly static string OpeningSceneName = "OpeningScenarioScene";

    private static readonly string FadeControllerTagName = "FadeController";
    private static readonly string ManagerName = "SceneTransitionManager";
    private static readonly string TransitionSceneName = "SceneTransition";
    private static readonly float activationProgressValue = 0.9f;

    private static SceneTransitionManager instance = null;
    public static SceneTransitionManager Instance { 
        get {
            if (instance == null)
            {
                GameObject managerObject = new GameObject(ManagerName);
                instance = managerObject.AddComponent<SceneTransitionManager>();

                DontDestroyOnLoad(instance);
            }
            return instance; 
        } 
        private set { instance = value; } 
    }

    /// <summary>
    /// 特定の名前のシーンに遷移させる
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadWaitingTime"></param>
    public void StartTransitionByName(string sceneName, float loadWaitingTime = 0.1f)
    {
        StartCoroutine(TransitionByName(sceneName, loadWaitingTime));
    }

    /// <summary>
    /// 特定の名前のシーンに遷移させる (UnScale)
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadWaitingTime"></param>
    public void StartScaledTransitionByName(string sceneName, float loadWaitingTime = 0.1f)
    {
        StartCoroutine(TransitionByName(sceneName, loadWaitingTime, false));
    }

    /// <summary>
    /// シーンリセットを行うために遷移させる
    /// </summary>
    /// <param name="loadWaitingTime"></param>
    public void StartTransitionForReset(float loadWaitingTime = 0.1f)
    {
        StartCoroutine(TransitionForReset(loadWaitingTime));
    }

    private IEnumerator TransitionByName(string sceneName, float loadWaitingTime, bool unScaledTime = true)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        LoadSceneParameters loadSceneParameters = new LoadSceneParameters();
        loadSceneParameters.loadSceneMode = LoadSceneMode.Additive;

        // 遷移演出用のシーンをロードする
        Scene transitionScene = SceneManager.LoadScene(TransitionSceneName, loadSceneParameters);

        // 1フレーム待たないとゲームオブジェクトを取得できない
        yield return null;

        FadeController fadeController = GameObject.FindWithTag(FadeControllerTagName).GetComponent<FadeController>();

        // フェードアウトさせる
        yield return StartCoroutine(fadeController.Out(null, unScaledTime));

        // シーンをロードする
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while(!loadAsync.isDone)
        {
            yield return null;
        }

        Scene loadScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(loadScene);

        // 読み込んでから少し間を開ける
        yield return new WaitForSecondsRealtime(loadWaitingTime);

        // 前のシーンをアンロードする
        yield return SceneManager.UnloadSceneAsync(currentScene);

        // フェードインさせる
        yield return StartCoroutine(fadeController.In(null, unScaledTime));

        // 遷移演出用のシーンをアンロードする
        yield return SceneManager.UnloadSceneAsync(transitionScene);
    }

    private IEnumerator TransitionForReset(float loadWaitingTime, bool unScaledTime = true)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string resetSceneName = currentScene.name;

        LoadSceneParameters loadSceneParameters = new LoadSceneParameters();
        loadSceneParameters.loadSceneMode = LoadSceneMode.Additive;

        // 遷移演出用のシーンをロードする
        Scene transitionScene = SceneManager.LoadScene(TransitionSceneName, loadSceneParameters);

        // 1フレーム待たないとゲームオブジェクトを取得できない
        yield return null;

        FadeController fadeController = GameObject.FindWithTag(FadeControllerTagName).GetComponent<FadeController>();

        // フェードアウトさせる
        yield return StartCoroutine(fadeController.Out(null, unScaledTime));

        // 前のシーンをアンロードする
        yield return SceneManager.UnloadSceneAsync(currentScene);
        // シーンをロードする
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(resetSceneName, LoadSceneMode.Additive);
        while (!loadAsync.isDone)
        {
            yield return null;
        }

        Scene loadScene = SceneManager.GetSceneByName(resetSceneName);
        SceneManager.SetActiveScene(loadScene);

        // 読み込んでから少し間を開ける
        yield return new WaitForSecondsRealtime(loadWaitingTime);

        // フェードインさせる
        yield return StartCoroutine(fadeController.In(null, unScaledTime));

        // 遷移演出用のシーンをアンロードする
        yield return SceneManager.UnloadSceneAsync(transitionScene);
    }
}
