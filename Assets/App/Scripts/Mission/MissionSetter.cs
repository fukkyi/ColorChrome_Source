using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSetter : BaseSceneManager<MissionSetter>
{
    [SerializeField]
    private Enemy[] middleSpawnEnemies = null;
    [SerializeField]
    private Enemy[] roseDespawnEnemies = null;
    [SerializeField]
    private BossEnemyHyou enemyHyouka = null;
    [SerializeField]
    private GameObject enemyHyoukaMagicWall = null;
    [SerializeField]
    private BossEnemyRose enemyRose = null;
    [SerializeField]
    private GrayableCalculator grayableCalculator = null;

    public TowardMission exitValleyMission;
    public TowardMission findKeoPrisonMission;
    public TowardMission findHyouMission;
    public Mission killHyouMission;
    public TowardMission backKeoPrisonMission;
    public CountableMission killEnemiesMission;
    public TowardMission backKeoPrison2Mission;
    public Mission killRoseMission;

    public bool IsCheckPointInit { get; private set; } = false;
    public int CurrentMissionCount { get; private set; } = 0;

    private List<Mission> missionList = new List<Mission>();

    protected new void Awake()
    {
        base.Awake();

        // TODO: もっとスマートなやり方を考えたいね
        // 要素順にミッションが進んでいく
        Mission[] missions = new Mission[] {
            exitValleyMission,
            findKeoPrisonMission,
            findHyouMission,
            killHyouMission,
            backKeoPrisonMission,
            killEnemiesMission,
            backKeoPrison2Mission,
            killRoseMission
        };
        missionList.AddRange(missions);
    }

    private void Start()
    {
        MissionManager.Instance.ResetMission();

        CheckPoint checkPoint = GamePlayDataManager.currentCheckPoint;
        if (checkPoint == null || checkPoint.missionCount <= 0)
        {
            MissionManager.Instance.AddMission(exitValleyMission);
        }
        else
        {
            IsCheckPointInit = true;
            // ひとつ前のミッションをクリアさせ、ミッションを追加させる
            CurrentMissionCount = checkPoint.missionCount - 1;
            MissionManager.Instance.AddMission(missionList[CurrentMissionCount]);
            MissionManager.Instance.ComplateMission(missionList[CurrentMissionCount].missionName, false);

            IsCheckPointInit = false;
        }
    }

    /// <summary>
    /// 次のミッションを追加する
    /// </summary>
    public void AddNextMission()
    {
        if (CurrentMissionCount + 1 >= missionList.Count) return;

        CurrentMissionCount++;
        MissionManager.Instance.AddMission(missionList[CurrentMissionCount]);
    }

    /// <summary>
    /// 中ボス雹禍を有効にする
    /// </summary>
    public void EnableEnemyHyouka()
    {
        enemyHyouka.enabled = true;
    }

    /// <summary>
    /// 中ボス雹禍用の通行不可にする壁の有効化を切り替える
    /// </summary>
    public void ActivateEnemyHyoukaMagicWall(bool enable)
    {
        enemyHyoukaMagicWall.SetActive(enable);
    }

    /// <summary>
    /// 中ボス雹禍を倒すミッションをクリアにする
    /// </summary>
    public void CompleteKillHyouMission()
    {
        MissionManager.Instance.ComplateMission(killHyouMission.missionName);
    }

    /// <summary>
    /// ゲーム途中に出てくる敵をスポーンさせる
    /// </summary>
    public void SpawnMiddleEnemies()
    {
        foreach(Enemy enemy in middleSpawnEnemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.enabled = true;
        }
    }

    /// <summary>
    /// ローズと戦うときに敵をデスポーンさせる
    /// </summary>
    public void DespawnRoseEnemies()
    {
        foreach(Enemy enemy in roseDespawnEnemies)
        {
            if (enemy == null) continue;

            enemy.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ラスボス ローズを有効にする
    /// </summary>
    public void EnableRose()
    {
        enemyRose.enabled = true;
    }
    
    /// <summary>
    /// ラスボス ローズを倒すミッションをクリアにする
    /// </summary>
    public void CompleteKillRoseMission()
    {
        MissionManager.Instance.ComplateMission(killRoseMission.missionName);
    }

    /// <summary>
    /// エピローグを再生する
    /// </summary>
    public void PlayEpilogue()
    {
        GameSceneManager.Instance.isGameClear = true;

        StartCoroutine(ComplateGame());
    }

    private IEnumerator ComplateGame()
    {
        yield return new WaitForSeconds(1.0f);

        if (GamePlayDataManager.IsValidHappyEnding())
        {
            TalkCanvasManager.Instance.ShowHappyEpilogueScenario();
        }
        else
        {
            TalkCanvasManager.Instance.ShowBadEpilogueScenario();
        }

        yield return new WaitForSeconds(0.5f);

        GamePlayDataManager.grayRate = grayableCalculator.GrayableRate;
        GameSceneManager.Instance.TransitionClearScene();
    }
}
