using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneManager : BaseSceneManager<GameSceneManager>
{
    [SerializeField]
    private PauseUI pauseUI = null;
    [SerializeField]
    private GameOverUI gameOverUI = null;
    [SerializeField]
    private TerrainMeshController terrainMeshController = null;

    [SerializeField]
    private ColorLevelProportionTable redLevelTable = null;
    public ColorLevelProportionTable RedLevelTable { get { return redLevelTable; } }
    [SerializeField]
    private ColorLevelProportionTable greenLevelTable = null;
    public ColorLevelProportionTable GreenLevelTable { get { return greenLevelTable; } }
    [SerializeField]
    private ColorLevelProportionTable blueLevelTable = null;
    public ColorLevelProportionTable BlueLevelTable { get { return blueLevelTable; } }

    [SerializeField]
    private ObjectPool grayExpFragmentPool = null;
    [SerializeField]
    private ObjectPool deadExpParticlePool = null;
    [SerializeField]
    private ObjectPool enemyMagicBulletPool = null;
    [SerializeField]
    private ObjectPool playerMagicBulletPool = null;
    [SerializeField]
    private ObjectPool afterImagePool = null;
    public ObjectPool AfterImagePool { get { return afterImagePool; } }
    [SerializeField]
    private ObjectPool playerMagicImpactPool = null;
    public ObjectPool PlayerMagicImpactPool { get { return playerMagicImpactPool; } }
    [SerializeField]
    private ObjectPool enemyMagicImpactPool = null;
    public ObjectPool EnemyMagicImpactPool { get { return enemyMagicImpactPool; } }
    [SerializeField]
    private ObjectPool colorFlakeGetParticlePool = null;
    public ObjectPool ColorFlakeGetParticlePool { get { return colorFlakeGetParticlePool; } }
    [SerializeField]
    private ObjectPool colorFlakeRedPool = null;
    public ObjectPool ColorFlakeRedPool { get { return colorFlakeRedPool; } }
    [SerializeField]
    private ObjectPool colorFlakeGreenPool = null;
    public ObjectPool ColorFlakeGreenPool { get { return colorFlakeGreenPool; } }
    [SerializeField]
    private ObjectPool colorFlakeBluePool = null;
    public ObjectPool ColorFlakeBluePool { get { return colorFlakeBluePool; } }

    public bool isGameClear = false;
    public bool isGameOver = false;

    [SerializeField]
    private float startBGMFadeInTime = 3.0f;
    [SerializeField]
    private float defaultTimeScale = 1.0f;
    [SerializeField]
    private float pauseTimeScale = 0;

    private bool isPausing = false;
    private float beforePauseTimeScale = 0;

    private void Start()
    {
        AudioManager.Instance.PlayBGMWithCrossFade("Plains_amenohinoneon", startBGMFadeInTime);
    }

    public void OnDestroy()
    {
        // ?|?[?Y?????????J???????????l???????^?C???X?P?[????????
        Time.timeScale = defaultTimeScale;
    }

    public ObjectPool GetGrayExpFragmentPool()
    {
        return grayExpFragmentPool;
    }

    public ObjectPool GetDeadExpParticlePool()
    {
        return deadExpParticlePool;
    }

    public ObjectPool GetEnemyMagicBulletPool()
    {
        return enemyMagicBulletPool;
    }

    public ObjectPool GetPlayerMagicBulletPool()
    {
        return playerMagicBulletPool;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed) TogglePause();
    }

    /// <summary>
    /// ?|?[?Y????????????????
    /// </summary>
    public void TogglePause()
    {
        if (pauseUI.isShowingOption) return;
        if (TalkCanvasManager.Instance.isEnableScenario) return;

        if (isPausing)
        {
            Time.timeScale = beforePauseTimeScale;
            pauseUI.HidePauseUI();
        }
        else
        {
            beforePauseTimeScale = Time.timeScale;
            Time.timeScale = pauseTimeScale;
            pauseUI.ShowPauseUI();
        }

        isPausing = !isPausing;
    }

    /// <summary>
    /// ?|?[?Y??????????
    /// </summary>
    /// <returns></returns>
    public bool isGamePausing()
    {
        return isPausing;
    }

    /// <summary>
    /// ?^?C?g????????
    /// </summary>
    public void BackTitle()
    {
        terrainMeshController.ResetGrayableDetail();
        SceneTransitionManager.Instance.StartTransitionByName(SceneTransitionManager.TitleSceneName);
    }

    /// <summary>
    /// ?Q?[???I?[?o?[??????
    /// </summary>
    public void ShowGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        AudioManager.Instance.PlayBGMWithCrossFade("natsunokiri", 2.0f);

        gameOverUI.Show();
    }

    /// <summary>
    /// ?N???A?V?[?????J??????
    /// </summary>
    public void TransitionClearScene()
    {
        terrainMeshController.ResetGrayableDetail();
        SceneTransitionManager.Instance.StartTransitionByName(SceneTransitionManager.ClearSceneName);
        AudioManager.Instance.StopCurrentBGMWithFade();
    }
}
