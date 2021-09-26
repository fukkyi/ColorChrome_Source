using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MissionManager : BaseManager<MissionManager>
{
    private List<Mission> currentMissionList = new List<Mission>();

    /// <summary>
    /// �~�b�V������ǉ�����
    /// </summary>
    /// <param name="mission"></param>
    public void AddMission(Mission mission)
    {
        currentMissionList.Add(mission);
        mission.OnAdded();

        GameSceneUIManager.Instance.MissionList.UpdateMissionList();
    }

    /// <summary>
    /// �~�b�V����������������
    /// </summary>
    /// <param name="name"></param>
    public void ComplateMission(MissionName name, bool playSound = true)
    {
        Mission mission = GetMissionByName(name);

        if (mission == null) return;
        if (mission.complete) return;

        mission.Complete();

        GameSceneUIManager.Instance.MissionList.UpdateMissionList();
        StartCoroutine(GameSceneUIManager.Instance.MissionList.PlayClearAnim(name));

        if (playSound)
        {
            AudioManager.Instance.PlaySE("Win sound 16");
        }
    }

    /// <summary>
    /// �J�E���g���~�b�V�����̃J�E���g�����Z����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="count"></param>
    public void AddCountOfMission(MissionName name, int count)
    {
        Mission mission = GetMissionByName(name);
        if (mission == null) return;
        if (mission.GetType() != typeof(CountableMission)) return;

        CountableMission contableMission = (CountableMission)mission;
        contableMission.AddCount(count);

        if (contableMission.currentCount >= contableMission.targetCount)
        {
            ComplateMission(name);
        }
        else
        {
            GameSceneUIManager.Instance.MissionList.UpdateMissionList();
        }
    }

    /// <summary>
    /// �~�b�V��������菜��
    /// </summary>
    /// <param name="name"></param>
    public void RemoveMission(MissionName name)
    {
        Mission mission = GetMissionByName(name);

        if (mission == null) return;

        currentMissionList.Remove(mission);

        GameSceneUIManager.Instance.MissionList.UpdateMissionList();
    }

    /// <summary>
    /// �~�b�V���������Z�b�g����
    /// </summary>
    public void ResetMission()
    {
        currentMissionList.Clear();
    }

    /// <summary>
    /// ���݂̃~�b�V�����ꗗ���擾����
    /// </summary>
    /// <returns></returns>
    public List<Mission> GetCurrentMission()
    {
        return currentMissionList;
    }

    /// <summary>
    /// �~�b�V�����𖼑O����擾����
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Mission GetMissionByName(MissionName name)
    {
        Mission resultMission = null;
        foreach(Mission mission in currentMissionList)
        {
            if (mission.missionName != name) continue;

            resultMission = mission;
            break;
        }

        return resultMission;
    }
}

/*
public enum TrialMissionName
{
    MushEnemyKill,
    GotoMushVillage,
    KillMushBoss,
}
*/

/// <summary>
/// �~�b�V�����̖��O
/// </summary>
public enum MissionName
{
    ExitValley,
    FindKeoPrison,
    FindHyou,
    KillHyou,
    BackKeoPrison,
    killEnemies,
    BackKeoPrison2,
    KillRose
}

[Serializable]
public class Mission
{
    public MissionName missionName;
    public string title;
    [NonSerialized]
    public bool complete;
    public UnityEvent onCompleted;

    /// <summary>
    /// �~�b�V�����B��������
    /// </summary>
    public virtual void Complete()
    {
        if (complete) return;

        complete = true;
        onCompleted.Invoke();
    }

    /// <summary>
    /// �~�b�V�������ǉ����ꂽ���̏���
    /// </summary>
    public virtual void OnAdded()
    {

    }
}

[Serializable]
public class CountableMission : Mission
{
    public int targetCount;
    [NonSerialized]
    public int currentCount;

    /// <summary>
    /// �J�E���g�����Z����
    /// </summary>
    /// <param name="count"></param>
    public void AddCount(int count)
    {
        currentCount += count;
    }
}

[Serializable]
public class TowardMission : Mission
{
    public MissionTargetArea targetArea;

    public override void OnAdded()
    {
        base.OnAdded();
        // �ړI�n�A�C�R���̒Ǐ]�ڕW�ɂ���
        GameSceneUIManager.Instance.DestinationIcon.SetDestination(targetArea.transform);
        // �ړI�n�ɒ��������Ƀ~�b�V�����N���A�ɂ�����
        targetArea.AddReachedAction(() => { MissionManager.Instance.ComplateMission(missionName); });
        // �ړI�n�A�C�R����L��������
        targetArea.gameObject.SetActive(true);
    }

    public override void Complete()
    {
        if (complete) return;
        // �N���A���ɖړI�n�A�C�R���𖳌���������
        targetArea.gameObject.SetActive(false);
        GameSceneUIManager.Instance.DestinationIcon.UnSetDestination();

        complete = true;
        onCompleted.Invoke();
    }
}