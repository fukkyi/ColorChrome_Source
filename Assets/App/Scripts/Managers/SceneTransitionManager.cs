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
    /// ����̖��O�̃V�[���ɑJ�ڂ�����
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadWaitingTime"></param>
    public void StartTransitionByName(string sceneName, float loadWaitingTime = 0.1f)
    {
        StartCoroutine(TransitionByName(sceneName, loadWaitingTime));
    }

    /// <summary>
    /// ����̖��O�̃V�[���ɑJ�ڂ����� (UnScale)
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadWaitingTime"></param>
    public void StartScaledTransitionByName(string sceneName, float loadWaitingTime = 0.1f)
    {
        StartCoroutine(TransitionByName(sceneName, loadWaitingTime, false));
    }

    /// <summary>
    /// �V�[�����Z�b�g���s�����߂ɑJ�ڂ�����
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

        // �J�ډ��o�p�̃V�[�������[�h����
        Scene transitionScene = SceneManager.LoadScene(TransitionSceneName, loadSceneParameters);

        // 1�t���[���҂��Ȃ��ƃQ�[���I�u�W�F�N�g���擾�ł��Ȃ�
        yield return null;

        FadeController fadeController = GameObject.FindWithTag(FadeControllerTagName).GetComponent<FadeController>();

        // �t�F�[�h�A�E�g������
        yield return StartCoroutine(fadeController.Out(null, unScaledTime));

        // �V�[�������[�h����
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while(!loadAsync.isDone)
        {
            yield return null;
        }

        Scene loadScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(loadScene);

        // �ǂݍ���ł��班���Ԃ��J����
        yield return new WaitForSecondsRealtime(loadWaitingTime);

        // �O�̃V�[�����A�����[�h����
        yield return SceneManager.UnloadSceneAsync(currentScene);

        // �t�F�[�h�C��������
        yield return StartCoroutine(fadeController.In(null, unScaledTime));

        // �J�ډ��o�p�̃V�[�����A�����[�h����
        yield return SceneManager.UnloadSceneAsync(transitionScene);
    }

    private IEnumerator TransitionForReset(float loadWaitingTime, bool unScaledTime = true)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string resetSceneName = currentScene.name;

        LoadSceneParameters loadSceneParameters = new LoadSceneParameters();
        loadSceneParameters.loadSceneMode = LoadSceneMode.Additive;

        // �J�ډ��o�p�̃V�[�������[�h����
        Scene transitionScene = SceneManager.LoadScene(TransitionSceneName, loadSceneParameters);

        // 1�t���[���҂��Ȃ��ƃQ�[���I�u�W�F�N�g���擾�ł��Ȃ�
        yield return null;

        FadeController fadeController = GameObject.FindWithTag(FadeControllerTagName).GetComponent<FadeController>();

        // �t�F�[�h�A�E�g������
        yield return StartCoroutine(fadeController.Out(null, unScaledTime));

        // �O�̃V�[�����A�����[�h����
        yield return SceneManager.UnloadSceneAsync(currentScene);
        // �V�[�������[�h����
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(resetSceneName, LoadSceneMode.Additive);
        while (!loadAsync.isDone)
        {
            yield return null;
        }

        Scene loadScene = SceneManager.GetSceneByName(resetSceneName);
        SceneManager.SetActiveScene(loadScene);

        // �ǂݍ���ł��班���Ԃ��J����
        yield return new WaitForSecondsRealtime(loadWaitingTime);

        // �t�F�[�h�C��������
        yield return StartCoroutine(fadeController.In(null, unScaledTime));

        // �J�ډ��o�p�̃V�[�����A�����[�h����
        yield return SceneManager.UnloadSceneAsync(transitionScene);
    }
}
