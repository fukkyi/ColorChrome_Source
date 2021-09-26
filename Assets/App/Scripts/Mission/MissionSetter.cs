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

        // TODO: �����ƃX�}�[�g�Ȃ������l��������
        // �v�f���Ƀ~�b�V�������i��ł���
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
            // �ЂƂO�̃~�b�V�������N���A�����A�~�b�V������ǉ�������
            CurrentMissionCount = checkPoint.missionCount - 1;
            MissionManager.Instance.AddMission(missionList[CurrentMissionCount]);
            MissionManager.Instance.ComplateMission(missionList[CurrentMissionCount].missionName, false);

            IsCheckPointInit = false;
        }
    }

    /// <summary>
    /// ���̃~�b�V������ǉ�����
    /// </summary>
    public void AddNextMission()
    {
        if (CurrentMissionCount + 1 >= missionList.Count) return;

        CurrentMissionCount++;
        MissionManager.Instance.AddMission(missionList[CurrentMissionCount]);
    }

    /// <summary>
    /// ���{�X蹉Ђ�L���ɂ���
    /// </summary>
    public void EnableEnemyHyouka()
    {
        enemyHyouka.enabled = true;
    }

    /// <summary>
    /// ���{�X蹉Зp�̒ʍs�s�ɂ���ǂ̗L������؂�ւ���
    /// </summary>
    public void ActivateEnemyHyoukaMagicWall(bool enable)
    {
        enemyHyoukaMagicWall.SetActive(enable);
    }

    /// <summary>
    /// ���{�X蹉Ђ�|���~�b�V�������N���A�ɂ���
    /// </summary>
    public void CompleteKillHyouMission()
    {
        MissionManager.Instance.ComplateMission(killHyouMission.missionName);
    }

    /// <summary>
    /// �Q�[���r���ɏo�Ă���G���X�|�[��������
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
    /// ���[�Y�Ɛ키�Ƃ��ɓG���f�X�|�[��������
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
    /// ���X�{�X ���[�Y��L���ɂ���
    /// </summary>
    public void EnableRose()
    {
        enemyRose.enabled = true;
    }
    
    /// <summary>
    /// ���X�{�X ���[�Y��|���~�b�V�������N���A�ɂ���
    /// </summary>
    public void CompleteKillRoseMission()
    {
        MissionManager.Instance.ComplateMission(killRoseMission.missionName);
    }

    /// <summary>
    /// �G�s���[�O���Đ�����
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
